using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public interface IMOT
    {
        List<Rectangle> UpdateTrackingObjects(Mat frame);
        public void AddTrackingObject(Mat frame, Rectangle roi, int objectId = 0);
    }
}
