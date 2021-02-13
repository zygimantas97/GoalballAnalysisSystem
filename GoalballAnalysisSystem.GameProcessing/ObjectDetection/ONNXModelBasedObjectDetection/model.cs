﻿// This file was automatically generated by VS extension Windows Machine Learning Code Generator v3
// from model file model.onnx
// Warning: This file may get overwritten if you add add an onnx file with the same name
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
using System.IO;

namespace MNIST_Demo
{
    
    public sealed class modelInput
    {
        public ImageFeatureValue data; // BitmapPixelFormat: Bgra8, BitmapAlphaMode: Premultiplied, width: 416, height: 416
    }

    public sealed class modelOutput
    {
        public TensorFloat model_outputs0; // shape(-1,-1,13,13)
    }

    public sealed class modelModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static modelModel CreateFromStreamAsync()
        {
            var root = System.AppDomain.CurrentDomain.BaseDirectory;
            var fileName = "ObjectDetection\\ONNXModelBasedObjectDetection\\ONNXModel\\model.onnx";
            var fullPath = Path.Combine(root, fileName);

            modelModel learningModel = new modelModel();
            learningModel.model = LearningModel.LoadFromFilePath(fullPath);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<modelOutput> EvaluateAsync(VideoFrame frame)
        {
            modelInput input = new modelInput();
            input.data = ImageFeatureValue.CreateFromVideoFrame(frame);
            binding.Bind("data", input.data);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new modelOutput();
            output.model_outputs0 = result.Outputs["model_outputs0"] as TensorFloat;
            return output;
        }
    }
    
}

