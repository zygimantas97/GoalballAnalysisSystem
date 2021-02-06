using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.MLBasedObjectDetectionStrategy.Models
{
    public class FramePredictions
    {
        [ColumnName("model_outputs0")]
        public float[] ObjectType { get; set; }
    }
}
