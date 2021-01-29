using GoalballAnalysisSystem.GameProcessing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.PlayFieldTracker
{
    public static class FilterParameters
    {
        //public static readonly Pair<int, int> Hue = new Pair<int, int>(8, 49);
        //public static readonly Pair<int, int> Saturation = new Pair<int, int>(13, 74);
        //public static readonly Pair<int, int> Value = new Pair<int, int>(110, 170);

        public static readonly Pair<int, int> Hue = new Pair<int, int>(0, 10);
        public static readonly Pair<int, int> Saturation = new Pair<int, int>(10, 160);
        public static readonly Pair<int, int> Value = new Pair<int, int>(80, 200);



        public static readonly int StructElementSizeErode = 4;
        public static readonly int StructElementSizeDilate = 8;
        public static readonly int ErodeIterations = 2;
        public static readonly int DilateIterations = 15;

        public static readonly Pair<int, int> BallSize = new Pair<int, int>(200, 1900);
    }
}
