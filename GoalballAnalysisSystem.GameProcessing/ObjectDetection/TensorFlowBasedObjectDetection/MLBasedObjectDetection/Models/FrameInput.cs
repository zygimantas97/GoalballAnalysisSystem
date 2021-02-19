using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.MLBasedObjectDetection.Models
{
    public class FrameInput
    {
        [ImageType(ImageSettings.ImageHeight, ImageSettings.ImageWidth)]
        public Bitmap Image { get; set; }
    }
}
