using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.MLBasedObjectDetection.Models
{

    public class ImageSettings
    {
        public const int ImageHeight = 416;
        public const int ImageWidth = 416;
        public const int Mean = 117;
        public const bool ChannelsLast = true;
    }
}
