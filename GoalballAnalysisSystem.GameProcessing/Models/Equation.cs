using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Models
{
    public class Equation
    {
        private readonly double _a;
        private readonly double _b;

        public Equation(Point point1, Point point2)
        {
            _a = (double)(point2.Y - point1.Y) / (double)(point2.X - point1.X);
            _b = point1.Y - _a * point1.X;
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
