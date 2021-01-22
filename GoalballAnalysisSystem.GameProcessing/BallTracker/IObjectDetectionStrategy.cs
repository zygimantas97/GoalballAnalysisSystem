using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.BallTracker
{
    public interface IObjectDetectionStrategy
    {
        Rectangle DetectObject(Mat frame);
    }
}
