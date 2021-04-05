using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Geometry.Equation
{
    public class LinearEquation : IEquation
    {
        private readonly double _a;
        private readonly double _b;

        public LinearEquation(Point point1, Point point2)
            : this(point1.X, point1.Y, point2.X, point2.Y)
        {

        }

        public LinearEquation(double x1, double y1, double x2, double y2)
        {
            double diffX = x2 - x1;
            if (diffX == 0)
                diffX = 0.000001;
            double diffY = y2 - y1;
            if (diffY == 0)
                diffY = 0.000001;
            _a = diffY / diffX;
            _b = y1 - _a * x1;
        }

        public double GetX(double y)
        {
            return (y - _b) / _a;
        }

        public double GetY(double x)
        {
            return _a * x + _b;
        }
    }
}
