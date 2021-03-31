using System;
using System.Collections.Generic;
using System.Drawing;

namespace GoalballAnalysisSystem.GameProcessing.Selection
{
    public interface ISelector<T> where T : class
    {
        event EventHandler<SelectionEventArgs<T>> Selected;

        void AddLocation(Point locationt, Dictionary<T, Point> objects);
    }
}