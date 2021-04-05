﻿using GoalballAnalysisSystem.GameProcessing.Geometry.Equation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Selection
{
    public class SelectionEventArgs<T> : EventArgs where T : class
    {
        public Point SelectionStart { get; set; }
        public Point SelectionEnd { get; set; }
        public IEquation SelectionEquation { get; set; }
        public T SelectionStartObject { get; set; }
        public T SelectionEndObject { get; set; }
    }
}