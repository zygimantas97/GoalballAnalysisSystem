using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private readonly List<Point> _selectionPoints = new List<Point>();
        private bool _isSelecting = false;

        public ProjectionSelector(int minHeight, int maxHeight, int maxDistance)
        {
            _minHeight = minHeight;
            _maxHeight = maxHeight;
            _maxDistance = maxDistance;
        }

        public void Update(Point location, Dictionary<T, Point> objects)
        {
            if(!_isSelecting)
            {
                if(location.Y < _minHeight || location.Y > _maxHeight)
                {
                    StartSelection(location, null);
                }
            }
            else if ((_selectionStart.Y < _minHeight && location.Y > _maxHeight) || (_selectionStart.Y > _maxHeight && location.Y < _minHeight))
            {
                EndSelection(location, null);
            }
            else
            {
                // TODO: In some case need to restart selection
                _selectionPoints.Add(location);
            }
        }

        private void StartSelection(Point location, T obj)
        {
            _selectionStart = location;
            _selectionPoints.Clear();
            _selectionPoints.Add(location);
            _isSelecting = true;
        }

        private void EndSelection(Point location, T obj)
        {
            var eventArgs = new SelectionEventArgs<T>()
            {
                SelectionStart = _selectionPoints.First(),
                SelectionEnd = location
            };
            Selected?.Invoke(this, eventArgs);

            _selectionPoints.Clear();
            _isSelecting = false;
        }
    }
}
