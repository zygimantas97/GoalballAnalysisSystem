using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.BallTracker
{
    public class ColorBasedObjectDetectionStrategy : IObjectDetectionStrategy
    {
        private Mat cameraFeedHSV;
        private Mat treshold;
        private Mat objectsFilterMask;

        public ColorBasedObjectDetectionStrategy()
        {
            cameraFeedHSV = new Mat(); ;
            treshold = new Mat();
            objectsFilterMask = new Mat();
        }

        public Rectangle DetectObject(Mat cameraFeed)
        {
            //conversion to HSV format
            CvInvoke.CvtColor(cameraFeed, cameraFeedHSV, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
            
            //HSV image filtering with given values
            CvInvoke.InRange(cameraFeedHSV, 
                             new ScalarArray(new MCvScalar(FilterParameters.Hue.Min, FilterParameters.Saturation.Min, FilterParameters.Value.Min)), //Minimum range
                             new ScalarArray(new MCvScalar(FilterParameters.Hue.Max, FilterParameters.Saturation.Max, FilterParameters.Value.Max)), //Maximum range
                             treshold);  //Treshold

            objectsFilterMask = NoiseReduction(treshold);
            return BallCoordinatesFromMask(objectsFilterMask);
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

        private Rectangle BallCoordinatesFromMask(Mat mask)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(); //all contours that could be found in mask
            VectorOfVectorOfPoint eligibleContours = new VectorOfVectorOfPoint(); //contours that fits all parameters
            Mat hierarchy = new Mat();
            bool ballWasFound = false;

            CvInvoke.FindContours(mask, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            
            if(contours.Size > 0)
            {
                for (int i=0; i< contours.Size; i++)
                {
                    if(CvInvoke.ContourArea(contours[i]) > FilterParameters.BallSize.Min && 
                       CvInvoke.ContourArea(contours[i]) < FilterParameters.BallSize.Max) //dar reikia patikrinimo del aikstes zonos veliau
                    {
                        eligibleContours.Push(contours[i]);
                        ballWasFound = true;
                    }
                }
            }

            if (ballWasFound)
                return CvInvoke.BoundingRectangle(eligibleContours[0]); //returns a first contour corner coordinates
            else
                return Rectangle.Empty; //if nothing was found
        }
    }
}
