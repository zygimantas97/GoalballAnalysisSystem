using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.BallTracker
{
    public class Pair <T, U>
    {
        public T Min { get; set; }
        public U Max { get; set; }

        public Pair(T first, U second)
        {
            this.Min = first;
            this.Max = second;
        }

    }
}
