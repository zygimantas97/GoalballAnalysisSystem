using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.WinAIBasedObjectDetection.Models
{
    public class ExtractedBoxes
    {
        public List<BoundingBox> Boxes { get; }
        public List<float[]> Probabilities { get; }
        public ExtractedBoxes(List<BoundingBox> boxes, List<float[]> probs)
        {
            this.Boxes = boxes;
            this.Probabilities = probs;
        }
    }
}
