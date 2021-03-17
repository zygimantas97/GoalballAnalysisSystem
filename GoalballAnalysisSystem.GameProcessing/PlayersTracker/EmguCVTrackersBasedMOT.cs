using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Emgu.CV.Tracking;
using Emgu.CV.Util;
using GoalballAnalysisSystem.GameProcessing.Models;
using System.Threading.Tasks;
using System.Linq;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public class EmguCVTrackersBasedMOT : IMOT
    {
        private readonly MultiTracker _multiTracker = new MultiTracker();
        private readonly Tracker _tracker = new TrackerCSRT();
        private readonly List<TrackingObject> _trackingObjects = new List<TrackingObject>();

        public int FramesCount { get; private set; } = 0;
        public int DetectedAnyCount { get; private set; } = 0;
        public int DetectedAllCount { get; private set; } = 0;
        public Dictionary<int, int> ObjectDetectionCounts { get; private set; } = new Dictionary<int, int>();

        public void AddTrackingObject(Mat frame, Rectangle roi, int objectId = 0)
        {
            // For testing purposes
            ObjectDetectionCounts.Add(objectId, 0);

            Tracker tracker;

            //tracker = new TrackerBoosting();
            //tracker= new TrackerCSRT();
            //tracker = new TrackerGOTURN();
            //tracker = new TrackerKCF();
            //tracker = new TrackerMIL();
            //tracker = new TrackerMOSSE();
            tracker = new TrackerTLD();




            tracker.Init(frame, roi);
            TrackingObject trackingObject = new TrackingObject
            {
                ObjectId = objectId,
                ObjectTracker = tracker,
                ROI = roi
            };
            _trackingObjects.Add(trackingObject);
        }

        public void RemoveTrackingObjectById(long id)
        {
            _trackingObjects.RemoveAll(trackingObject => trackingObject.ObjectId == id);
        }

        public void RemoveTrackingObjectByLocation(Point location)
        {
            if(_trackingObjects.Count > 0)
            {
                double minDistance = _trackingObjects[0].GetDistance(location);
                int index = 0;
                for(int i = 1; i < _trackingObjects.Count; i++)
                {
                    double distance = _trackingObjects[i].GetDistance(location);
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        index = i;
                    }
                }
                _trackingObjects.RemoveAt(index);
            }
        }

        public void RemoveAllTrackingObjects()
        {
            _trackingObjects.Clear();
        }

        public List<Rectangle> UpdateTrackingObjects(Mat frame)
        {
            // For testing implement success checks

            List<Rectangle> rois = new List<Rectangle>();

            Parallel.ForEach(_trackingObjects, trackingObject =>
            {
                var rect = trackingObject.Update(frame);
                if (rect != Rectangle.Empty)
                {
                    ObjectDetectionCounts[trackingObject.ObjectId]++;
                }
                rois.Add(rect);
            });

            // For testing only
            if(rois.Count > 0)
            {
                if (rois.All(r => r != Rectangle.Empty))
                {
                    DetectedAllCount++;
                }

                if (rois.Any(r => r != Rectangle.Empty))
                {
                    DetectedAnyCount++;
                }
            }
            
            FramesCount++;

            return rois;
        }
    }
}
