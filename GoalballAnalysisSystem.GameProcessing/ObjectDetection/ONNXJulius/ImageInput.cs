using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML;
using Microsoft.ML.Transforms.Image;
using System.Drawing;
using System.IO;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXJulius
{
    class ImageInput
    {
        [ImageType(ImageSettings.imageHeight, ImageSettings.imageWidth)]
        public Bitmap Image { get; set; }
    }
}
