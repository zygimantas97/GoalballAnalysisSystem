using System.Drawing;

namespace GoalballAnalysisSystem.GameProcessing.Models
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