using Emgu.CV;
using Emgu.CV.Tracking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectTracking.SOT.Models
{
    public class TrackingObject<T> where T : class
    {
        public T Object { get; private set; }
        public Tracker Tracker { get; private set; }
        public Rectangle BoundingBox { get; private set; }
        public int FailCount { get; private set; } = 0;

        public Point Center
        {
            get
            {
                return new Point(BoundingBox.X + BoundingBox.Width / 2, BoundingBox.Y + BoundingBox.Height / 2);
            }
        }


        public TrackingObject(T obj, Tracker tracker, Rectangle boundingBox)
        {
            Object = obj;
            Tracker = tracker;
            BoundingBox = boundingBox;
        }

        public Rectangle Update(Mat frame)
        {
            Rectangle rec = new Rectangle();
            bool success = Tracker.Update(frame, out rec);
            if (success)
            {
                BoundingBox = rec;
                FailCount = 0;
                return rec;
            }
            FailCount++;
            return Rectangle.Empty;
        }
    }
}
