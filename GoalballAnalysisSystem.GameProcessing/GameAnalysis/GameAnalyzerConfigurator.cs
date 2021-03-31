using GoalballAnalysisSystem.GameProcessing.Geometry;
using GoalballAnalysisSystem.GameProcessing.Geometry.Equation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.GameAnalysis
{
    public class GameAnalyzerConfigurator : IGameAnalyzerConfigurator
    {
        private readonly Point _basePoint;
        private readonly IEquation _baseLine;
        private readonly double _rotationSin;
        private readonly double _rotationCos;
        private readonly double _playgroundWidth;
        private readonly double _playgroundHeight;

        private readonly IEquation _topEdge;
        private readonly IEquation _bottomEdge;
        private readonly IEquation _leftEdge;
        private readonly IEquation _rightEdge;

        private readonly IEquation _topEdgeRotated;
        private readonly IEquation _bottomEdgeRotated;
        private readonly IEquation _leftEdgeRotated;
        private readonly IEquation _rightEdgeRotated;

        public Point TopLeft { get; private set; }
        public Point TopRight { get; private set; }
        public Point BottomLeft { get; private set; }
        public Point BottomRight { get; private set; }

        private GameAnalyzerConfigurator(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight, double playgroundWidth, double playgroundHeight)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

            _playgroundWidth = playgroundWidth;
            _playgroundHeight = playgroundHeight;

            _topEdge = new LinearEquation(topLeft, topRight);
            _bottomEdge = new LinearEquation(bottomLeft, bottomRight);
            _leftEdge = new LinearEquation(bottomLeft, topLeft);
            _rightEdge = new LinearEquation(topRight, bottomRight);

            _basePoint = Calculations.GetMiddlePoint(bottomLeft, bottomRight);
            _rotationSin = Calculations.GetRotationSin(_basePoint, bottomRight);
            _rotationCos = Calculations.GetRotationCos(_basePoint, bottomRight);

            var topLeftRotated = Calculations.RotatePoint(_basePoint, topLeft, -_rotationSin, _rotationCos);
            var topRightRotated = Calculations.RotatePoint(_basePoint, topRight, -_rotationSin, _rotationCos);
            var bottomLeftRotated = Calculations.RotatePoint(_basePoint, bottomLeft, -_rotationSin, _rotationCos);
            var bottomRightRotated = Calculations.RotatePoint(_basePoint, bottomRight, -_rotationSin, _rotationCos);

            _topEdgeRotated = new LinearEquation(topLeftRotated, topRightRotated);
            _bottomEdgeRotated = new LinearEquation(bottomLeftRotated, bottomRightRotated);
            _leftEdgeRotated = new LinearEquation(bottomLeftRotated, topLeftRotated);
            _rightEdgeRotated = new LinearEquation(topRightRotated, bottomRightRotated);

            var topEdgeMiddlePoint = Calculations.GetMiddlePoint(topLeft, topRight);
            var topEdgeMiddlePointRotated = Calculations.RotatePoint(_basePoint, topEdgeMiddlePoint, -_rotationSin, _rotationCos);
            _baseLine = new LinearEquation(_basePoint, topEdgeMiddlePointRotated);
        }

        public static GameAnalyzerConfigurator Create(List<Point> points, int frameWidth, int frameHeight, double playgroundWidth, double playgroundHeight)
        {
            var center = new Point(frameWidth / 2, frameHeight / 2);

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

            if (topLeft == null || topRight == null || bottomLeft == null || bottomRight == null)
                return null;

            if (topLeft.X < bottomLeft.X || topRight.X > bottomRight.X)
                return null;

            return new GameAnalyzerConfigurator(topLeft, topRight, bottomLeft, bottomRight, playgroundWidth, playgroundHeight);
        }

        public bool IsPointInZoneOfInterest(Point point)
        {
            if (point == null)
                return false;
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

        public Point GetPlaygroundOXY(Point point)
        {
            var rotatedPoint = Calculations.RotatePoint(_basePoint, point, -_rotationSin, _rotationCos);

            double width;
            double baseLineX = _baseLine.GetX(rotatedPoint.Y);
            if (baseLineX < rotatedPoint.X)
            {
                width = _rightEdgeRotated.GetX(rotatedPoint.Y) - baseLineX;
            }
            else
            {
                width = baseLineX - _leftEdgeRotated.GetX(rotatedPoint.Y);
            }
            double height = _basePoint.Y - _topEdgeRotated.GetY(rotatedPoint.X);

            double x = rotatedPoint.X - baseLineX;
            double y = rotatedPoint.Y - _basePoint.Y;

            var widthScaleEquation = new LinearEquation(0, 1, width, _playgroundWidth / width / 2);
            var heightScaleEquation = new LinearEquation(0, 1, height, _playgroundHeight / height);

            double widthScale = widthScaleEquation.GetY(Math.Abs(x));
            double heightScale = heightScaleEquation.GetY(Math.Abs(y));

            double scaledX = x * widthScale;
            double scaledY = y * heightScale;

            return new Point((int)Math.Round(scaledX + _playgroundWidth / 2), (int)Math.Round(scaledY + _playgroundHeight));
        }
    }
}
