using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Tracking
{
    class Equation
    {
        private CustomPoint BasePoint;
        public double a { get; private set; }
        public double b { get; private set; }

        public bool PointsAreBelow;

        public Equation(CustomPoint point1, CustomPoint point2, bool pointsAreBelow)
        {
            PointsAreBelow = pointsAreBelow;
            a = (double)(point2.Y - point1.Y) / (double)(point2.X - point1.X);
            b = point1.Y - a * point1.X;
        }
        public Equation(double point1X, double point1Y, double point2X, double point2Y)
        {
            a = (double)(point2Y - point1Y) / (double)(point2X - point1X);
            b = point1Y - a * point1X;
        }
        public Equation(double point1X, double point1Y)
        {
            BasePoint = new CustomPoint((int)point1X, (int)point1Y);
        }
        public void SetAnotherPoint(double point2X, double point2Y)
        {
            double point1X = BasePoint.X;
            double point1Y = BasePoint.Y;
            a = (double)(point2Y - point1Y) / (double)(point2X - point1X);
            b = point1Y - a * point1X;
        }
        public double GetX(double y)
        {
            double x;
            x = (y - b) / a;
            return x;
        }
        public double GetY(double x)
        {
            double y;
            y = a * x + b;
            return y;
        }
        
        public bool IsPointSuitable(CustomPoint point)
        {
            double tempValue = a * point.X + b;

            if (PointsAreBelow)
            {
                if (tempValue > point.Y)
                    return true;
                return false;
            }
            else
            {
                if (tempValue < point.Y)
                    return true;
                return false;
            }
        }
    }
}
