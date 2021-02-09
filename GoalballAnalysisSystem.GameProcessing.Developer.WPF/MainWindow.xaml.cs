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

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public GameAnalyzer GameAnalyzer { get; private set; }
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

        private EmguCVTrackersBasedMOT _playersTracker;

        public event PropertyChangedEventHandler PropertyChanged;

        public  MainWindow()
        {
            InitializeComponent();
            Image<Bgr, byte> imageBoxBackground = new Image<Bgr, byte>(imageBox.Width, imageBox.Height, new Bgr(0, 0, 0));
            imageBox.Image = imageBoxBackground;
            DataContext = this;
            
        }

        private void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if(result == true)
            {
                IPlayFieldTracker playFieldTracker = new ColorBasedPlayFieldTracker();
                var videoCapture = new VideoCapture(openFileDialog.FileName);
                Mat firstFrame = new Mat();
                videoCapture.Read(firstFrame);

                var gameZoneCorners = playFieldTracker.GetPlayFieldCorners(firstFrame);

                //IBallTracker ballTracker = new CNNBasedBallTracker();

                IObjectDetectionStrategy ballTracker = new MLBasedObjectDetectionStrategy(new List<string> { "ball" });
                _playersTracker = new EmguCVTrackersBasedMOT();

                GameAnalyzer = new GameAnalyzer(openFileDialog.FileName,
                                                new System.Drawing.Point(0,0),
                                                new System.Drawing.Point(0, 0),
                                                new System.Drawing.Point(0, 0),
                                                new System.Drawing.Point(0, 0),
                                                ballTracker, _playersTracker);

                GameAnalyzer.FrameChanged += GameAnalyzer_FrameChanged;
                VideoStackPanel.Visibility = Visibility.Visible;
                GameAnalyzer.Start();
            }
        }

        private void GameAnalyzer_FrameChanged(object sender, EventArgs e)
        {
            try
            {
                var image = GameAnalyzer.CurrentFrame;
                if(image != null)
                {
                    imageBox.Image = image;
                    /*
                    if (_frameNo % 50 == 0)
                    {
                        image.Save("Output\\frame_" + (_frameNo + 1) + ".jpg");
                    }
                    */
                }
                _frameNo++;
                Progress = String.Format("{0:0.00} {1}", (double)_frameNo / (double)GameAnalyzer.FrameCount * 100, "%");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            GameAnalyzer.Resume();
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
                Image<Bgr, byte> image = new Image<Bgr, byte>(openFileDialog.FileName);
                var objectDetectionStrategy = new MLBasedObjectDetectionStrategy(new List<string>() { "ball" });
                var rectangles = objectDetectionStrategy.DetectAllObjects(image.Mat);
                if(rectangles.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Not detected");
                }
                else
                {
                    foreach(var rect in rectangles)
                    {
                        CvInvoke.PutText(image, rect.X.ToString() + "," + rect.Y.ToString(), new System.Drawing.Point(rect.X, rect.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255), 5);
                    }
                    
                    imageBox.Image = image;
                    System.Windows.Forms.MessageBox.Show("Detected: " + rectangles.Count);
                }
            }




        }

        private void imageBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(GameAnalyzer.Status == GameAnalyzerStatus.Paused)
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

        private async void sendFrame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                HttpResponseMessage response = await ObjectDetection.APIBasedObjectDetectionStrategy.CVSPrediction.MakePredictionRequest(openFileDialog.FileName);
                System.Windows.Forms.MessageBox.Show("JSON was obtained");
                Trace.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }

        private async void DetectObjectWinAIButton_Click(object sender, RoutedEventArgs e)
        {
            

            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                /*
                // Creating object detection strategy
                var objectDetectionStrategy = new WinAIBasedObjectDetectionStrategy(new List<string>() { "ball", "player" });
                await objectDetectionStrategy.Init();

                // Reading image
                Image<Bgra, byte> image = new Image<Bgra, byte>(openFileDialog.FileName);
                imageBox.Image = image;

                // Converting Bitmap -> VideoFrame
                byte[] imageByteArray = ImageToByte(image.ToBitmap());
                IBuffer buffer = CryptographicBuffer.CreateFromByteArray(imageByteArray);
                SoftwareBitmap softwareBitmap = new SoftwareBitmap(BitmapPixelFormat.Bgra8, image.Width, image.Height);
                softwareBitmap.CopyFromBuffer(buffer);
                VideoFrame inputImage = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);

                // Detecting objects
                var results = await objectDetectionStrategy.PredictImageAsync(inputImage);
                
                
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

        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            /*ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));*/
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }

        }
    }
}
