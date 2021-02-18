using Emgu.CV;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.BallTracker;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.MLBasedObjectDetection.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.APIBasedObjectDetectionStrategy
{
    public class APIBasedObjectDetectionStrategy // : IObjectDetectionStrategy
    {
        private readonly List<string> _targetLabels;
        private readonly float _probabilityThreshold;
        public APIBasedObjectDetectionStrategy(List<string> targetLabels, float probabilityThreshold = 0.1f)
        {
            _targetLabels = targetLabels;
            _probabilityThreshold = probabilityThreshold;
        }

        public async Task<Rectangle> DetectObject(Mat frame)
        {
            var boundingBoxes = new List<Prediction>();
            HttpResponseMessage response =  await MakePredictionRequest(frame);

            var originalWidth = frame.Width;
            var originalHeight = frame.Height;

            string jsonStrng = await response.Content.ReadAsStringAsync();

            Rectangle result = new Rectangle(0, 0, 0, 0);
            try
            {
                ApiPredictionModel parsedPredictionModel = JsonConvert.DeserializeObject<ApiPredictionModel>(jsonStrng); // Json convert to model class

                for (int i = 0; i < parsedPredictionModel.Predictions.Length; i++)
                {
                    boundingBoxes.Add(parsedPredictionModel.Predictions[i]);

                }
                boundingBoxes = boundingBoxes.Where(b => _targetLabels.Contains(b.TagName)).ToList();
                if (boundingBoxes.Count > 0)
                {
                    var maxConfidence = boundingBoxes.Max(b => b.Probability);
                    var topBoundingBox = boundingBoxes.FirstOrDefault(b => b.Probability == maxConfidence);
                    double x = Math.Max(topBoundingBox.BoundingBox.Left, 0);
                    double y = Math.Max(topBoundingBox.BoundingBox.Top, 0);
                    double width = Math.Min(originalWidth - x, topBoundingBox.BoundingBox.Width);
                    double height = Math.Min(originalHeight - y, topBoundingBox.BoundingBox.Height);

                    x = originalWidth * x ;
                    y = originalHeight * y ;
                    width = originalWidth * width ;
                    height = originalHeight * height ;
                    result = new Rectangle(
                                (int)Math.Round(x),
                                (int)Math.Round(y),
                                (int)Math.Round(width),
                                (int)Math.Round(height));
                }

            }
            catch (Exception exception)
            {
                ;
            }
            return result;
        }

        public async Task<List<Rectangle>> DetectAllObjects(Mat frame)
        {
            var detectedObjectsRectangles = new List<Rectangle>();
            var boundingBoxes = new List<Prediction>();
            HttpResponseMessage response = await MakePredictionRequest(frame);

            var originalWidth = frame.Width;
            var originalHeight = frame.Height;

            string jsonStrng = await response.Content.ReadAsStringAsync();

            try
            {
                ApiPredictionModel parsedPredictionModel = JsonConvert.DeserializeObject<ApiPredictionModel>(jsonStrng); // Json convert to model class

                for (int i = 0; i < parsedPredictionModel.Predictions.Length; i++)
                {
                    boundingBoxes.Add(parsedPredictionModel.Predictions[i]);

                }
                boundingBoxes = boundingBoxes.Where(b => _targetLabels.Contains(b.TagName)).ToList();
                if (boundingBoxes.Count > 0)
                {
                    foreach (var bb in boundingBoxes)
                    {
                        if(bb.Probability > _probabilityThreshold)
                        {
                            double x = Math.Max(bb.BoundingBox.Left, 0);
                            double y = Math.Max(bb.BoundingBox.Top, 0);
                            double width = Math.Min(originalWidth - x, bb.BoundingBox.Width);
                            double height = Math.Min(originalHeight - y, bb.BoundingBox.Height);


                            x = originalWidth * x;
                            y = originalHeight * y;
                            width = originalWidth * width;
                            height = originalHeight * height;
                            var rectangle = new Rectangle(
                                (int)Math.Round(x),
                                (int)Math.Round(y),
                                (int)Math.Round(width),
                                (int)Math.Round(height));
                            detectedObjectsRectangles.Add(rectangle);
                        }
                        
                    }
                }

            }
            catch (Exception exception)
            {
                ;
            }
            return detectedObjectsRectangles;
        }

        private async Task<HttpResponseMessage> MakePredictionRequest(Mat frame)
        {
            var client = new HttpClient();
            var image = frame.ToBitmap();
            byte[] imageBmp = ToByteArray(image, ImageFormat.Bmp);
            // Request headers - replace this example key with your valid Prediction-Key.
            client.DefaultRequestHeaders.Add("Prediction-Key", "7c58542b2c8d44c3bed9c76487b658e3");

            // Prediction URL - replace this example URL with your valid Prediction URL.
            string url = "https://northeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/50750174-c9c5-4bf0-82f3-68a6c9027f2d/detect/iterations/TrainedModel1/image";

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.

            using (var content = new ByteArrayContent(imageBmp))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                return response;
            }
        }

        private byte[] ToByteArray(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

    }
}
