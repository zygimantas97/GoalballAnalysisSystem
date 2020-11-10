using GoalballAnalysisSystem.Tracking.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoalballAnalysisSystem.Tracking
{
    public class GameZone
    {
        public static int[] zones = { 0, 50, 275, 325, 575, 625, 850, 900 };
        const int sectorWidth = 1800 / 7;
        int pointNr;
        // fiksuojama kiek į kurią zoną įėjo/išėjo (In/Out) kamuolys
        public int[] topIn;
        public int[] topOut;
        public int[] bottomIn;
        public int[] bottomOut;

        // užbaigtų ir tebekuriamų vektorių sąrašai
        public List<CustomVector> remainingVectors;
        public List<CustomVector> completedVectors;

        private int index = 0;
        List<double> values = new List<double>();
        public double scale;
        public List<CustomPoint> Points;
        int count = 0;
        //Equations
        private Equation top;
        public Equation topRotated;
        private Equation bottom;
        public Equation bottomRotated;
        public Equation left;
        public Equation leftRotated;
        public Equation right;
        public Equation rightRotated;

        public Equation equationOfHeightCoef;
        public Equation equationOfWidthCoef;

        public double horizontalLength;
        public double verticalLength;

        public double SinA { get; private set; }
        public double CosA { get; private set; }
        public CustomPoint BottomMiddle { get; private set; }
        public CustomPoint TopMiddle { get; private set; }

        public GameZone(List<CustomPoint> points)
        {
            pointNr = 0;
            remainingVectors = new List<CustomVector>();
            completedVectors = new List<CustomVector>();

            topIn = new int[9];
            topOut = new int[9];
            bottomIn = new int[9];
            bottomOut = new int[9];

            Points = new List<CustomPoint>();
            points = points.OrderBy(item => item.Y).ToList<CustomPoint>();

            if (points[0].X < points[1].X)
            {
                Points.Add(points[0]);
                Points.Add(points[1]);
            }
            else
            {
                Points.Add(points[1]);
                Points.Add(points[0]);
            }

            if (points[2].X < points[3].Y)
            {
                Points.Add(points[2]);
                Points.Add(points[3]);
            }
            else
            {
                Points.Add(points[3]);
                Points.Add(points[2]);
            }

            BottomMiddle = new CustomPoint((Points[2].X + Points[3].X) / 2, (Points[2].Y + Points[3].Y) / 2);
            //topMiddle = new MyPoint((Points[0].X + Points[1].X) / 2, (Points[0].Y + Points[1].Y) / 2);
            horizontalLength = GetLengthOfLine(Points[2], Points[3]);
            //verticalLength = GetLengthOfLine(bottomMiddle, topMiddle);
            SinA = (double)(Points[3].Y - Points[2].Y) / (double)horizontalLength;
            CosA = (double)(Points[3].X - Points[2].X) / horizontalLength;
            top = new Equation(Points[0], Points[1], false);
            bottom = new Equation(Points[2], Points[3], true);

            if (Points[2].X < Points[0].X)
            {
                left = new Equation(Points[2], Points[0], false);
            }
            else
            {
                left = new Equation(Points[0], Points[2], true);
            }

            if (Points[1].X < Points[3].X)
            {
                right = new Equation(Points[1], Points[3], false);
            }
            else
            {
                right = new Equation(Points[3], Points[1], true);
            }

            for (int i = 0; i < 4; i++)
            {
                Points[i] = RotatePoint(Points[i]);
            }

            int MinTopY;
            if (Points[0].Y <= Points[1].Y)
            {
                MinTopY = Points[0].Y;
            }
            else
            {
                MinTopY = Points[1].Y;
            }

            int topLength = (int)GetLengthOfLine(Points[0], Points[1]);
            Points[0].Y = MinTopY;
            Points[1].Y = MinTopY;
            Points[0].X = BottomMiddle.X - (topLength / 2);
            Points[1].X = BottomMiddle.X + (topLength / 2);
            verticalLength = BottomMiddle.Y - MinTopY;
            horizontalLength = Points[3].X - Points[2].X;

            topRotated = new Equation(Points[0], Points[1], false);
            bottomRotated = new Equation(Points[2], Points[3], true);
            if (Points[2].X < Points[0].X)
            {
                leftRotated = new Equation(Points[2], Points[0], false);
            }
            else
            {
                leftRotated = new Equation(Points[0], Points[2], true);
            }

            if (Points[1].X < Points[3].X)
            {

                rightRotated = new Equation(Points[1], Points[3], false);

            }
            else
            {
                rightRotated = new Equation(Points[3], Points[1], true);
            }

            equationOfHeightCoef = new Equation(0, 1, verticalLength, horizontalLength / verticalLength * 2);
            equationOfWidthCoef = new Equation(0, 1);
            scale = 1800 / horizontalLength;
        }

        public double GetLengthOfLine(CustomPoint point1, CustomPoint point2)
        {
            return Math.Pow(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2), 0.5);
        }

        public List<CustomPoint> GetPoints()
        {
            return Points;
        }

        public CustomPoint videoOXY2gameZoneOXY(CustomPoint point, bool rotation)
        {
            CustomPoint tempPoint;
            if (rotation)
            {
                tempPoint = RotatePoint(point);
            }
            else
            {
                tempPoint = point;
            }
            values.Add(tempPoint.Y);

            tempPoint.X = tempPoint.X - BottomMiddle.X;
            tempPoint.Y = tempPoint.Y - BottomMiddle.Y;

            int x = tempPoint.X;
            int y = tempPoint.Y;
            //values.Add(y);
            /*
            if(values.Count == 2)
            {
                throw new Exception(values[0].ToString() + " " + values[1].ToString());
            }
            */
            //throw new Exception(y.ToString());
            double heightCoef = equationOfHeightCoef.GetY(Math.Abs(y));
            tempPoint.Y = (int)(tempPoint.Y * heightCoef);
            //throw new Exception(heightCoef.ToString());
            //throw new Exception(x + ";" + y);
            double MaxWidth = Math.Abs(rightRotated.GetX(y + BottomMiddle.Y));
            MaxWidth = MaxWidth - BottomMiddle.X;
            //throw new Exception(MaxWidth.ToString());
            equationOfWidthCoef.SetAnotherPoint(MaxWidth, ((double)horizontalLength / 2) / MaxWidth);
            //throw new Exception((((double)horizontalLength / 2) / MaxWidth).ToString());
            //throw new Exception(EquationOfWidthCoef.GetY(MaxWidth).ToString());
            double widthCoef = equationOfWidthCoef.GetY(Math.Abs(x));
            //throw new Exception(widthCoef.ToString());
            tempPoint.X = (int)(tempPoint.X * widthCoef);
            //throw new Exception(widthCoef.ToString());
            //throw new Exception(tempPoint.X + ";" + tempPoint.Y);
            tempPoint.X = (int)(tempPoint.X * scale);
            tempPoint.Y = (int)(tempPoint.Y * scale);
            //tempPoint.X = tempPoint.X + bottomMiddle.X;
            //tempPoint.Y = tempPoint.Y + bottomMiddle.Y;
            tempPoint.X = tempPoint.X + 1000;
            tempPoint.Y = tempPoint.Y + 3800;
            index++;
            return tempPoint;
        }
        public CustomPoint RotatePoint(CustomPoint tempPoint)
        {
            tempPoint.X = tempPoint.X - BottomMiddle.X;
            tempPoint.Y = tempPoint.Y - BottomMiddle.Y;

            int x = tempPoint.X;
            int y = tempPoint.Y;
            tempPoint.X = (int)(x * CosA + y * SinA);
            tempPoint.Y = (int)(-x * SinA + y * CosA);

            tempPoint.X = tempPoint.X + BottomMiddle.X;
            tempPoint.Y = tempPoint.Y + BottomMiddle.Y;
            count++;

            return tempPoint;
        }
        public bool IsPointSuitable(CustomPoint point)
        {
            if (!top.IsPointSuitable(point))
                return false;

            if (!bottom.IsPointSuitable(point))
                return false;

            if (!left.IsPointSuitable(point))
                return false;

            if (!right.IsPointSuitable(point))
                return false;

            return true;
        }
        public void AddPointToVectors(CustomPoint point)
        {
            pointNr++;
            bool vectorFound = false;

            foreach (CustomVector vector in remainingVectors)
            {
                if (vector.TryAddPoint(point))
                    vectorFound = true;
            }

            if (!vectorFound)
            {
                //throw new Exception("x=" + point.X.ToString() + " y=" + point.Y.ToString());
                if ((point.Y >= CustomVector.minVal) && (point.Y <= CustomVector.maxVal))
                {
                    //throw new Exception("x=" + point.X.ToString() + " y=" + point.Y.ToString());
                    CustomVector tempVector = new CustomVector(point);
                    //throw new Exception("x=" + tempVector.pointsOfVector[0].X.ToString() + " y=" + tempVector.pointsOfVector[0].Y.ToString());
                    remainingVectors.Add(tempVector);
                    //throw new Exception("remaining: " + remainingVectors.Count.ToString());
                }
            }
            else
            {
                List<CustomVector> completed = remainingVectors.Where(item => item.isCompleted == true).ToList<CustomVector>();
                if (completed.Count > 0)
                {
                    List<CustomVector> filteredCompleted = completed.Where(item => (item.GetBeginPoint().X >= 100) && (item.GetBeginPoint().X <= 1900)).ToList<CustomVector>();
                    if (filteredCompleted.Count > 0)
                    {
                        completedVectors.AddRange(filteredCompleted);
                        foreach (CustomVector vector in filteredCompleted)
                        {
                            SetInOutValues(vector);
                        }
                    }
                }
                remainingVectors.RemoveAll(item => item.isCompleted == true);
            }

            if (pointNr == 0)
            {
                throw new Exception("remaining: " + remainingVectors.Count.ToString());
            }
        }

        private void SetInOutValues(CustomVector vector)
        {
            int firstX = vector.GetBeginPoint().X - 100;
            int lastX = vector.GetEndPoint().X - 100;
            int outSector = GetZone(firstX);
            int inSector = GetZone(lastX);

            if (vector.direction == Direction.Up)
            {
                bottomOut[outSector]++;
                topIn[inSector]++;
            }
            else
            {
                topOut[outSector]++;
                bottomIn[inSector]++;
            }
        }

        public int GetZone(int X)
        {
            int index = 0;
            if (X > 0)
                index++;

            for (int i = 1; i < GameZone.zones.Length; i++)
            {
                X = X - (GameZone.zones[i] - GameZone.zones[i - 1]) * 2;
                if (X > 0)
                    index++;
            }

            return index;
        }
    }
}
