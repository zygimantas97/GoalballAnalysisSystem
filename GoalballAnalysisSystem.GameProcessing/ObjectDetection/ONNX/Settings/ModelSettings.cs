using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Settings
{
    public static class ModelSettings
    {
        public const string Input = "data";
        public const string Output = "model_outputs0";
        public const string ModelPath = "./ObjectDetection/ONNX/ONNXModel/model.onnx";
        public const string LabelsPath = "./ObjectDetection/ONNX/ONNXModel/labels.txt";
    }
}
