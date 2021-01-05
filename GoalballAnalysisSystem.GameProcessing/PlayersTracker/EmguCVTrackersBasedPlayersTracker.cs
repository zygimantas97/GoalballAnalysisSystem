using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.PlayersTracker
{
    public class EmguCVTrackersBasedPlayersTracker : IPlayersTracker
    {
        public List<Point> GetPlayersPositions(Mat cameraFeed)
        {
            throw new NotImplementedException();
        }
    }
}
