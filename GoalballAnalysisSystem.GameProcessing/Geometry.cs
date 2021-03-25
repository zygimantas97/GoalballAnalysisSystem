using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing
{
    public static class Geometry
    {
        public static double GetDistanceBetweenPoints(Point point1, Point point2)
        {
            return Math.Pow((Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2)), 0.5);
        }

        public static Point GetMiddlePoint(Point point1, Point point2)
        {
            return new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        }

        public static double GetRotationSin(Point basePoint, Point targetPoint)
        {
            return (targetPoint.Y - basePoint.Y) / GetDistanceBetweenPoints(basePoint, targetPoint);
        }

        public static double GetRotationCos(Point basePoint, Point targetPoint)
        {
            return (targetPoint.X - basePoint.X) / GetDistanceBetweenPoints(basePoint, targetPoint);
        }

        public static Point RotatePoint(Point basePoint, Point targetPoint, double rotationSin, double rotationCos)
        {
            int x = targetPoint.X - basePoint.X;
            int y = targetPoint.Y - basePoint.Y;
            int rotatedX = (int)(x * rotationCos - y * rotationSin);
            int rotatedY = (int)(x * rotationSin + y * rotationCos);

            return new Point(rotatedX + basePoint.X, rotatedY + basePoint.Y);
        }
    }
}
