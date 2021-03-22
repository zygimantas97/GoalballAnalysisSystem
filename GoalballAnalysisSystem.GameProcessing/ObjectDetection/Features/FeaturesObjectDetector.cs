using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.Features
{
    public class FeaturesObjectDetector : IObjectDetector
    {
        private readonly Image<Gray, byte> _template;
        private readonly VectorOfKeyPoint _templateKeyPoints = new VectorOfKeyPoint();
        private readonly Mat _templateDescriptor = new Mat();
        private readonly Feature2D _featureDetector;
        private readonly DescriptorMatcher _matcher;
        private readonly int _k;
        private readonly double _uniquenessTreshold;
        private readonly string _label;

        public FeaturesObjectDetector(Image<Gray, byte> template, string label = "object", int k = 2, double uniquenessTreshold = 0.8)
        {
            _featureDetector = new Brisk();

            _matcher = new BFMatcher(DistanceType.Hamming);

            _template = template;
            _featureDetector.DetectAndCompute(_template, null, _templateKeyPoints, _templateDescriptor, false);
            _matcher.Add(_templateDescriptor);

            _k = k;
            _uniquenessTreshold = uniquenessTreshold;
            _label = label;
        }

        public async Task<Dictionary<string, List<Rectangle>>> Detect(Mat frame)
        {
            var detectedObjects = new Dictionary<string, List<Rectangle>>();
            detectedObjects[_label] = new List<Rectangle>();

            // Initialize variables
            VectorOfPoint vectorOfDetectedObjectPoints = null;
            Mat homography = null;
            VectorOfKeyPoint imageKeyPoints = new VectorOfKeyPoint();
            Mat imageDescriptor = new Mat();
            Mat mask;
            VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
            Image<Gray, byte> image = frame.ToImage<Gray, byte>();

            // Detect features of given image
            _featureDetector.DetectAndCompute(image, null, imageKeyPoints, imageDescriptor, false);

            // Match features of template image and features of given image
            _matcher.KnnMatch(imageDescriptor, matches, _k);

            // Create mask for matches
            mask = new Mat(matches.Size, 1, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            // Filter unnecessary matches
            Features2DToolbox.VoteForUniqueness(matches, _uniquenessTreshold, mask);
            int count = Features2DToolbox.VoteForSizeAndOrientation(_templateKeyPoints, imageKeyPoints, matches, mask, 1.5, 20);

            // Create homography between template and given image key points
            if (count >= 4)
            {
                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(_templateKeyPoints, imageKeyPoints, matches, mask, 5);
            }

            // Transform template key points using homography
            if (homography != null)
            {
                Rectangle rect = new Rectangle(Point.Empty, _template.Size);
                PointF[] pts = new PointF[]
                {
                    new PointF(rect.Left, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Right, rect.Top),
                    new PointF(rect.Left, rect.Top)
                };
                pts = CvInvoke.PerspectiveTransform(pts, homography);
                Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
                vectorOfDetectedObjectPoints = new VectorOfPoint(points);
            }

            // Create bounding rectangle for detected object
            if (vectorOfDetectedObjectPoints != null)
            {
                detectedObjects[_label].Add(CvInvoke.BoundingRectangle(vectorOfDetectedObjectPoints));
            }

            return detectedObjects;
        }
    }
}
