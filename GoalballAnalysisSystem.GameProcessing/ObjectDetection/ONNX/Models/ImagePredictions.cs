using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Settings;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Models
{
    public class ImagePredictions
    {
        [ColumnName(ModelSettings.Output)]
        public float[] Predictions { get; set; }
    }
}
