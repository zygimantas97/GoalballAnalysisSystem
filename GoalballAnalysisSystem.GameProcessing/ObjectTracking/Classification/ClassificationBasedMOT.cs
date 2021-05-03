using Emgu.CV;
using GoalballAnalysisSystem.GameProcessing.Geometry;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking.Classification.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectTracking.Classification
{
    public class ClassificationBasedMOT<T> : IMOT<T> where T : class
    {
        private readonly List<TrackingObject<T>> _trackingObjects = new List<TrackingObject<T>>();
        private readonly IObjectDetector _objectDetector;

        public ClassificationBasedMOT(IObjectDetector objectDetector)
        {
            _objectDetector = objectDetector;
        }

        public async Task<Dictionary<T, Rectangle>> Update(Mat frame)
        {
            var boundingBoxes = new Dictionary<T, Rectangle>();

            if (_trackingObjects.Count > 0)
            {
                var detectedCategories = await _objectDetector.Detect(frame);

                foreach (var key in detectedCategories.Keys)
                {
                    var category = detectedCategories[key];
                    foreach(var detectedObject in category)
                    {
                        int objectIndex = DetermineTrackingObject(_trackingObjects, detectedObject);
                        _trackingObjects[objectIndex].AddPredictionBoundary(detectedObject);
                    }
                }

                foreach (var trackingObject in _trackingObjects)
                {
                    trackingObject.DetermineAndUpdateMostFittingPrediction();
                    trackingObject.ClearPredictionBoundaries();
                    boundingBoxes[trackingObject.Object] = trackingObject.BoundingBox;
                }

                _trackingObjects[0].ClearPredictionBoundaries();
            }

            return boundingBoxes;
        }

        public void Add(T obj, Mat frame, Rectangle roi)
        {
            var trackingObject = new TrackingObject<T>(obj, roi);

            _trackingObjects.Add(trackingObject);
        }

        private int DetermineTrackingObject(List<TrackingObject<T>> trackedObjects, Rectangle boundary)
        {
            Dictionary<int, double> distances = new Dictionary<int, double>();
            int counter = 0;
            int indexValue = -1;

            foreach (var trackedObject in trackedObjects)
            {
                double dist = 0;
                for (int i = 1; i < 3; i++)
                {
                    dist += trackedObject.DistanceToPreviousPoint(boundary, i);
                }
                distances.Add(counter, dist);
                counter++;
            }

            if (distances.Count > 0)
            {
                var keyAndValue = distances.OrderBy(kvp => kvp.Value).First();
                indexValue = keyAndValue.Key;
            }

            return indexValue;
        }

        public void Remove(T obj)
        {
            _trackingObjects.RemoveAll(to => to.Object == obj);
        }

        public void RemoveAt(Point location)
        {
            if (_trackingObjects.Count > 0)
            {
                double minDistance = Calculations.GetDistanceBetweenPoints(location, Calculations.GetMiddlePoint(_trackingObjects[0].BoundingBox));
                int index = 0;
                for (int i = 1; i < _trackingObjects.Count; i++)
                {
                    double distance = Calculations.GetDistanceBetweenPoints(location, Calculations.GetMiddlePoint(_trackingObjects[i].BoundingBox));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        index = i;
                    }
                }
                _trackingObjects.RemoveAt(index);
            }
        }

        public void RemoveAll()
        {
            _trackingObjects.Clear();
        }
    }
}
