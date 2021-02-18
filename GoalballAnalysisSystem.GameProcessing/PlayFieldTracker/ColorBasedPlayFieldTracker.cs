using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Drawing;

namespace GoalballAnalysisSystem.GameProcessing.PlayFieldTracker
{
    public class ColorBasedPlayFieldTracker : IPlayFieldTracker
    {
        private Mat cameraFeedHSV;
        private Mat treshold;
        public ColorBasedPlayFieldTracker()
        {
            cameraFeedHSV = new Mat();
            treshold = new Mat();
            //ObjectsFilterMask = new Mat();
        }

        public Point[] GetPlayFieldCorners(Mat cameraFeed)
        {
            CvInvoke.CvtColor(cameraFeed, cameraFeedHSV, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
            Trace.WriteLine(cameraFeed);
            //HSV image filtering with given values
            CvInvoke.InRange(cameraFeedHSV,
                             new ScalarArray(new MCvScalar(FilterParameters.Hue.Min, FilterParameters.Saturation.Min, FilterParameters.Value.Min)), //Minimum range
                             new ScalarArray(new MCvScalar(FilterParameters.Hue.Max, FilterParameters.Saturation.Max, FilterParameters.Value.Max)), //Maximum range
                             treshold);  //Treshold

            return FindCornerCoordinates( NoiseReduction( treshold) );
        }

        public Mat GetPlayFieldMask(Mat cameraFeed)
        {
            CvInvoke.CvtColor(cameraFeed, cameraFeedHSV, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            //HSV image filtering with given values
            CvInvoke.InRange(cameraFeedHSV,
                             new ScalarArray(new MCvScalar(FilterParameters.Hue.Min, FilterParameters.Saturation.Min, FilterParameters.Value.Min)), //Minimum range
                             new ScalarArray(new MCvScalar(FilterParameters.Hue.Max, FilterParameters.Saturation.Max, FilterParameters.Value.Max)), //Maximum range
                             treshold);  //Treshold

            return NoiseReduction(treshold);
        }

        private Mat NoiseReduction(Mat cameraFeed)
        {
            Mat erodeElement = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle,
                                                              new System.Drawing.Size(FilterParameters.StructElementSizeErode, FilterParameters.StructElementSizeErode),
                                                              new System.Drawing.Point(-1, -1));  //Starting from the center
            Mat dilateElement = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle,
                                                               new System.Drawing.Size(FilterParameters.StructElementSizeDilate, FilterParameters.StructElementSizeDilate),
                                                               new System.Drawing.Point(-1, -1)); //Starting from the center

            CvInvoke.Erode(cameraFeed, cameraFeed, erodeElement, new Point(-1, -1), FilterParameters.ErodeIterations, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(255, 255, 255));
            CvInvoke.Dilate(cameraFeed, cameraFeed, dilateElement, new Point(-1, -1), FilterParameters.DilateIterations, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(255, 255, 255));

            return cameraFeed;
        }

        private Point[] FindCornerCoordinates(Mat mask)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(); //all contours that could be found in mask
            VectorOfVectorOfPoint eligibleContours = new VectorOfVectorOfPoint(); //contours that fits all parameters
            Mat hierarchy = new Mat();

            CvInvoke.FindContours(mask, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
     
            Point Top = new Point(int.MaxValue, int.MaxValue);
            VectorOfPoint approxContour = new VectorOfPoint();
            // CvInvoke.ApproxPolyDP(contours[0], approxContour, CvInvoke.ArcLength(contours[0], true) * 0.01, true);

            VectorOfVectorOfPoint accepted = new VectorOfVectorOfPoint();
            int maxsize = -1;
            int acc = -1;

            for (int i=0; i< contours.Size; i++)
            {
                if(contours[i].Size > maxsize)
                {
                    maxsize = contours[i].Size;
                    acc = i;
                }
            }

            CvInvoke.ApproxPolyDP(contours[acc], approxContour, CvInvoke.ArcLength(contours[acc], true) * 0.05, true);

            //Rectangle rect = new Rectangle(leftx, lefty, 30, 30);
            //CvInvoke.Rectangle(mask, rect, new MCvScalar(0, 0, 255), 5);

            //rect = new Rectangle(rightx, righty, 30, 30);
            //CvInvoke.Rectangle(mask, rect, new MCvScalar(0, 0, 255), 5);
            return approxContour.ToArray();
            
        }

    }
}
