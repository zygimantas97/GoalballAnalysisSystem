using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing
{
    public static class Calculations
    {
        public static double GetDistanceBetweenPoints(Point point1, Point point2)
        {
            double diffX = point1.X - point2.X;
            double diffY = point1.Y - point2.Y;
            double distance = Math.Pow((Math.Pow(diffX, 2) + Math.Pow(diffY, 2)), 0.5);
            return distance;
        }
    }
}
