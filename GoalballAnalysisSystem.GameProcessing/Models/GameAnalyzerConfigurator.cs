using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Models
{
    public class GameAnalyzerConfigurator : IGameAnalyzerConfigurator
    {
        private readonly Equation _topEdge;
        private readonly Equation _bottomEdge;
        private readonly Equation _leftEdge;
        private readonly Equation _rightEdge;

        private readonly Equation _topEdgeRotated;
        private readonly Equation _bottomEdgeRotated;
        private readonly Equation _leftEdgeRotated;
        private readonly Equation _rightEdgeRotated;

        private GameAnalyzerConfigurator(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
        {
            _topEdge = new Equation(topLeft, topRight);
            _bottomEdge = new Equation(bottomLeft, bottomRight);
            _leftEdge = new Equation(bottomLeft, topLeft);
            _rightEdge = new Equation(topRight, bottomRight);
        }

        public static GameAnalyzerConfigurator Create(List<Point> points, int width, int height)
        {
            var center = new Point(width / 2, height / 2);

            var topLeft = points
                .Where(p => p.X < center.X && p.Y < center.Y)
                .OrderByDescending(p => Calculations.GetDistanceBetweenPoints(p, center))
                .FirstOrDefault();
            var topRight = points
                .Where(p => p.X > center.X && p.Y < center.Y)
                .OrderByDescending(p => Calculations.GetDistanceBetweenPoints(p, center))
                .FirstOrDefault();
            var bottomLeft = points
                .Where(p => p.X < center.X && p.Y > center.Y)
                .OrderByDescending(p => Calculations.GetDistanceBetweenPoints(p, center))
                .FirstOrDefault();
            var bottomRight = points
                .Where(p => p.X > center.X && p.Y > center.Y)
                .OrderByDescending(p => Calculations.GetDistanceBetweenPoints(p, center))
                .FirstOrDefault();

            int detectedCorners = 0;
            if (topLeft != null) detectedCorners++;
            if (topRight != null) detectedCorners++;
            if (bottomLeft != null) detectedCorners++;
            if (bottomRight != null) detectedCorners++;

            if (detectedCorners < 4)
                return null;
            if (topLeft.X < bottomLeft.X)
                return null;
            if (topRight.X > bottomRight.X)
                return null;

            return new GameAnalyzerConfigurator(topLeft, topRight, bottomLeft, bottomRight);
        }

        public bool CheckPointInZoneOfInterest(Point point)
        {
            if (_topEdge.GetY(point.X) > point.Y)
                return false;
            if (_bottomEdge.GetY(point.X) < point.Y)
                return false;
            if (_leftEdge.GetY(point.X) > point.Y)
                return false;
            if (_rightEdge.GetY(point.X) > point.Y)
                return false;
            return true;
        }
    }
}
