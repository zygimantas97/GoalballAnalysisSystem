using Emgu.CV;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Models;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX.Settings;
using Microsoft.ML;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX
{
    public class ONNXObjectDetector : IObjectDetector
    {
        private readonly PredictionEngine<ImageInput, ImagePredictions> _predictionEngine;
        private readonly string[] _labels;
        private readonly List<string> _targetLabels;
        private readonly float _probabilityThreshold;

        public ONNXObjectDetector(List<string> targetLabels, float probabilityThreshold = 0.1f)
        {
            var context = new MLContext();
            var emptyData = new List<ImageInput>();
            var data = context.Data.LoadFromEnumerable(emptyData);

            var pipeline = context.Transforms.ResizeImages(
                resizing: ImageResizingEstimator.ResizingKind.Fill,
                outputColumnName: ModelSettings.Input,
                imageWidth: ImageSettings.ImageWidth,
                imageHeight: ImageSettings.ImageHeight,
                inputColumnName: nameof(ImageInput.Image))
                .Append(context.Transforms.ExtractPixels(outputColumnName: ModelSettings.Input))
                .Append(context.Transforms.ApplyOnnxModel(
                    modelFile: ModelSettings.ModelPath,
                    outputColumnName: ModelSettings.Output,
                    inputColumnName: ModelSettings.Input));

            var model = pipeline.Fit(data);
            _predictionEngine = context.Model.CreatePredictionEngine<ImageInput, ImagePredictions>(model);

            _labels = File.ReadAllLines(ModelSettings.LabelsPath);
            _targetLabels = targetLabels;
            _probabilityThreshold = probabilityThreshold;
        }

        public async Task<Dictionary<string, List<Rectangle>>> Detect(Mat frame)
        {
            var detectedObjects = new Dictionary<string, List<Rectangle>>();

            var bitmap = frame.ToBitmap();

            var originalWidth = bitmap.Width;
            var originalHeight = bitmap.Height;

            var objectPrediction = _predictionEngine.Predict(new ImageInput { Image = bitmap });

            var predictions = ParseOutputs(objectPrediction.Predictions, _labels, _probabilityThreshold);

            foreach(var label in _targetLabels)
            {
                var rectangles = predictions
                    .Where(p => p.Label == label)
                    .OrderByDescending(p => p.Confidence)
                    .Select(p => {
                        float x = Math.Max(p.Dimensions.X, 0);
                        float y = Math.Max(p.Dimensions.Y, 0);
                        float width = Math.Min(originalWidth - x, p.Dimensions.Width);
                        float height = Math.Min(originalHeight - y, p.Dimensions.Height);

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

            return detectedObjects;
        }

        // ----------
        // Helper methods
        // ----------
        private static List<BoundingBox> ParseOutputs(float[] modelOutput, string[] labels, float probabilityThreshold)
        {
            var boxes = new List<BoundingBox>();

            for (int row = 0; row < PredictionSettings.RowCount; row++)
            {
                for (int column = 0; column < PredictionSettings.ColumnCount; column++)
                {
                    for (int box = 0; box < PredictionSettings.Anchors.Length; box++)
                    {
                        var channel = box * (labels.Length + PredictionSettings.FeaturesPerBox);

                        var boundingBoxPrediction = ExtractBoundingBoxPrediction(modelOutput, row, column, channel);

                        var mappedBoundingBox = MapBoundingBoxToCell(row, column, box, boundingBoxPrediction);

                        if (boundingBoxPrediction.Confidence < probabilityThreshold)
                            continue;

                        float[] classProbabilities = ExtractClassProbabilities(modelOutput, row, column, channel, boundingBoxPrediction.Confidence, labels);

                        var (topProbability, topIndex) = classProbabilities.Select((probability, index) => (Score: probability, Index: index)).Max();

                        if (topProbability < probabilityThreshold)
                            continue;

                        boxes.Add(new BoundingBox
                        {
                            Dimensions = mappedBoundingBox,
                            Confidence = topProbability,
                            Label = labels[topIndex]
                        });
                    }
                }
            }

            return boxes;
        }

        private static BoundingBoxDimensions MapBoundingBoxToCell(int row, int column, int box, BoundingBoxPrediction boxDimensions)
        {
            const float cellWidth = ImageSettings.ImageWidth / PredictionSettings.ColumnCount;
            const float cellHeight = ImageSettings.ImageHeight / PredictionSettings.RowCount;

            var mappedBox = new BoundingBoxDimensions
            {
                X = (row + Sigmoid(boxDimensions.X)) * cellWidth,
                Y = (column + Sigmoid(boxDimensions.Y)) * cellHeight,
                Width = (float)Math.Exp(boxDimensions.Width) * cellWidth * PredictionSettings.Anchors[box].x,
                Height = (float)Math.Exp(boxDimensions.Height) * cellHeight * PredictionSettings.Anchors[box].y,
            };

            // The x,y coordinates from the (mapped) bounding box prediction represent the center
            // of the bounding box. We adjust them here to represent the top left corner.
            mappedBox.X -= mappedBox.Width / 2;
            mappedBox.Y -= mappedBox.Height / 2;

            return mappedBox;
        }

        private static BoundingBoxPrediction ExtractBoundingBoxPrediction(float[] modelOutput, int row, int column, int channel)
        {
            return new BoundingBoxPrediction
            {
                X = modelOutput[GetOffset(row, column, channel++)],
                Y = modelOutput[GetOffset(row, column, channel++)],
                Width = modelOutput[GetOffset(row, column, channel++)],
                Height = modelOutput[GetOffset(row, column, channel++)],
                Confidence = Sigmoid(modelOutput[GetOffset(row, column, channel++)])
            };
        }

        private static float[] ExtractClassProbabilities(float[] modelOutput, int row, int column, int channel, float confidence, string[] labels)
        {
            var classProbabilitiesOffset = channel + PredictionSettings.FeaturesPerBox;
            float[] classProbabilities = new float[labels.Length];
            for (int classProbability = 0; classProbability < labels.Length; classProbability++)
                classProbabilities[classProbability] = modelOutput[GetOffset(row, column, classProbability + classProbabilitiesOffset)];
            return Softmax(classProbabilities).Select(p => p * confidence).ToArray();
        }

        private static float Sigmoid(float value)
        {
            var k = (float)Math.Exp(value);
            return k / (1.0f + k);
        }

        private static float[] Softmax(float[] classProbabilities)
        {
            var max = classProbabilities.Max();
            var exp = classProbabilities.Select(v => Math.Exp(v - max));
            var sum = exp.Sum();
            return exp.Select(v => (float)v / (float)sum).ToArray();
        }

        private static int GetOffset(int row, int column, int channel)
        {
            const int channelStride = PredictionSettings.RowCount * PredictionSettings.ColumnCount;
            return (channel * channelStride) + (column * PredictionSettings.ColumnCount) + row;
        }
    }
}
