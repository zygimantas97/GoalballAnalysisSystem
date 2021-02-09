using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.WinAIBasedObjectDetection.Models
{
    public sealed class PredictionModel
    {
        public PredictionModel(float probability, string tagName, BoundingBox boundingBox)
        {
            this.Probability = probability;
            this.TagName = tagName;
            this.BoundingBox = boundingBox;
        }

        public float Probability { get; private set; }
        public string TagName { get; private set; }
        public BoundingBox BoundingBox { get; private set; }
    }
}
