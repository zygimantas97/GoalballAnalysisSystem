using System;
using System.Collections.Generic;
using System.Drawing;

namespace GoalballAnalysisSystem.GameProcessing.Selector
{
    public interface ISelector<T> where T : class
    {
        event EventHandler<SelectionEventArgs<T>> Selected;

        void AddPoint(Point locationt, Dictionary<T, Point> objects);
    }
}