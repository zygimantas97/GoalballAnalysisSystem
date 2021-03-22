using Emgu.CV;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.CustomVision.Models;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.CustomVision.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.CustomVision
{
    public class CustomVisionObjectDetector : IObjectDetector
    {
        private readonly List<string> _targetLabels;
        private readonly float _probabilityThreshold;

        public CustomVisionObjectDetector(List<string> targetLabels, float probabilityThreshold = 0.1f)
        {
            _targetLabels = targetLabels;
            _probabilityThreshold = probabilityThreshold;
        }

        public async Task<Dictionary<string, List<Rectangle>>> Detect(Mat frame)
        {
            var detectedObjects = new Dictionary<string, List<Rectangle>>();

            var originalWidth = frame.Width;
            var originalHeight = frame.Height;

            HttpResponseMessage response = await MakePredictionRequest(frame);
            string jsonStrng = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                var predictionResponse = JsonConvert.DeserializeObject<CustomVisionPredictionResponse>(jsonStrng);

                var predictions = predictionResponse.Predictions
                    .Where(p => p.Probability > _probabilityThreshold);

                foreach (var label in _targetLabels)
                {
                    var rectangles = predictions
                        .Where(p => p.TagName == label)
                        .OrderByDescending(p => p.Probability)
                        .Select(p => {
                            double x = Math.Max(p.BoundingBox.Left, 0);
                            double y = Math.Max(p.BoundingBox.Top, 0);
                            double width = Math.Min(originalWidth - x, p.BoundingBox.Width);
                            double height = Math.Min(originalHeight - y, p.BoundingBox.Height);

                            x = originalWidth * x / ImageSettings.ImageWidth;
                            y = originalHeight * y / ImageSettings.ImageHeight;
                            width = originalWidth * width / ImageSettings.ImageWidth;
                            height = originalHeight * height / ImageSettings.ImageHeight;

                            return new Rectangle(
                                (int)Math.Round(x),
                                (int)Math.Round(y),
                                (int)Math.Round(width),
                                (int)Math.Round(height));
                        }).ToList();
                    detectedObjects[label] = rectangles;
                }
            }
            return detectedObjects;
        }

        private static async Task<HttpResponseMessage> MakePredictionRequest(Mat frame)
        {
            var bitmap = frame.ToBitmap();
            byte[] byteArray = BitmapToByteArray(bitmap, ImageFormat.Bmp);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(CustomVisionApiSettings.PredictionKeyHeader, CustomVisionApiSettings.PredictionKey);
                using (var content = new ByteArrayContent(byteArray))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return await client.PostAsync(CustomVisionApiSettings.Url, content);
                }
            }
        }

        private static byte[] BitmapToByteArray(Bitmap bitmap, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}
