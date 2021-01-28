using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Drawing
{
    public static class EmguCVFiguresDrawing
    {
        public static Mat DrawPolygon(Mat frame, Point[] corners)
        {
            Mat modifiedFrame = frame;
            if (corners.Length >= 2)
            {
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    modifiedFrame = DrawLine(modifiedFrame, corners[i], corners[i + 1]);
                }
                modifiedFrame = DrawLine(modifiedFrame, corners[corners.Length - 1], corners[0]);
            }
            
            return modifiedFrame;
        }

        public static Mat DrawRectangle(Mat frame, Point topCorner, int length, int width)
        {
            var rect = new Rectangle(topCorner.X, topCorner.Y, length, width);
            CvInvoke.Rectangle(frame, rect, new MCvScalar(0, 0, 255), 5);

            return frame;
        }

        public static Mat DrawLine(Mat frame, Point start, Point end)
        {
            CvInvoke.Line(frame, start, end, new MCvScalar(0, 0, 255), 3);

            return frame;
        }

    }
}
