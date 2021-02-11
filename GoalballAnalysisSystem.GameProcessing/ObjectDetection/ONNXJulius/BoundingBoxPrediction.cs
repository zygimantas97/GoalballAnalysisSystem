using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXJulius
{
    public class BoundingBoxPrediction : BoundingBoxDimensions
    {
        public float Confidence { get; set; }
    }
}
