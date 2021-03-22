using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Settings
{
    public static class PredictionSettings
    {
        public const int RowCount = 13;
        public const int ColumnCount = 13;
        public const int FeaturesPerBox = 5;
        public static readonly (float x, float y)[] Anchors = { (0.573f, 0.677f), (1.87f, 2.06f), (3.34f, 5.47f), (7.88f, 3.53f), (9.77f, 9.17f) };
    }
}
