using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Selector
{
    public class ProjectionSelector<T> : ISelector<T> where T : class
    {
        public event EventHandler<SelectionEventArgs<T>> Selected;

        private readonly int _minHeight;
        private readonly int _maxHeight;
        private readonly int _maxDistance;

        private Point _selectionStart;
        private T _selectionStartObject;

        private Point _previousLocation;

        public ProjectionSelector(int minHeight, int maxHeight, int maxDistance)
        {
            _minHeight = minHeight;
            _maxHeight = maxHeight;
            _maxDistance = maxDistance;
        }

        public void Update(Point locationt, Dictionary<T, Point> objects)
        {
            var eventArgs = new SelectionEventArgs<T>
            {
                SelectionStart = new Point(100, 100),
                SelectionEnd = new Point(800, 1700)
            };
            Selected?.Invoke(this, eventArgs);
        }
    }
}
