using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectTracking.CNN.Models
{
    public class TrackingObject<T> where T : class
    {
        public T Object { get; private set; }
        public Rectangle BoundingBox { get; private set; }

        private readonly List<double> xValues = new List<double>();
        private readonly List<double> yValues = new List<double>();
        private readonly List<Rectangle> predictedBoundaries = new List<Rectangle>();

        private readonly int _pointCountInRegression;

        public TrackingObject(T obj, Rectangle roi, int pointCountInRegression = 100)
        {
            Object = obj;
            BoundingBox = roi;
            _pointCountInRegression = pointCountInRegression;

            xValues.Add(0);
            yValues.Add(0);

            xValues.Add(roi.X + (roi.Width / 2));
            yValues.Add(roi.Y + (roi.Height / 2));
        }

        public void Update(Rectangle newROI)
        {
            BoundingBox = newROI;
            xValues.Add(newROI.X + (newROI.Width / 2));
            yValues.Add(newROI.Y + (newROI.Height / 2));

            if (xValues.Count >= _pointCountInRegression)
            {
                xValues.RemoveAt(0);
                yValues.RemoveAt(0);
            }
        }

        public double DistanceToPreviousPoint(Rectangle rect, int index)
        {
            double distance = 0;
            if (xValues.Count - index >= 0)
                distance = Math.Pow(Math.Pow(xValues[xValues.Count - index] - (rect.X + (rect.Width / 2)), 2) + Math.Pow(yValues[yValues.Count - index] - (rect.Y + (rect.Height / 2)), 2), 0.5);

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
