using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Models
{
    public class ONNXTrackingObject
    {
        public long ObjectId { get; set; }
        public Rectangle ROI { get; set; }

        List<double> xValues = new List<double>();
        List<double> yValues = new List<double>();
        List<Rectangle> predictedBoundaries = new List<Rectangle>();

        int HowManyPointsInRegression = 100;

        public ONNXTrackingObject(long objectID, Rectangle roi)
        {
            ObjectId = objectID;
            ROI = roi;

            xValues.Add(0);
            yValues.Add(0);

            xValues.Add(roi.X + (roi.Width / 2));
            yValues.Add(roi.Y + (roi.Height / 2));

        }

        public void Update(Rectangle newROI)
        {
            ROI = newROI;
            xValues.Add(newROI.X + (newROI.Width / 2));
            yValues.Add(newROI.Y + (newROI.Height / 2));

            if (xValues.Count >= HowManyPointsInRegression)
            {
                xValues.RemoveAt(0);
                yValues.RemoveAt(0);
            }

        }

        public void LinearRegression(List<double> xVals, List<double> yVals, out double rSquared, out double yIntercept, out double slope)
        {
            if (xVals.Count != yVals.Count)
            {
                throw new Exception("Input values should be with the same length.");
            }

            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            int counter = 0;
            foreach (var item in xVals)
            {
                var x = xVals[counter];
                var y = yValues[counter];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
                counter++;
            }

            var count = xVals.Count;
            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }

        public double Distance(Rectangle rect)
        {
            double x = rect.X + (rect.Width / 2);
            double y = rect.Y + (rect.Height / 2);
            double rSquared, yIntercept, slope;

            LinearRegression(xValues, yValues, out rSquared, out yIntercept, out slope);

            var Distance = Math.Abs(slope * x - (1 * y) + yIntercept) / Math.Pow(slope * slope + 1, 0.5);
            return Distance;
        }

        public double DistanceToPreviousPoint(Rectangle rect, int index)
        {
            double distance = 0;
            if(xValues.Count - index >= 0)
                distance = Math.Pow(Math.Pow(xValues[xValues.Count - index] - (rect.X+(rect.Width/2)), 2) + Math.Pow(yValues[yValues.Count - index] - (rect.Y + (rect.Height / 2)), 2), 0.5);

            return distance;
        }

        public void AddPredictionBoundary(Rectangle boundary)
        {
            predictedBoundaries.Add(boundary);
        }

        public void ClearPredictionBoundaries()
        {
            predictedBoundaries.Clear();
        }

        public void DetermineAndUpdateMostFittingPrediction()
        {
            if (predictedBoundaries.Count > 0)
            {
                var distance = Math.Pow(Math.Pow(xValues[xValues.Count - 1] - (predictedBoundaries[0].X + (predictedBoundaries[0].Width / 2)), 2) + Math.Pow(yValues[yValues.Count - 1] - (predictedBoundaries[0].Y + (predictedBoundaries[0].Height / 2)), 2), 0.5);
                var currentBest = predictedBoundaries[0];

                foreach (var boundary in predictedBoundaries) //select closest to the previous ROI
                {
                    var dist = Math.Pow(Math.Pow(xValues[xValues.Count - 1] - (boundary.X + (boundary.Width / 2)), 2) + Math.Pow(yValues[yValues.Count - 1] - (boundary.Y + (boundary.Height / 2)), 2), 0.5);
                    if (dist < distance)
                    {
                        distance = dist;
                        currentBest = boundary;
                    }
                }
                Update(currentBest);
            }
        }
    }
}
