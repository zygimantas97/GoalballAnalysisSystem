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

        private readonly int _top;
        private readonly int _bottom;
        private readonly int _maxDistance;

        private Point _selectionStart;
        private T _selectionStartObject;
        private readonly List<Point> _selectionPoints = new List<Point>();
        private bool _isSelecting = false;

        public ProjectionSelector(int top, int bottom, int maxDistance)
        {
            _top = top;
            _bottom = bottom;
            _maxDistance = maxDistance;
        }

        public void AddPoint(Point location, Dictionary<T, Point> objects)
        {
            if(!_isSelecting)
            {
                if(location.Y < _top || location.Y > _bottom)
                {
                    StartSelection(location, null);
                }
            }
            else if ((_selectionStart.Y < _top && location.Y > _bottom) || (_selectionStart.Y > _bottom && location.Y < _top))
            {
                EndSelection(location, null);
            }
            else
            {
                // TODO: update always when location.Y < _top || location.Y > _bottom
                if((_selectionStart.Y < _top && location.Y < _top && location.Y > _selectionStart.Y) ||
                   (_selectionStart.Y > _bottom && location.Y > _bottom && location.Y < _selectionStart.Y))
                {
                    StartSelection(location, null);
                }
                else
                {
                    _selectionPoints.Add(location);
                }
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
            // TODO: make equation representation of projection
            // TODO: separate end of selection and emitting of event
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
