using Emgu.CV;
using Emgu.CV.Tracking;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking.SOT.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectTracking.SOT
{
    public class SOTBasedMOT<T> : IMOT<T> where T : class
    {
        private readonly List<TrackingObject<T>> _trackingObjects = new List<TrackingObject<T>>();
        private readonly TrackerType _trackerType;

        public SOTBasedMOT(TrackerType trackerType = TrackerType.KCF)
        {
            _trackerType = trackerType;
        }

        public void Add(T obj, Mat frame, Rectangle roi)
        {
            var tracker = CreateTracker(_trackerType);
            tracker.Init(frame, roi);

            var trackingObject = new TrackingObject<T>(obj, tracker, roi);
            
            _trackingObjects.Add(trackingObject);
        }

        public async Task<Dictionary<T, Rectangle>> Update(Mat frame)
        {
            Dictionary<T, Rectangle> boundingBoxes = new Dictionary<T, Rectangle>();

            Parallel.ForEach(_trackingObjects, trackingObject =>
            {
                trackingObject.Update(frame);

                boundingBoxes[trackingObject.Object] = trackingObject.BoundingBox;
            });

            return boundingBoxes;
        }

        public void Remove(T obj)
        {
            _trackingObjects.RemoveAll(to => to.Object == obj);
        }

        public void RemoveAt(Point location)
        {
            if(_trackingObjects.Count > 0)
            {
                double minDistance = GetDistanceBetweenPoints(location, _trackingObjects[0].Center);
                int index = 0;
                for (int i = 1; i < _trackingObjects.Count; i++)
                {
                    double distance = GetDistanceBetweenPoints(location, _trackingObjects[i].Center);
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

        private static Tracker CreateTracker(TrackerType trackerType)
        {
            Tracker tracker = null;

            switch (trackerType)
            {
                case TrackerType.Boosting:
                    tracker = new TrackerBoosting();
                    break;
                case TrackerType.CSRT:
                    tracker = new TrackerCSRT();
                    break;
                case TrackerType.GOTURN:
                    tracker = new TrackerGOTURN();
                    break;
                case TrackerType.KCF:
                    tracker = new TrackerKCF();
                    break;
                case TrackerType.MIL:
                    tracker = new TrackerMIL();
                    break;
                case TrackerType.MOOSE:
                    tracker = new TrackerMOSSE();
                    break;
                case TrackerType.TLD:
                    tracker = new TrackerTLD();
                    break;
            }

            return tracker;
        }

        private static double GetDistanceBetweenPoints(Point point1, Point point2)
        {
            double diffX = point1.X - point2.X;
            double diffY = point1.Y - point2.Y;
            double distance = Math.Pow((Math.Pow(diffX, 2) + Math.Pow(diffY, 2)), 0.5);
            return distance;
        }
    }
}
