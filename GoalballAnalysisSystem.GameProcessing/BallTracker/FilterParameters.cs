using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.BallTracker
{
    public static class FilterParameters
    {
        public static readonly Pair<int, int> Hue = new Pair<int, int>(94, 118);
        public static readonly Pair<int, int> Saturation = new Pair<int, int>(100, 250);
        public static readonly Pair<int, int> Value = new Pair<int, int>(98, 255);
        public static readonly int StructElementSizeErode = 3;
        public static readonly int StructElementSizeDilate = 8;
        public static readonly int ErodeIterations = 3;
        public static readonly int DilateIterations = 3;

        public static readonly Pair<int, int> BallSize = new Pair<int, int>(200, 1900);
    }
}
