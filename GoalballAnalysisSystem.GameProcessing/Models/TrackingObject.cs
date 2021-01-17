using Emgu.CV;
using Emgu.CV.Tracking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Models
{
    public class TrackingObject
    {
        public long ObjectId { get; set; }
        public Tracker ObjectTracker { get; set; }
        public Rectangle ROI { get; set; }

        public void Update(Mat frame)
        {
            Rectangle rec = new Rectangle();
            bool success = ObjectTracker.Update(frame, out rec);
            if (success)
            {
                ROI = rec;
            }
        }

        public double GetDistance(Point location)
        {
            int cx = ROI.X + ROI.Width / 2;
            int cy = ROI.Y + ROI.Height / 2;
            return Math.Pow((Math.Pow((cx - location.X), 2) + Math.Pow((cy - location.Y), 2)), 0.5);
        }
    }
}
