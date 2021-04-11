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

        private readonly List<double> _xValues = new List<double>();
        private readonly List<double> _yValues = new List<double>();
        private readonly List<Rectangle> _predictedBoundaries = new List<Rectangle>();

        private readonly int _pointCountInRegression;

        public TrackingObject(T obj, Rectangle roi, int pointCountInRegression = 100)
        {
            Object = obj;
            BoundingBox = roi;
            _pointCountInRegression = pointCountInRegression;

            _xValues.Add(0);
            _yValues.Add(0);

            _xValues.Add(roi.X + (roi.Width / 2));
            _yValues.Add(roi.Y + (roi.Height / 2));
        }

        public double DistanceToPreviousPoint(Rectangle rect, int index)
        {
            double distance = 0;
            if (_xValues.Count - index >= 0)
                distance = Math.Pow(Math.Pow(_xValues[_xValues.Count - index] - (rect.X + (rect.Width / 2)), 2) + Math.Pow(_yValues[_yValues.Count - index] - (rect.Y + (rect.Height / 2)), 2), 0.5);

            return distance;
        }

        public void AddPredictionBoundary(Rectangle boundary)
        {
            _predictedBoundaries.Add(boundary);
        }

        public void ClearPredictionBoundaries()
        {
            _predictedBoundaries.Clear();
        }

        public void DetermineAndUpdateMostFittingPrediction()
        {
            if (_predictedBoundaries.Count > 0)
            {
                var distance = Math.Pow(Math.Pow(_xValues[_xValues.Count - 1] - (_predictedBoundaries[0].X + (_predictedBoundaries[0].Width / 2)), 2) + Math.Pow(_yValues[_yValues.Count - 1] - (_predictedBoundaries[0].Y + (_predictedBoundaries[0].Height / 2)), 2), 0.5);
                var currentBest = _predictedBoundaries[0];

                foreach (var boundary in _predictedBoundaries) //select closest to the previous ROI
                {
                    var dist = Math.Pow(Math.Pow(_xValues[_xValues.Count - 1] - (boundary.X + (boundary.Width / 2)), 2) + Math.Pow(_yValues[_yValues.Count - 1] - (boundary.Y + (boundary.Height / 2)), 2), 0.5);
                    if (dist < distance)
                    {
                        distance = dist;
                        currentBest = boundary;
                    }
                }
                Update(currentBest);
            }
        }

        private void Update(Rectangle newROI)
        {
            BoundingBox = newROI;
            _xValues.Add(newROI.X + (newROI.Width / 2));
            _yValues.Add(newROI.Y + (newROI.Height / 2));

            if (_xValues.Count >= _pointCountInRegression)
            {
                _xValues.RemoveAt(0);
                _yValues.RemoveAt(0);
            }
        }
    }
}
