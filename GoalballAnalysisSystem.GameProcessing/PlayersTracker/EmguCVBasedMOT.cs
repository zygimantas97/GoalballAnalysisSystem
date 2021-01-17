using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Emgu.CV.Tracking;
using Emgu.CV.Util;
using GoalballAnalysisSystem.GameProcessing.Models;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public class EmguCVBasedMOT : IMOT
    {
        private readonly MultiTracker _multiTracker = new MultiTracker();
        private readonly Tracker _tracker = new TrackerCSRT();
        private readonly List<TrackingObject> _trackingObjects = new List<TrackingObject>();

        public void AddTrackingObject(Mat frame, Rectangle roi, int objectId = 0)
        {
            Tracker tracker = new TrackerCSRT();
            tracker.Init(frame, roi);
            TrackingObject trackingObject = new TrackingObject
            {
                ObjectId = objectId,
                ObjectTracker = tracker,
                ROI = roi
            };
            _trackingObjects.Add(trackingObject);

            //_tracker.Init(frame, roi);
            
            // System.AccessViolationException: 'Attempted to read or write...
            // Kai bandoma prideti nauja tracker _multiTracker.Add()
            //Tracker tracker = new TrackerBoosting();

            // System.AccessViolationException: 'Attempted to read or write...
            // Kai bandoma kviesti _multiTracker.Update()
            //Tracker tracker = new TrackerCSRT();

            // Emgu.CV.Util.CvException: 'OpenCV: FAILED: fs.is_open(). Can't open...
            // Kai bandoma prideti nauja tracker _multiTracker.Add()
            //Tracker tracker = new TrackerGOTURN();

            // System.AccessViolationException: 'Attempted to read or write...
            // Kai bandoma kviesti _multiTracker.Update()
            //Tracker tracker = new TrackerKCF();

            // Netestuota, nes reikalauja papildomu parametru
            //Tracker tracker = new TrackerMedianFlow(;

            // System.AccessViolationException: 'Attempted to read or write...
            // Kai bandoma prideti nauja tracker _multiTracker.Add()
            //Tracker tracker = new TrackerMIL();

            // System.AccessViolationException: 'Attempted to read or write...
            // Kai bandoma kviesti _multiTracker.Update()
            //Tracker tracker = new TrackerMOSSE();

            // System.AccessViolationException: 'Attempted to read or write...
            // Kai bandoma kviesti _multiTracker.Update()
            //Tracker tracker = new TrackerTLD();

            //_multiTracker.Add(tracker, frame, roi);
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
            /*
            VectorOfRect vectorOfRect = new VectorOfRect();
            bool success = _multiTracker.Update(frame, vectorOfRect);
            if (success)
            {
                return vectorOfRect.ToArray();
            }
            */

            /*
            Rectangle rec = new Rectangle();
            bool success = _tracker.Update(frame, out rec);
            if (success)
            {
                return new Rectangle[] { rec };
            }
            */

            List<Rectangle> rois = new List<Rectangle>();

            Parallel.ForEach(_trackingObjects, trackingObject =>
            {
                trackingObject.Update(frame);
                rois.Add(trackingObject.ROI);
            });

            return rois;
        }
    }
}
