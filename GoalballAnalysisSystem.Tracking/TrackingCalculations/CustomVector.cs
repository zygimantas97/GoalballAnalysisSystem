using GoalballAnalysisSystem.Tracking.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoalballAnalysisSystem.Tracking
{
    public class CustomVector
    {
        public const int minVal = 800;
        public const int maxVal = 2600;
        public const int maxDistantion = 1000;
        public const double maxTg = 1;
        public bool isCompleted { get; private set; }
        public List<CustomPoint> pointsOfVector;
        public Equation equationOfVector;
        public Direction direction;

        public CustomVector(CustomPoint point)
        {
            isCompleted = false;
            pointsOfVector = new List<CustomPoint>();
            pointsOfVector.Add(point);
        }
        public bool TryAddPoint(CustomPoint point)
        {
            if (CheckDistantion(point))
            {
                if (pointsOfVector.Count == 1)
                {
                    pointsOfVector.Add(point);
                    SetEquation();
                    TryToComplete();
                }
                else
                {
                    pointsOfVector.Add(point);
                    SetEquation();
                    TryToComplete();
                    return true;
                }

                return true;
            }

            return false;

        }
        public bool CheckDistantion(CustomPoint point)
        {
            return maxDistantion >= GetDistation(pointsOfVector.Last<CustomPoint>(), point);
        }
        public int GetDistation(CustomPoint point1, CustomPoint point2)
        {
            return (int)Math.Pow(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2), 0.5);
        }
        public void TryToComplete()
        {
            if (pointsOfVector.Last<CustomPoint>().Y < minVal || pointsOfVector.Last<CustomPoint>().Y > maxVal)
            {
                isCompleted = true;
            }
        }
        public bool CheckDirection(CustomPoint point)
        {
            if (pointsOfVector.First<CustomPoint>().Y >= pointsOfVector.Last<CustomPoint>().Y)
            {
                if (pointsOfVector.Last<CustomPoint>().Y >= point.Y)
                {
                    if (pointsOfVector.First<CustomPoint>().X <= pointsOfVector.Last<CustomPoint>().X)
                    {
                        if (pointsOfVector.Last<CustomPoint>().X <= point.X)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (pointsOfVector.Last<CustomPoint>().X >= point.X)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (pointsOfVector.Last<CustomPoint>().Y <= point.Y)
                {
                    if (pointsOfVector.First<CustomPoint>().X <= pointsOfVector.Last<CustomPoint>().X)
                    {
                        if (pointsOfVector.Last<CustomPoint>().X <= point.X)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (pointsOfVector.Last<CustomPoint>().X >= point.X)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        public void SetEquation()
        {
            if (pointsOfVector.First<CustomPoint>().X <= pointsOfVector.Last<CustomPoint>().X)
            {
                equationOfVector = new Equation(pointsOfVector.First<CustomPoint>(), pointsOfVector.Last<CustomPoint>(), true);
            }
            else
            {
                equationOfVector = new Equation(pointsOfVector.Last<CustomPoint>(), pointsOfVector.First<CustomPoint>(), true);
            }

            if (pointsOfVector.First<CustomPoint>().Y >= pointsOfVector.Last<CustomPoint>().Y)
            {
                direction = Direction.Up;
            }
            else
            {
                direction = Direction.Down;
            }

        }

        // Perdaryti į static metodą (galbūt net iškelti
        public double TgOfAngle(double k1, double k2)
        {
            return (k1 - k2) / (1 + k1 * k2);
        }

        // Konstantas iš'hardcode'int
        public CustomPoint GetBeginPoint()
        {
            if (pointsOfVector.First<CustomPoint>().Y >= pointsOfVector.Last<CustomPoint>().Y)
            {
                return new CustomPoint((int)equationOfVector.GetX(3800), 3800);
            }
            else
            {
                return new CustomPoint((int)equationOfVector.GetX(200), 200);
            }
        }
        
        // Konstantas iš'hardcode'int
        public CustomPoint GetEndPoint()
        {
            if (pointsOfVector.First<CustomPoint>().Y >= pointsOfVector.Last<CustomPoint>().Y)
            {
                return new CustomPoint((int)equationOfVector.GetX(200), 200);
            }
            else
            {
                return new CustomPoint((int)equationOfVector.GetX(3800), 3800);
            }
        }
    }
}
