using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public interface IMOT
    {
        Rectangle[] UpdateTrackObjects(Mat frame);
    }
}
