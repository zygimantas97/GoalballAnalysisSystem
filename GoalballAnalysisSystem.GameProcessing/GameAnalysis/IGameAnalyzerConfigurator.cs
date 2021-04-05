using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.GameAnalysis
{
    public interface IGameAnalyzerConfigurator
    {
        Point BottomLeft { get; }
        Point BottomRight { get; }
        Point TopLeft { get; }
        Point TopRight { get; }

        Point GetPlaygroundOXY(Point point);
        bool IsPointInZoneOfInterest(Point point);
    }
}
