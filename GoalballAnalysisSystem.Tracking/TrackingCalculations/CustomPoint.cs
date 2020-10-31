using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.Tracking
{
    class CustomPoint : IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CustomPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int CompareTo(object obj)
        {
            CustomPoint point = (CustomPoint)obj;
            if (X.CompareTo(point.X) != 0)
                return X.CompareTo(point.X);

            return Y.CompareTo(point.Y);
        }

        public Point ConvertoToDrawingPoint()
        {
            return new Point(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            CustomPoint point = obj as CustomPoint;
            if (point == null)
                return false;

            if (Math.Abs(this.X - point.X) < 5 && Math.Abs(this.Y - point.Y) < 5)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
