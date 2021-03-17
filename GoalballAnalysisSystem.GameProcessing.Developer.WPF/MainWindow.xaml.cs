using GoalballAnalysisSystem.GameProcessing.BallTracker;
using GoalballAnalysisSystem.GameProcessing.PlayersTracker;
using GoalballAnalysisSystem.GameProcessing.PlayFieldTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using Microsoft.Win32;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.ComponentModel;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.MLBasedObjectDetection;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.WinAIBasedObjectDetection;
using Windows.Storage;
using Windows.Media;
using Windows.Graphics.Imaging;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.APIBasedObjectDetectionStrategy;
using Windows.Storage.Pickers;
using MNIST_Demo;
using Windows.AI.MachineLearning;
using OnnxObjectDetection;
using Microsoft.ML;
using BoundingBox = OnnxObjectDetection.BoundingBox;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.MLBasedObjectDetection;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.TensorFlowBasedObjectDetection.TensorFlowSharpBasedObjectDetection;

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool finished = false;
        private Stopwatch sw = new Stopwatch();
        private Mat firstFrame = new Mat();
        private int objID = 1;

        private OnnxOutputParser outputParser;
        private PredictionEngine<ImageInputData, CustomVisionPrediction> customVisionPredictionEngine;

        private static readonly string modelsDirectory = Path.Combine(Environment.CurrentDirectory, @"ObjectDetection\ONNXModelBasedObjectDetection\ML");

        private Bitmap globalBitmap;
        public GameAnalyzer GameAnalyzer { get; private set; }
        ObjectDetection.ONNXJulius.Processing _predictionModel;
        private Image<Bgr, byte> selectedPart;

        Rectangle _selectedROI = Rectangle.Empty;
        System.Drawing.Point _startROI;
        bool _isSelecting = false;

        private int _frameNo = 0;

        private string _progress;

        public string Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private EmguCVTrackersBasedMOT _playersTracker;
        private IMOT _playersTracker;

        public event PropertyChangedEventHandler PropertyChanged;

        public  MainWindow()
        {
            InitializeComponent();
            Image<Bgr, byte> imageBoxBackground = new Image<Bgr, byte>(imageBox.Width, imageBox.Height, new Bgr(0, 0, 0));
            imageBox.Image = imageBoxBackground;
            DataContext = this;
            _predictionModel = new ObjectDetection.ONNXJulius.Processing();

            LoadModel();
        }

        private void LoadModel()
        {
            // Check for an Onnx model exported from Custom Vision
            var customVisionExport = Directory.GetFiles(modelsDirectory, "*.zip").FirstOrDefault();

            // If there is one, use it.
            if (customVisionExport != null)
            {
                var customVisionModel = new CustomVisionModel(customVisionExport);
                var modelConfigurator = new OnnxModelConfigurator(customVisionModel);

                outputParser = new OnnxOutputParser(customVisionModel);
                customVisionPredictionEngine = modelConfigurator.GetMlNetPredictionEngine<CustomVisionPrediction>();
            }
        }

        private void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if(result == true)
            {
                var capture = new VideoCapture(openFileDialog.FileName);
                capture.Read(firstFrame);
                imageBox.Image = firstFrame;
                //IPlayFieldTracker playFieldTracker = new ColorBasedPlayFieldTracker();
                
                
                
                //var gameZoneCorners = playFieldTracker.GetPlayFieldCorners(firstFrame);

                //IBallTracker ballTracker = new CNNBasedBallTracker();

                IObjectDetectionStrategy ballTracker = new MLBasedObjectDetectionStrategy(new List<string> { "ball" });
                //_playersTracker = new EmguCVTrackersBasedMOT();
                _playersTracker = new EmguCVTrackersBasedMOT();



                GameAnalyzer = new GameAnalyzer(openFileDialog.FileName,
                                                new System.Drawing.Point(0,0),
                                                new System.Drawing.Point(0, 0),
                                                new System.Drawing.Point(0, 0),
                                                new System.Drawing.Point(0, 0),
                                                ballTracker, _playersTracker);

                GameAnalyzer.FrameChanged += GameAnalyzer_FrameChanged;
                GameAnalyzer.ProcessingFinished += GameAnalyzer_ProcessingFinished;
                //VideoStackPanel.Visibility = Visibility.Visible;
                //GameAnalyzer.Start();
                
                
            }
        }

        private void GameAnalyzer_ProcessingFinished(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GameAnalyzer_FrameChanged(object sender, EventArgs e)
        {
            try
            {
                if(GameAnalyzer.Status != GameAnalyzerStatus.Stopped)
                {
                    _frameNo++;
                    var image = GameAnalyzer.CurrentFrame;
                    if (image != null && image.Data != null)
                    {
                        imageBox.Image = image;
                    }
                    
                    
                    Progress = String.Format("{0:0.00} {1}", (double)_frameNo / (double)GameAnalyzer.FrameCount * 100, "%");
                    if(Math.Abs((double)_frameNo / (double)GameAnalyzer.FrameCount - 1) < 0.0001 && !finished)
                    {
                        Finish();
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("ERROR: FrameChanged");
            }
            
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            if(GameAnalyzer.Status == GameAnalyzerStatus.NotStarted)
            {
                GameAnalyzer.Start();
                sw.Start();
            }
            else
            {
                GameAnalyzer.Resume();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Stop();
        }

        private void DetectObjectMLButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (_selectedROI != Rectangle.Empty)
            {
                double horizontalScale = GameAnalyzer.CurrentFrame.Width / imageBox.Width;
                double verticalScale = GameAnalyzer.CurrentFrame.Height / imageBox.Height;

                Rectangle rectangle = new Rectangle((int)(_selectedROI.X * horizontalScale),
                                                    (int)(_selectedROI.Y * verticalScale),
                                                    (int)(_selectedROI.Width * horizontalScale),
                                                    (int)(_selectedROI.Height * verticalScale));
                //_playersTracker.AddTrackingObject(GameAnalyzer.CurrentFrame.Mat, rectangle);
                
                var image = GameAnalyzer.CurrentFrame.Clone();
                image.ROI = rectangle;
                selectedPart = new Image<Bgr, byte>(rectangle.Width, rectangle.Height);
                image.CopyTo(selectedPart);
                _selectedROI = Rectangle.Empty;
                imageBox.Invalidate();
            }
            */
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Image<Bgr, byte> img;
                Bitmap image = new Bitmap(System.Drawing.Image.FromFile(openFileDialog.FileName));
                img = image.ToImage<Bgr, byte>();

                var objectDetectionStrategy = new MLBasedObjectDetectionStrategy(new List<string>() { "ball" }, 0.1f);
                var rectangles = objectDetectionStrategy.DetectAllObjects(image);

                if(rectangles.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Not detected");
                }
                else
                {
                    foreach(var rect in rectangles)
                    {
                        //CvInvoke.PutText(img, rect.X.ToString() + "," + rect.Y.ToString(), new System.Drawing.Point(rect.X, rect.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        CvInvoke.Rectangle(img, rect, new MCvScalar(0, 0, 255), 4);
                    }
                    
                    imageBox.Image = img;
                    //System.Windows.Forms.MessageBox.Show("Detected: " + rectangles.Count);
                }
            }

        }

        private void imageBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(GameAnalyzer.Status == GameAnalyzerStatus.Paused || GameAnalyzer.Status == GameAnalyzerStatus.NotStarted)
            {
                _isSelecting = true;
                _startROI = e.Location;
            }
        }

        private void imageBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_isSelecting)
            {
                int width = Math.Max(_startROI.X, e.X) - Math.Min(_startROI.X, e.X);
                int height = Math.Max(_startROI.Y, e.Y) - Math.Min(_startROI.Y, e.Y);
                _selectedROI = new Rectangle(Math.Min(_startROI.X, e.X), Math.Min(_startROI.Y, e.Y), width, height);
                imageBox.Invalidate();
            }
        }

        private void imageBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_isSelecting)
            {
                _isSelecting = false;
            }
        }

        private void imageBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_isSelecting)
            {
                using(System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(pen, _selectedROI);
                }
            }
        }

        private void ClearROIButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedROI = Rectangle.Empty;
            imageBox.Invalidate();
        }

        private void loadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Image<Gray, byte> template = new Image<Gray, byte>("ball_template.jpg");
                Image<Gray, byte> template = selectedPart.Convert<Gray, byte>();
                FeatureBasedObjectDetectionStrategy ballDetector = new FeatureBasedObjectDetectionStrategy(template);

                var image = GameAnalyzer.CurrentFrame.Clone();
                
                var rect = ballDetector.DetectObject(image.Mat);

                CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255), 5);
                imageBox.Image = image;
                System.Windows.Forms.MessageBox.Show(rect.X.ToString() + ":" + rect.Y.ToString());
                

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void sendFrame_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var videoCapture = new VideoCapture(openFileDialog.FileName);
                Mat firstFrame = new Mat();
                videoCapture.Read(firstFrame);
                Image<Bgr, byte> image = new Image<Bgr, byte>(openFileDialog.FileName);
                var convertedImage = (Bitmap)System.Drawing.Image.FromFile(openFileDialog.FileName);
                APIBasedObjectDetectionStrategy a = new APIBasedObjectDetectionStrategy(new List<string>() { "ball", "player", "BottomLeft", "BottomRight", "TopLeft", "TopRight" }, 0.1f);

                List<Rectangle> rectangles = new List<Rectangle>();
                Task<List<Rectangle>> task = Task.Run<List<Rectangle>>(async () => await a.DetectAllObjects(firstFrame));
                rectangles = task.Result;

                if (rectangles.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Not detected");
                }
                else
                {
                    foreach (var rect in rectangles)
                    {
                        CvInvoke.PutText(image, rect.X.ToString() + "," + rect.Y.ToString(), new System.Drawing.Point(rect.X, rect.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        CvInvoke.Rectangle(image, rect, new MCvScalar(255, 0, 0), 4);
                    }

                    imageBox.Image = image;
                    System.Windows.Forms.MessageBox.Show("Detected: " + rectangles.Count);
                }
            }
        }

        private async void DetectObjectWinAIButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {

                Bitmap bitmapImage = new Bitmap(System.Drawing.Image.FromFile(openFileDialog.FileName));
                globalBitmap = (Bitmap)bitmapImage.Clone();
                imageBox.Image = bitmapImage.ToImage<Bgr, byte>();
                await ParseWebCamFrame(bitmapImage);


                /*
                var objectDetectionStrategy = new WinAIBasedObjectDetectionStrategy();
                objectDetectionStrategy.Init();

                // Reading image
                Image<Bgra, byte> image = new Image<Bgra, byte>(openFileDialog.FileName);

                modelInput input = new modelInput();
                MNIST_Demo.modelOutput output = new MNIST_Demo.modelOutput();
                modelModel model = modelModel.CreateFromStreamAsync();


                SoftwareBitmap softwareBitmap = BitmapToSoftwareBitmap(image.ToBitmap());
                softwareBitmap = await WinAIBasedObjectDetectionStrategy.Resize(softwareBitmap, 416, 416);
                VideoFrame vf = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
                input.data = ImageFeatureValue.CreateFromVideoFrame(vf);

                // Evaluate the model
                output = await model.EvaluateAsync(vf);
                var results = objectDetectionStrategy.Process(output.model_outputs0);
                 




                //Bitmap bitmap = SoftwareBitmapToBitmap(softwareBitmap);
                */
                /*var root = System.AppDomain.CurrentDomain.BaseDirectory;
                var fileName = "ObjectDetection\\ONNXModelBasedObjectDetection\\ONNXModel\\model.onnx";
                var fullPath = Path.Combine(root, fileName);

                var storageFile = await StorageFile.GetFileFromPathAsync(openFileDialog.FileName);
                SoftwareBitmap softwareBitmap;
                using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);

                    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                }
                var videoFrame = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);


                Bitmap bitmap = image.ToBitmap();
                using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                {
                    bitmap.Save(stream, ImageFormat.Bmp);
                    Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                }*/

                /*SoftwareBitmapSource source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(softwareBitmap);
                imageBox = source;*/



                // Detecting objects
                //var results = await objectDetectionStrategy.PredictImageAsync(videoFrame);

                /*
                using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                {
                    bitmap.Save(stream.AsStream(), ImageFormat.Jpeg);//choose the specific image format by your own bitmap source
                    Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                }
                
                






                
                if (rectangles.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Not detected");
                }
                else
                {
                    foreach (var rect in rectangles)
                    {
                        CvInvoke.PutText(image, rect.X.ToString() + "," + rect.Y.ToString(), new System.Drawing.Point(rect.X, rect.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255), 5);
                    }

                    imageBox.Image = image;
                    System.Windows.Forms.MessageBox.Show("Detected: " + rectangles.Count);
                }
                */
            }
        }
        /*
        private async Task<ImageSource> ProcessImageAsync(StorageFile ImageFile)
        {
            if (ImageFile == null)
                throw new ArgumentNullException("ImageFile cannot be null.");

            //The new size of processed image.
            const int side = 100;

            //Initialize bitmap transformations to be applied to the image.
            var transform = new BitmapTransform() { ScaledWidth = side, ScaledHeight = side, InterpolationMode = BitmapInterpolationMode.Cubic };

            //Get image pixels.
            var stream = await ImageFile.OpenStreamForReadAsync();
            var decoder = await System.Windows.Media.Imaging.BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
            var pixels = pixelData.DetachPixelData();

            //Initialize writable bitmap.
            var wBitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
            await wBitmap.SetSourceAsync(stream.AsRandomAccessStream());

            //Create a software bitmap from the writable bitmap's pixel buffer.
            var sBitmap = SoftwareBitmap.CreateCopyFromBuffer(wBitmap.PixelBuffer, BitmapPixelFormat.Bgra8, side, side, BitmapAlphaMode.Premultiplied);

            //Create software bitmap source.
            var sBitmapSource = new SoftwareBitmapSource();
            await sBitmapSource.SetBitmapAsync(sBitmap);

            return sBitmapSource;
        }

        private async Task<SoftwareBitmap> ResizeSoftwareBitmap(SoftwareBitmap bitmap)
        {
            const int side = 100;

            //Initialize bitmap transformations to be applied to the image.
            var transform = new BitmapTransform() { ScaledWidth = side, ScaledHeight = side, InterpolationMode = BitmapInterpolationMode.Cubic };

            //Get image pixels.
            var stream = await bitmap.OpenStreamForReadAsync();
            var decoder = await System.Windows.Media.Imaging.BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
            var pixels = pixelData.DetachPixelData();

            //Initialize writable bitmap.
            var wBitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
            await wBitmap.SetSourceAsync(stream.AsRandomAccessStream());

            //Create a software bitmap from the writable bitmap's pixel buffer.
            var sBitmap = SoftwareBitmap.CreateCopyFromBuffer(bitmap, BitmapPixelFormat.Bgra8, side, side, BitmapAlphaMode.Premultiplied);

            return sBitmap
        }
        */

        

        private void sendFrameONNX_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Image<Bgr, byte> image = new Image<Bgr, byte>(openFileDialog.FileName);
               var resultImage =  _predictionModel.StartProcessing(image.ToBitmap());
                imageBox.Image = resultImage.ToImage<Bgr, byte>();
            }
        }

        private Bitmap SoftwareBitmapToBitmap(SoftwareBitmap softwareBitmap)
        {
            byte[] byteArray;
            var buffer = CryptographicBuffer.GenerateRandom((uint)(softwareBitmap.PixelHeight * softwareBitmap.PixelWidth * 4 + 54));
            
            softwareBitmap.CopyToBuffer(buffer);
            
            CryptographicBuffer.CopyToByteArray(buffer, out byteArray);

            using (var ms = new MemoryStream(byteArray))
            {
                var image = System.Drawing.Image.FromStream(ms);
                return new Bitmap(image);
            }
        }

        // Need to check
        private SoftwareBitmap BitmapToSoftwareBitmap(Bitmap bitmap)
        {
            byte[] byteArray;
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                byteArray = ms.ToArray();
            }
            IBuffer buffer = CryptographicBuffer.CreateFromByteArray(byteArray);
            SoftwareBitmap softwareBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, bitmap.Width, bitmap.Height);
            softwareBitmap.CopyFromBuffer(buffer);
            return softwareBitmap;
        }

        // Need to be checked
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
        }
        /*
        private async Task CropAndDisplayInputImageAsync(VideoFrame inputVideoFrame)
        {
            BitmapBounds cropBounds = new BitmapBounds();
            uint h = 416;
            uint w = 416;
            var frameHeight = useDX ? inputVideoFrame.Direct3DSurface.Description.Height : inputVideoFrame.SoftwareBitmap.PixelHeight;
            var frameWidth = useDX ? inputVideoFrame.Direct3DSurface.Description.Width : inputVideoFrame.SoftwareBitmap.PixelWidth;

            var requiredAR = ((float)28 / 28);
            w = Math.Min((uint)(requiredAR * frameHeight), (uint)frameWidth);
            h = Math.Min((uint)(frameWidth / requiredAR), (uint)frameHeight);
            cropBounds.X = (uint)((frameWidth - w) / 2);
            cropBounds.Y = 0;
            cropBounds.Width = w;
            cropBounds.Height = h;

            cropped_vf = new VideoFrame(BitmapPixelFormat.Bgra8, 28, 28, BitmapAlphaMode.Ignore);

            await inputVideoFrame.CopyToAsync(cropped_vf, cropBounds, null);
        }
        */



        // Need to check
        async Task ParseWebCamFrame(Bitmap bitmap)
        {
            if (customVisionPredictionEngine == null)
                return;

            var frame = new ImageInputData { Image = bitmap };
            var filteredBoxes = DetectObjectsUsingModel(frame);

          
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                DrawOverlays(filteredBoxes, bitmap.Height, bitmap.Width);
            });
         
        }

        public List<BoundingBox> DetectObjectsUsingModel(ImageInputData imageInputData)
        {
            var labels = customVisionPredictionEngine?.Predict(imageInputData).PredictedLabels;
            var boundingBoxes = outputParser.ParseOutputs(labels, 0.1f);
            boundingBoxes = boundingBoxes.Where(bb => bb.Label == "ball").ToList();
            //var filteredBoxes = outputParser.FilterBoundingBoxes(boundingBoxes, 20, 0.5f);
            var filteredBoxes = boundingBoxes;
            return filteredBoxes;
        }

        // Need to check
        private void DrawOverlays(List<BoundingBox> filteredBoxes, double originalHeight, double originalWidth)
        {
            var image = globalBitmap.ToImage<Bgr, byte>();
            foreach (var box in filteredBoxes)
            {
                // process output boxes
                double x = Math.Max(box.Dimensions.X, 0);
                double y = Math.Max(box.Dimensions.Y, 0);
                double width = Math.Min(originalWidth - x, box.Dimensions.Width);
                double height = Math.Min(originalHeight - y, box.Dimensions.Height);

                // fit to current image size
                x = originalWidth * x / ImageSettings.imageWidth;
                y = originalHeight * y / ImageSettings.imageHeight;
                width = originalWidth * width / ImageSettings.imageWidth;
                height = originalHeight * height / ImageSettings.imageHeight;

                

                var objBox = new Rectangle
                {
                    X = (int)Math.Round(x),
                    Y = (int)Math.Round(y),
                    Width = (int)Math.Round(width),
                    Height = (int)Math.Round(height)
                };

                
                CvInvoke.Rectangle(image, objBox, new MCvScalar(255, 0, 0), 4);
            }
            imageBox.Image = image;
        }

        private void DetectMLTensorFlow_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Image<Bgr, byte> img;
                Bitmap image = new Bitmap(System.Drawing.Image.FromFile(openFileDialog.FileName));
                img = image.ToImage<Bgr, byte>();

                var objectDetectionStrategy = new TensorFlowMLBasedObjectDetectionStrategy(new List<string>() { "ball, player" }, 0.1f);
                var rectangles = objectDetectionStrategy.DetectAllObjects(image);

                if (rectangles.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Not detected");
                }
                else
                {
                    foreach (var rect in rectangles)
                    {
                        //CvInvoke.PutText(img, rect.X.ToString() + "," + rect.Y.ToString(), new System.Drawing.Point(rect.X, rect.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        CvInvoke.Rectangle(img, rect, new MCvScalar(0, 0, 255), 4);
                    }

                    imageBox.Image = img;
                    //System.Windows.Forms.MessageBox.Show("Detected: " + rectangles.Count);
                }
            }

        }

        private void DetectTensorFlowSharp_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var model = new TensorFlowSharpBasedObjectDetectionStrategy();
                model.DetectAllObjects(openFileDialog.FileName);
                /*
                Image<Bgr, byte> img;
                Bitmap image = new Bitmap(System.Drawing.Image.FromFile(openFileDialog.FileName));
                img = image.ToImage<Bgr, byte>();

                var objectDetectionStrategy = new TensorFlowMLBasedObjectDetectionStrategy(new List<string>() { "ball, player" }, 0.1f);
                var rectangles = objectDetectionStrategy.DetectAllObjects(image);

                if (rectangles.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Not detected");
                }
                else
                {
                    foreach (var rect in rectangles)
                    {
                        //CvInvoke.PutText(img, rect.X.ToString() + "," + rect.Y.ToString(), new System.Drawing.Point(rect.X, rect.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        CvInvoke.Rectangle(img, rect, new MCvScalar(0, 0, 255), 4);
                    }

                    imageBox.Image = img;
                    //System.Windows.Forms.MessageBox.Show("Detected: " + rectangles.Count);
                }
                */
            }
        }

        private void addMarkedPlayerONNX_CLick(object sender, RoutedEventArgs e)
        {
            if (_selectedROI != Rectangle.Empty)
            {
                double horizontalScale = GameAnalyzer.CurrentFrame.Width / imageBox.Width;
                double verticalScale = GameAnalyzer.CurrentFrame.Height / imageBox.Height;

                Rectangle rectangle = new Rectangle((int)(_selectedROI.X * horizontalScale),
                                                    (int)(_selectedROI.Y * verticalScale),
                                                    (int)(_selectedROI.Width * horizontalScale),
                                                    (int)(_selectedROI.Height * verticalScale));
                _playersTracker.AddTrackingObject(GameAnalyzer.CurrentFrame.Mat, rectangle);
                _selectedROI = Rectangle.Empty;
                imageBox.Invalidate();
            }
        }

        private void AddTrackingPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedROI != Rectangle.Empty)
            {
                double horizontalScale;
                double verticalScale;
                if(GameAnalyzer.CurrentFrame != null)
                {
                    horizontalScale = (double)GameAnalyzer.CurrentFrame.Width / (double)imageBox.Width;
                    verticalScale = (double)GameAnalyzer.CurrentFrame.Height / (double)imageBox.Height;
                }
                else
                {
                    horizontalScale = (double)firstFrame.Width / (double)imageBox.Width;
                    verticalScale = (double)firstFrame.Height / (double)imageBox.Height;
                }

                Rectangle rectangle = new Rectangle((int)(_selectedROI.X * horizontalScale),
                                                    (int)(_selectedROI.Y * verticalScale),
                                                    (int)(_selectedROI.Width * horizontalScale),
                                                    (int)(_selectedROI.Height * verticalScale));
                if(GameAnalyzer.CurrentFrame != null)
                {
                    _playersTracker.AddTrackingObject(GameAnalyzer.CurrentFrame.Mat, rectangle, objID++);
                }
                _playersTracker.AddTrackingObject(firstFrame, rectangle, objID++);
                _selectedROI = Rectangle.Empty;
                imageBox.Invalidate();
            }
        }

        private void Finish()
        {
            finished = true;
            sw.Stop();
            var elapsedMiliseconds = sw.ElapsedMilliseconds;

            List<string> results = new List<string>() { "Elapsed time: " + elapsedMiliseconds};

            var mot = (EmguCVTrackersBasedMOT)_playersTracker;
            results.Add("Processed frames: " + mot.FramesCount);
            results.Add("Detected all: " + mot.DetectedAllCount);
            results.Add("Detected any: " + mot.DetectedAnyCount);
            results.Add("\nDetected objects:\n");

            foreach(var key in mot.ObjectDetectionCounts.Keys)
            {
                results.Add(key + ": " + mot.ObjectDetectionCounts[key]);
            }

            File.WriteAllLines("results.txt", results);
            System.Windows.Forms.MessageBox.Show("Finished");
        }
    }
}
