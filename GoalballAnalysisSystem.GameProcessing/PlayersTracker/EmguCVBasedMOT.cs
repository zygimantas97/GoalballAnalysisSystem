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
            //Tracker tracker = new TrackerCSRT();
            //_multiTracker.Add(tracker, frame, roi);
        }

        public Rectangle[] UpdateTrackObjects(Mat frame)
        {
            VectorOfRect vectorOfRect = new VectorOfRect();
            Rectangle rec = new Rectangle();
            bool success = _tracker.Update(frame, out rec);
            if (success)
            {
                return new Rectangle[] { rec };
            }
            /*
            bool success =_multiTracker.Update(frame, vectorOfRect);

            if (success)
            {
                return vectorOfRect.ToArray();
            }*/
            return null;
        }
    }
}
