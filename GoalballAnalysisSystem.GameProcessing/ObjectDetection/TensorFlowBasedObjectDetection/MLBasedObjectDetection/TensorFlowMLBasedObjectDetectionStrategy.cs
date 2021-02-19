using Emgu.CV;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.MLBasedObjectDetection.Models;
using Microsoft.ML;
using Microsoft.ML.Transforms.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.MLBasedObjectDetection
{
    public class TensorFlowMLBasedObjectDetectionStrategy
    {
        private const int _rowCount = 13;
        private const int _columnCount = 13;
        private const int _featuresPerBox = 5;
        private static readonly (float x, float y)[] _boxAnchors = { (0.573f, 0.677f), (1.87f, 2.06f), (3.34f, 5.47f), (7.88f, 3.53f), (9.77f, 9.17f) };

        private readonly PredictionEngine<FrameInput, FramePredictions> _predictionEngine;
        private readonly string[] _labels;
        private readonly List<string> _targetLabels;
        private readonly float _probabilityThreshold;

        public TensorFlowMLBasedObjectDetectionStrategy(List<string> targetLabels, float probabilityThreshold = 0.1f)
        {
            var context = new MLContext();
            var emptyData = new List<FrameInput>();
            var data = context.Data.LoadFromEnumerable(emptyData);

            var pipeline = context.Transforms.ResizeImages( outputColumnName: TensorFlowModelSettings.InputTensorName,
                                                            imageWidth: ImageSettings.ImageWidth,
                                                            imageHeight: ImageSettings.ImageHeight,
                                                            inputColumnName: nameof(FrameInput.Image))
                    .Append(context.Transforms.ExtractPixels(outputColumnName: TensorFlowModelSettings.InputTensorName,
                                                            interleavePixelColors: ImageSettings.ChannelsLast,
                                                            offsetImage: ImageSettings.Mean))
                    .Append(context.Model.LoadTensorFlowModel(TensorFlowModelSettings.TensorFlowModelLocation)
                        .ScoreTensorFlowModel(outputColumnNames: new[] { TensorFlowModelSettings.OutputTensorName },
                                            inputColumnNames: new[] { TensorFlowModelSettings.InputTensorName },
                                            addBatchDimensionInput: false));

            var model = pipeline.Fit(data);
            _predictionEngine = context.Model.CreatePredictionEngine<FrameInput, FramePredictions>(model);

            _labels = File.ReadAllLines(TensorFlowModelSettings.TensorFlowLabelsLocation);
            _targetLabels = targetLabels;
            _probabilityThreshold = probabilityThreshold;
        }

        public Rectangle DetectObject(Mat frame)
        {
            var image = frame.ToBitmap();

            var originalWidth = image.Width;
            var originalHeight = image.Height;

            var prediction = _predictionEngine.Predict(new FrameInput { Image = image });

            var boundingBoxes = ParseOutputs(prediction.ObjectType, _labels, _probabilityThreshold);
            boundingBoxes = boundingBoxes.Where(b => _targetLabels.Contains(b.Label)).ToList();

            if (boundingBoxes.Count > 0)
            {
                var maxConfidence = boundingBoxes.Max(b => b.Confidence);
                var topBoundingBox = boundingBoxes.FirstOrDefault(b => b.Confidence == maxConfidence);

                float x = Math.Max(topBoundingBox.Dimensions.X, 0);
                float y = Math.Max(topBoundingBox.Dimensions.Y, 0);
                float width = Math.Min(originalWidth - x, topBoundingBox.Dimensions.Width);
                float height = Math.Min(originalHeight - y, topBoundingBox.Dimensions.Height);

                // fit to current image size
                x = originalWidth * x / ImageSettings.ImageWidth;
                y = originalHeight * y / ImageSettings.ImageHeight;
                width = originalWidth * width / ImageSettings.ImageWidth;
                height = originalHeight * height / ImageSettings.ImageHeight;

                return new Rectangle(
                    (int)Math.Round(x),
                    (int)Math.Round(y),
                    (int)Math.Round(width),
                    (int)Math.Round(height));
            }
            return Rectangle.Empty;
        }

        public List<Rectangle> DetectAllObjects(Bitmap image)
        {
            var detectedObjectsRectangles = new List<Rectangle>();

            //var image = frame.ToBitmap();

            var originalWidth = image.Width;
            var originalHeight = image.Height;

            var prediction = _predictionEngine.Predict(new FrameInput { Image = image });

            var boundingBoxes = ParseOutputs(prediction.ObjectType, _labels, _probabilityThreshold);
            boundingBoxes = boundingBoxes.Where(b => _targetLabels.Contains(b.Label)).ToList();

            if (boundingBoxes.Count > 0)
            {
                foreach (var bb in boundingBoxes)
                {
                    float x = Math.Max(bb.Dimensions.X, 0);
                    float y = Math.Max(bb.Dimensions.Y, 0);
                    float width = Math.Min(originalWidth - x, bb.Dimensions.Width);
                    float height = Math.Min(originalHeight - y, bb.Dimensions.Height);

                    // fit to current image size
                    x = originalWidth * x / ImageSettings.ImageWidth;
                    y = originalHeight * y / ImageSettings.ImageHeight;
                    width = originalWidth * width / ImageSettings.ImageWidth;
                    height = originalHeight * height / ImageSettings.ImageHeight;

                    var rectangle = new Rectangle(
                        (int)Math.Round(x),
                        (int)Math.Round(y),
                        (int)Math.Round(width),
                        (int)Math.Round(height));
                    detectedObjectsRectangles.Add(rectangle);
                }
            }
            return detectedObjectsRectangles;
        }

        // ----------
        // Helper methods
        // ----------
        private static List<BoundingBox> ParseOutputs(float[] modelOutput, string[] labels, float probabilityThreshold)
        {
            var boxes = new List<BoundingBox>();

            for (int row = 0; row < _rowCount; row++)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    for (int box = 0; box < _boxAnchors.Length; box++)
                    {
                        var channel = box * (labels.Length + _featuresPerBox);

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
            const float cellWidth = ImageSettings.ImageWidth / _columnCount;
            const float cellHeight = ImageSettings.ImageHeight / _rowCount;

            var mappedBox = new BoundingBoxDimensions
            {
                X = (row + Sigmoid(boxDimensions.X)) * cellWidth,
                Y = (column + Sigmoid(boxDimensions.Y)) * cellHeight,
                Width = (float)Math.Exp(boxDimensions.Width) * cellWidth * _boxAnchors[box].x,
                Height = (float)Math.Exp(boxDimensions.Height) * cellHeight * _boxAnchors[box].y,
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
            var classProbabilitiesOffset = channel + _featuresPerBox;
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
            const int channelStride = _rowCount * _columnCount;
            return (channel * channelStride) + (column * _columnCount) + row;
        }
    }
}
