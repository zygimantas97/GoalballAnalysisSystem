using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.APIBasedObjectDetectionStrategy
{
    public class CVSPrediction
    {

        public static async Task<HttpResponseMessage> MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid Prediction-Key.
            client.DefaultRequestHeaders.Add("Prediction-Key", "7c58542b2c8d44c3bed9c76487b658e3");

            // Prediction URL - replace this example URL with your valid Prediction URL.
            string url = "https://northeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/50750174-c9c5-4bf0-82f3-68a6c9027f2d/detect/iterations/TrainedModel1/image";

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                return response;
            }
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }


}
