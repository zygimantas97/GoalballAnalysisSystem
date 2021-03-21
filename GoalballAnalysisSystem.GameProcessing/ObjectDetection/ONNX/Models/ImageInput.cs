using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Settings;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Models
{
    public class ImageInput
    {
        [ImageType(ImageSettings.ImageHeight, ImageSettings.ImageWidth)]
        public Bitmap Image { get; set; }
    }
}
