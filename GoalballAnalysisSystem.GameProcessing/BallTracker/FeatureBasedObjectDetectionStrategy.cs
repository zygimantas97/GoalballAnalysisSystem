using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.BallTracker
{
    public class FeatureBasedObjectDetectionStrategy : IObjectDetectionStrategy
    {
        private readonly Image<Gray, byte> _template;
        private readonly VectorOfKeyPoint _templateKeyPoints = new VectorOfKeyPoint();
        private readonly Mat _templateDescriptor = new Mat();
        private readonly Feature2D _featureDetector;
        private readonly DescriptorMatcher _matcher;
        private readonly int _k;
        private readonly double _uniquenessTreshold;

        public FeatureBasedObjectDetectionStrategy(Image<Gray, byte> template, int k = 2, double uniquenessTreshold = 0.8)
        {
            _featureDetector = new Brisk();
            //_featureDetector = new KAZE();
            //_featureDetector = new GFTTDetector();
            //_featureDetector = new FastFeatureDetector();
            //_featureDetector = new ORBDetector();
            //_featureDetector = new MSERDetector();
            
            // Brute Force matcher alternative
            _matcher = new BFMatcher(DistanceType.Hamming);

            /*
            // FLANN matcher alternative
            KdTreeIndexParams indexParams = new KdTreeIndexParams();
            AutotunedIndexParams indexParams = new AutotunedIndexParams();
            LinearIndexParams indexParams = new LinearIndexParams();
            KMeansIndexParams indexParams = new KMeansIndexParams();
            SearchParams searchParams = new SearchParams();
            _matcher = new FlannBasedMatcher(indexParams, searchParams);
            */

            _template = template;
            _featureDetector.DetectAndCompute(_template, null, _templateKeyPoints, _templateDescriptor, false);
            _matcher.Add(_templateDescriptor);
            
            _k = k;
            _uniquenessTreshold = uniquenessTreshold;
        }

        public Rectangle DetectObject(Mat frame)
        {
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
                return CvInvoke.BoundingRectangle(vectorOfDetectedObjectPoints);
            }
            return Rectangle.Empty;
        }
    }
}
