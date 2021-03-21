using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.Color.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.Color
{
    public class ColorObjectDetector : IObjectDetector
    {
        private readonly string _label;

        private readonly int _hueMin;
        private readonly int _hueMax;
        private readonly int _saturationMin;
        private readonly int _saturationMax;
        private readonly int _valueMin;
        private readonly int _valueMax;

        private readonly int _minObjectSize;
        private readonly int _maxObjectSize;

        public ColorObjectDetector(string label = "object",
            int hueMin = 94, int hueMax = 118,
            int saturationMin = 100, int saturationMax = 250,
            int valueMin = 98, int valueMax = 255,
            int minObjectSize = 200, int maxObjectSize = 1900)
        {
            _label = label;

            _hueMin = hueMin;
            _hueMax = hueMax;
            _saturationMin = saturationMin;
            _saturationMax = saturationMax;
            _valueMin = valueMin;
            _valueMax = valueMax;

            _minObjectSize = minObjectSize;
            _maxObjectSize = maxObjectSize;
        }

        public async Task<Dictionary<string, List<Rectangle>>> Detect(Mat frame)
        {
            var detectedObjects = new Dictionary<string, List<Rectangle>>();

            var frameHSV = new Mat();
            CvInvoke.CvtColor(frame, frameHSV, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            var treshold = new Mat();
            CvInvoke.InRange(frameHSV,
                             new ScalarArray(new MCvScalar(_hueMin, _saturationMin, _valueMin)),
                             new ScalarArray(new MCvScalar(_hueMax, _saturationMax, _valueMax)),
                             treshold);

            var objectsFilterMask = RemoveNoise(treshold);

            detectedObjects[_label] = DetectFromMask(objectsFilterMask);
            return detectedObjects;
        }

        private Mat RemoveNoise(Mat frame)
        {
            Mat erodeElement = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle,
                                                              new System.Drawing.Size(ModelSettings.ErodeElementSize, ModelSettings.ErodeElementSize),
                                                              new System.Drawing.Point(-1, -1));  //Starting from the center
            Mat dilateElement = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle,
                                                               new System.Drawing.Size(ModelSettings.DilateElementSize, ModelSettings.DilateElementSize),
                                                               new System.Drawing.Point(-1, -1)); //Starting from the center
            var frameAfterErode = new Mat();
            CvInvoke.Erode(frame, frameAfterErode, erodeElement, new Point(-1, -1), ModelSettings.ErodeIterations, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(255, 255, 255));
            
            var frameAfterDilate = new Mat();
            CvInvoke.Dilate(frameAfterErode, frameAfterDilate, dilateElement, new Point(-1, -1), ModelSettings.DilateIterations, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(255, 255, 255));

            return frameAfterDilate;
        }

        private List<Rectangle> DetectFromMask(Mat mask)
        {
            var rectangles = new List<Rectangle>();

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(); //all contours that could be found in mask
            Mat hierarchy = new Mat();

            CvInvoke.FindContours(mask, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                var contourArea = CvInvoke.ContourArea(contours[i]);
                if (contourArea > _minObjectSize && contourArea < _maxObjectSize)
                {
                    rectangles.Add(CvInvoke.BoundingRectangle(contours[i]));
                }
            }

            return rectangles;
        }
    }
}
