using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Emgu.CV.Tracking;
using Emgu.CV.Util;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public class EmguCVBasedMOT : IMOT
    {
        private readonly MultiTracker _multiTracker = new MultiTracker();
        private readonly Tracker _tracker = new TrackerCSRT();

        public void AddTrackObject(Mat frame, Rectangle roi)
        {
            _tracker.Init(frame, roi);

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

        public Rectangle[] UpdateTrackObjects(Mat frame)
        {
            /*
            VectorOfRect vectorOfRect = new VectorOfRect();
            bool success = _multiTracker.Update(frame, vectorOfRect);
            if (success)
            {
                return vectorOfRect.ToArray();
            }
            */
            
            Rectangle rec = new Rectangle();
            bool success = _tracker.Update(frame, out rec);
            if (success)
            {
                return new Rectangle[] { rec };
            }

            return null;
        }
    }
}
