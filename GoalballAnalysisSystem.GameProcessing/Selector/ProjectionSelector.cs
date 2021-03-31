using GoalballAnalysisSystem.GameProcessing.Models;
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

        public void AddLocation(Point location, Dictionary<T, Point> objects)
        {
            if(!_isSelecting)
            {
                if(location.Y < _top || location.Y > _bottom)
                {
                    StartSelection(location, objects);
                }
            }
            else if ((_selectionStart.Y < _top && location.Y > _bottom) || (_selectionStart.Y > _bottom && location.Y < _top))
            {
                EndSelection(location, objects);
            }
            else
            {
                // TODO: update always when location.Y < _top || location.Y > _bottom
                if((_selectionStart.Y < _top && location.Y < _top && location.Y > _selectionStart.Y) ||
                   (_selectionStart.Y > _bottom && location.Y > _bottom && location.Y < _selectionStart.Y))
                {
                    StartSelection(location, objects);
                }
                else
                {
                    _selectionPoints.Add(location);
                }
            }
        }

        private void StartSelection(Point location, Dictionary<T, Point> objects)
        {
            _selectionStart = location;
            _selectionPoints.Clear();
            _selectionPoints.Add(location);
            _selectionStartObject = objects
                .Where(kvp => _maxDistance >= Geometry.GetDistanceBetweenPoints(kvp.Value, location))
                .OrderBy(kvp => Geometry.GetDistanceBetweenPoints(kvp.Value, location))
                .FirstOrDefault().Key;
            _isSelecting = true;
        }

        private void EndSelection(Point location, Dictionary<T, Point> objects)
        {
            var selectionEndObject = objects
                .Where(kvp => _maxDistance >= Geometry.GetDistanceBetweenPoints(kvp.Value, location))
                .OrderBy(kvp => Geometry.GetDistanceBetweenPoints(kvp.Value, location))
                .FirstOrDefault().Key;
            
            OnSelected(_selectionPoints, _selectionStartObject, selectionEndObject);

            _selectionPoints.Clear();
            _selectionStartObject = null;
            _isSelecting = false;
        }

        private void OnSelected(List<Point> selectionPoints, T selectionStartObject, T selectionEndObject)
        {
            var eventArgs = new SelectionEventArgs<T>()
            {
                SelectionStart = selectionPoints.First(),
                SelectionEnd = selectionPoints.Last(),
                SelectionEquation = new LinearEquation(selectionPoints.First(), selectionPoints.Last()),
                SelectionStartObject = selectionStartObject,
                SelectionEndObject = selectionEndObject
            };
            Selected?.Invoke(this, eventArgs);
        }
    }
}
