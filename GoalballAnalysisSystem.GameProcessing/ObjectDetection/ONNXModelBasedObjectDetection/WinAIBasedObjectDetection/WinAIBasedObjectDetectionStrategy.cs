﻿// The steps implemented in the object detection sample code: 
// 1. for an image of width and height being (w, h) pixels, resize image to (w', h'), where w/h = w'/h' and w' x h' = 262144
// 2. resize network input size to (w', h')
// 3. pass the image to network and do inference
// (4. if inference speed is too slow for you, try to make w' x h' smaller, which is defined with DEFAULT_INPUT_SIZE (in object_detection.py or ObjectDetection.cs))
// <copyright file="ObjectDetection.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

/// Script for CustomVision's exported object detection model.

using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.WinAIBasedObjectDetection.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.WinAIBasedObjectDetection
{
    public sealed class modelOutput
    {
        public TensorFloat model_outputs0; // shape(-1,-1,13,13)
    }

    public class WinAIBasedObjectDetectionStrategy
    {
        private static readonly float[] Anchors = new float[] { 0.573f, 0.677f, 1.87f, 2.06f, 3.34f, 5.47f, 7.88f, 3.53f, 9.77f, 9.17f };

        private readonly IList<string> labels;
        private readonly int maxDetections;
        private readonly float probabilityThreshold;
        private readonly float iouThreshold;
        private LearningModel model;
        private LearningModelSession session;
        private const int imageInputSize = 512 * 512;

        public WinAIBasedObjectDetectionStrategy(int maxDetections = 20, float probabilityThreshold = 0.1f, float iouThreshold = 0.45f)
        {
            this.labels = File.ReadAllLines("./ObjectDetection/ONNXModelBasedObjectDetection/ONNXModel/labels.txt");
            this.maxDetections = maxDetections;
            this.probabilityThreshold = probabilityThreshold;
            this.iouThreshold = iouThreshold;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="file">The ONNX file</param>
        public void Init()
        {
            var root = System.AppDomain.CurrentDomain.BaseDirectory;
            var fileName = "ObjectDetection\\ONNXModelBasedObjectDetection\\ONNXModel\\model.onnx";
            var fullPath = Path.Combine(root, fileName);

            this.model = LearningModel.LoadFromFilePath(fullPath);
            this.session = new LearningModelSession(this.model);

            Debug.Assert(this.model.InputFeatures.Count == 1, "The number of input must be 1");
            Debug.Assert(this.model.OutputFeatures.Count == 1, "The number of output must be 1");
        }

        /// <summary>
        /// Detect objects from the given image.
        /// The input image must be 416x416.
        /// </summary>
        public async Task<IList<PredictionModel>> PredictImageAsync(VideoFrame image)
        {
            var output = new modelOutput();
            var imageFeature = ImageFeatureValue.CreateFromVideoFrame(image);
            var bindings = new LearningModelBinding(session);
            bindings.Bind("data", imageFeature);
            bindings.Bind("model_outputs0", output.model_outputs0);
            var result = await session.EvaluateAsync(bindings, "0");

            return Postprocess(output.model_outputs0);
        }

        private static float Logistic(float x)
        {
            if (x > 0)
            {
                return (float)(1 / (1 + Math.Exp(-x)));
            }
            else
            {
                var e = Math.Exp(x);
                return (float)(e / (1 + e));
            }
        }

        /// <summary>
        /// Calculate Intersection over Union (IOU) for the given 2 bounding boxes.
        /// </summary>
        private static float CalculateIOU(BoundingBox box0, BoundingBox box1)
        {
            var x1 = Math.Max(box0.Left, box1.Left);
            var y1 = Math.Max(box0.Top, box1.Top);
            var x2 = Math.Min(box0.Left + box0.Width, box1.Left + box1.Width);
            var y2 = Math.Min(box0.Top + box0.Height, box1.Top + box1.Height);
            var w = Math.Max(0, x2 - x1);
            var h = Math.Max(0, y2 - y1);

            return w * h / ((box0.Width * box0.Height) + (box1.Width * box1.Height) - (w * h));
        }

        /// <summary>
        /// Extract bounding boxes and their probabilities from the prediction output.
        /// </summary>
        private ExtractedBoxes ExtractBoxes(TensorFloat predictionOutput, float[] anchors)
        {
            var shape = predictionOutput.Shape;
            Debug.Assert(shape.Count == 4, "The model output has unexpected shape");
            Debug.Assert(shape[0] == 1, "The batch size must be 1");

            IReadOnlyList<float> outputs = predictionOutput.GetAsVectorView();

            var numAnchor = anchors.Length / 2;
            var channels = shape[1];
            var height = shape[2];
            var width = shape[3];

            Debug.Assert(channels % numAnchor == 0);
            var numClass = (channels / numAnchor) - 5;

            Debug.Assert(numClass == this.labels.Count);

            var boxes = new List<BoundingBox>();
            var probs = new List<float[]>();
            for (int gridY = 0; gridY < height; gridY++)
            {
                for (int gridX = 0; gridX < width; gridX++)
                {
                    int offset = 0;
                    int stride = (int)(height * width);
                    int baseOffset = gridX + gridY * (int)width;

                    for (int i = 0; i < numAnchor; i++)
                    {
                        var x = (Logistic(outputs[baseOffset + (offset++ * stride)]) + gridX) / width;
                        var y = (Logistic(outputs[baseOffset + (offset++ * stride)]) + gridY) / height;
                        var w = (float)Math.Exp(outputs[baseOffset + (offset++ * stride)]) * anchors[i * 2] / width;
                        var h = (float)Math.Exp(outputs[baseOffset + (offset++ * stride)]) * anchors[i * 2 + 1] / height;

                        x = x - (w / 2);
                        y = y - (h / 2);

                        var objectness = Logistic(outputs[baseOffset + (offset++ * stride)]);

                        var classProbabilities = new float[numClass];
                        for (int j = 0; j < numClass; j++)
                        {
                            classProbabilities[j] = outputs[baseOffset + (offset++ * stride)];
                        }
                        var max = classProbabilities.Max();
                        for (int j = 0; j < numClass; j++)
                        {
                            classProbabilities[j] = (float)Math.Exp(classProbabilities[j] - max);
                        }
                        var sum = classProbabilities.Sum();
                        for (int j = 0; j < numClass; j++)
                        {
                            classProbabilities[j] *= objectness / sum;
                        }

                        if (classProbabilities.Max() > this.probabilityThreshold)
                        {
                            boxes.Add(new BoundingBox(x, y, w, h));
                            probs.Add(classProbabilities);
                        }
                    }
                    Debug.Assert(offset == channels);
                }
            }

            Debug.Assert(boxes.Count == probs.Count);
            return new ExtractedBoxes(boxes, probs);
        }

        /// <summary>
        /// Remove overlapping predictions and return top-n predictions.
        /// </summary>
        private List<PredictionModel> SuppressNonMaximum(ExtractedBoxes extractedBoxes)
        {
            var predictions = new List<PredictionModel>();

            if (extractedBoxes.Probabilities.Count > 0)
            {
                var maxProbs = extractedBoxes.Probabilities.Select(x => x.Max()).ToArray();

                while (predictions.Count < this.maxDetections)
                {
                    var max = maxProbs.Max();
                    if (max < this.probabilityThreshold)
                    {
                        break;
                    }
                    var index = Array.IndexOf(maxProbs, max);
                    var maxClass = Array.IndexOf(extractedBoxes.Probabilities[index], max);

                    predictions.Add(new PredictionModel(max, this.labels[maxClass], extractedBoxes.Boxes[index]));

                    for (int i = 0; i < extractedBoxes.Boxes.Count; i++)
                    {
                        if (CalculateIOU(extractedBoxes.Boxes[index], extractedBoxes.Boxes[i]) > this.iouThreshold)
                        {
                            extractedBoxes.Probabilities[i][maxClass] = 0;
                            maxProbs[i] = extractedBoxes.Probabilities[i].Max();
                        }
                    }
                }
            }

            return predictions;
        }

        private List<PredictionModel> Postprocess(TensorFloat predictionOutputs)
        {
            var extractedBoxes = this.ExtractBoxes(predictionOutputs, WinAIBasedObjectDetectionStrategy.Anchors);
            return this.SuppressNonMaximum(extractedBoxes);
        }

        public async Task<SoftwareBitmap> ResizeBitmap(SoftwareBitmap softwareBitmap, int targetWidth, int targetHeight)
        {
            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);
                encoder.BitmapTransform.ScaledWidth = (uint)targetWidth;
                encoder.BitmapTransform.ScaledHeight = (uint)targetHeight;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.NearestNeighbor;
                encoder.IsThumbnailGenerated = false;

                await encoder.FlushAsync();
                stream.Seek(0);

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                var result = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                return result;
            }
        }
        public List<PredictionModel> Process(TensorFloat predictionOutputs)
        {
            var extractedBoxes = this.ExtractBoxes(predictionOutputs, WinAIBasedObjectDetectionStrategy.Anchors);
            return this.SuppressNonMaximum(extractedBoxes);
        }

        public static async Task<SoftwareBitmap> Resize(SoftwareBitmap softwareBitmap, int targetWidth, int targetHeight)
        {
            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);
                encoder.BitmapTransform.ScaledWidth = (uint)targetWidth;
                encoder.BitmapTransform.ScaledHeight = (uint)targetHeight;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.NearestNeighbor;
                encoder.IsThumbnailGenerated = false;

                await encoder.FlushAsync();
                stream.Seek(0);

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                var result = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                return result;
            }
        }
    }
}
