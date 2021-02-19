using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TensorFlow;

namespace GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.TensorFlowSharpBasedObjectDetection
{
	
    public class TensorFlowSharpBasedObjectDetectionStrategy
	{
		//private static IEnumerable<CatalogItem> _catalog;
		private static string _currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static string _input_relative = "test_images/input.jpg";
		private static string _output_relative = "test_images/output.jpg";
		private static string _input = Path.Combine(_currentDir, _input_relative);
		private static string _output = Path.Combine(_currentDir, _output_relative);
		private static string _catalogPath;
		private static string _modelPath = Path.Combine(_currentDir, "ObjectDetection\\TensorFlowBasedObjectDetection\\TensorFlowModel\\model.pb");

		private static double MIN_SCORE_FOR_OBJECT_HIGHLIGHTING = 0.5;

		/*
		static OptionSet options = new OptionSet()
		{
			{ "input_image=",  "Specifies the path to an image ", v => _input = v },
			{ "output_image=",  "Specifies the path to the output image with detected objects", v => _output = v },
			{ "catalog=", "Specifies the path to the .pbtxt objects catalog", v=> _catalogPath = v},
			{ "model=", "Specifies the path to the trained model", v=> _modelPath = v},
			{ "h|help", v => Help () }
		};
		*/

		/// <summary>
		/// Run the ExampleObjectDetection util from command line. Following options are available:
		/// input_image - optional, the path to the image for processing (the default is 'test_images/input.jpg')
		/// output_image - optional, the path where the image with detected objects will be saved (the default is 'test_images/output.jpg')
		/// catalog - optional, the path to the '*.pbtxt' file (by default, 'mscoco_label_map.pbtxt' been loaded)
		/// model - optional, the path to the '*.pb' file (by default, 'frozen_inference_graph.pb' model been used, but you can download any other from here 
		/// https://github.com/tensorflow/models/blob/master/object_detection/g3doc/detection_model_zoo.md or train your own)
		/// 
		/// for instance, 
		/// ExampleObjectDetection --input_image="/demo/input.jpg" --output_image="/demo/output.jpg" --catalog="/demo/mscoco_label_map.pbtxt" --model="/demo/frozen_inference_graph.pb"
		/// </summary>
		/// <param name="args"></param>
		public void DetectAllObjects(string fileName)
		{
			/*
			options.Parse(args);

			if (_catalogPath == null)
			{
				_catalogPath = DownloadDefaultTexts(_currentDir);
			}

			if (_modelPath == null)
			{
				_modelPath = DownloadDefaultModel(_currentDir);
			}

			_catalog = CatalogUtil.ReadCatalogItems(_catalogPath);
			
			var fileTuples = new List<(string input, string output)>() { (_input, _output) };
			
			string modelFile = _modelPath;
			*/

			using (var graph = new TFGraph())
			{
				var model = File.ReadAllBytes(_modelPath);
				graph.Import(new TFBuffer(model));

				using (var session = new TFSession(graph))
				{
					//Console.WriteLine("Detecting objects");

					
					
					//var tensor = ImageUtil.CreateTensorFromImageFile (tuple.input, TFDataType.UInt8);
					var tensor = ImageToTensorGrayScale(fileName);
					var runner = session.GetRunner();


					runner
						.AddInput(graph["image_tensor"][0], tensor)
						.Fetch(
						graph["detection_boxes"][0],
						graph["detection_scores"][0],
						graph["detection_classes"][0],
						graph["num_detections"][0]);
					var output = runner.Run();

					var boxes = (float[,,])output[0].GetValue(jagged: false);
					var scores = (float[,])output[1].GetValue(jagged: false);
					var classes = (float[,])output[2].GetValue(jagged: false);
					var num = (float[])output[3].GetValue(jagged: false);

					/*
					DrawBoxes(boxes, scores, classes, tuple.input, tuple.output, MIN_SCORE_FOR_OBJECT_HIGHLIGHTING);
					Console.WriteLine($"Done. See {_output_relative}");
					*/
				}
			}
		}

		private static void DrawBoxes(float[,,] boxes, float[,] scores, float[,] classes, string inputFile, string outputFile, double minScore)
		{
			var x = boxes.GetLength(0);
			var y = boxes.GetLength(1);
			var z = boxes.GetLength(2);

			float ymin = 0, xmin = 0, ymax = 0, xmax = 0;

			using (var editor = new ImageEditor(inputFile, outputFile))
			{
				for (int i = 0; i < x; i++)
				{
					for (int j = 0; j < y; j++)
					{
						if (scores[i, j] < minScore) continue;

						for (int k = 0; k < z; k++)
						{
							var box = boxes[i, j, k];
							switch (k)
							{
								case 0:
									ymin = box;
									break;
								case 1:
									xmin = box;
									break;
								case 2:
									ymax = box;
									break;
								case 3:
									xmax = box;
									break;
							}

						}

						int value = Convert.ToInt32(classes[i, j]);
						//CatalogItem catalogItem = _catalog.FirstOrDefault(item => item.Id == value);
						//editor.AddBox(xmin, xmax, ymin, ymax, $"{catalogItem.DisplayName} : {(scores[i, j] * 100).ToString("0")}%");
					}
				}
			}
		}

		public static TensorFlow.TFTensor ImageToTensorGrayScale(string file)
		{
			using (System.Drawing.Bitmap image = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
			{
				var matrix = new float[1, image.Size.Height, image.Size.Width, 1];
				for (var iy = 0; iy < image.Size.Height; iy++)
				{
					for (int ix = 0, index = iy * image.Size.Width; ix < image.Size.Width; ix++, index++)
					{
						System.Drawing.Color pixel = image.GetPixel(ix, iy);
						matrix[0, iy, ix, 0] = pixel.B / 255.0f;
					}
				}
				TensorFlow.TFTensor tensor = matrix;
				return tensor;
			}
		}
	}
	
}
