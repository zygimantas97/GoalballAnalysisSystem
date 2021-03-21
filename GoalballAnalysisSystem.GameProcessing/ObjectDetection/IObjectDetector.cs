using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection
{
    public interface IObjectDetector
    {
        Task<Dictionary<string, List<Rectangle>>> Detect(Mat frame);
    }
}
