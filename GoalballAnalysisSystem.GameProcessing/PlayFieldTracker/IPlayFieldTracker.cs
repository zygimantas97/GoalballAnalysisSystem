using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.PlayFieldTracker
{
    public interface IPlayFieldTracker
    {
        Point[] GetPlayFieldCorners(Mat cameraFeed);
        Mat GetPlayFieldMask(Mat cameraFeed);
    }
}
