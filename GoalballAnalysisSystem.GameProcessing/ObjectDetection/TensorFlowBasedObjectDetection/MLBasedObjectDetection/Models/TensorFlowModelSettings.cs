using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.MLBasedObjectDetection.Models
{
    public class TensorFlowModelSettings
    {
        public const string InputTensorName = "Placeholder";
        public const string OutputTensorName = "m_outputs0/BiasAdd";
        public const string TensorFlowModelLocation = "./ObjectDetection/TensorFlowBasedObjectDetection/TensorFlowModel/model.pb";
        public const string TensorFlowLabelsLocation = "./ObjectDetection/TensorFlowBasedObjectDetection/TensorFlowModel/labels.txt";
    }
}
