using System.Drawing;
using System.ComponentModel;
using System;
using System.Windows;
using Microsoft.Win32;
using Emgu.CV;
using GoalballAnalysisSystem.GameProcessing.Models;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.Color;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking.SOT;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.CustomVision;
using System.Collections.Generic;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.GameProcessing.Selector;
using System.Linq;

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isSelecting = false;
        private Rectangle _selectedROI = Rectangle.Empty;
        private System.Drawing.Point _selectionStart;

        private GameAnalyzer<TeamPlayerResponse> _gameAnalyzer;
        private IObjectDetector _objectDetector;
        private IMOT<TeamPlayerResponse> _mot;
        private ISelector<TeamPlayerResponse> _selector;
        private IGameAnalyzerConfigurator _gameAnalyzerConfigurator;
        private Mat _frame = new Mat();

        public event PropertyChangedEventHandler PropertyChanged;

        private const int PLAYGROUND_WIDTH = 900;
        private const int PLAYGROUND_HEIGHT = 1800;
        private readonly Image<Bgr, byte> _playgroundImageBoxBackground;

        public MainWindow()
        {
            InitializeComponent();
            Image<Bgr, byte> videoImageBoxBackground = new Image<Bgr, byte>(VideoImageBox.Width, VideoImageBox.Height, new Bgr(0, 0, 0));
            VideoImageBox.Image = videoImageBoxBackground;
            _playgroundImageBoxBackground = new Image<Bgr, byte>(PLAYGROUND_WIDTH, PLAYGROUND_HEIGHT, new Bgr(0, 0, 0));
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;
            DataContext = this;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartResumeButton_Click(object sender, RoutedEventArgs e)
        {
            _gameAnalyzer.Process();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _gameAnalyzer.Pause();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            _gameAnalyzer.Finish();
        }

        private void VideoImageBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_gameAnalyzer.Status == GameAnalyzerStatus.Paused)
            {
                _isSelecting = true;
                _selectionStart = e.Location;
            }
        }

        private void VideoImageBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_isSelecting)
            {
                int width = Math.Max(_selectionStart.X, e.X) - Math.Min(_selectionStart.X, e.X);
                int height = Math.Max(_selectionStart.Y, e.Y) - Math.Min(_selectionStart.Y, e.Y);
                _selectedROI = new Rectangle(Math.Min(_selectionStart.X, e.X), Math.Min(_selectionStart.Y, e.Y), width, height);
                VideoImageBox.Invalidate();
            }
        }

        private void VideoImageBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_isSelecting)
            {
                _isSelecting = false;
            }
        }

        private void VideoImageBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_isSelecting)
            {
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(pen, _selectedROI);
                }
            }
        }

        private async void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var capture = new VideoCapture(openFileDialog.FileName);
                capture.Read(_frame);
                VideoImageBox.Image = _frame;

                var playgroundDetector = new CustomVisionObjectDetector(new List<string>() { "TopLeft", "TopRight", "BottomLeft", "BottomRight" });
                var playgroundDetections = await playgroundDetector.Detect(_frame);
                var playgroundPoints = playgroundDetections
                    .SelectMany(c => c.Value)
                    .Select(rec => new System.Drawing.Point(rec.X + rec.Width / 2, rec.Y + rec.Height / 2))
                    .ToList();
                
                _gameAnalyzerConfigurator = GameAnalyzerConfigurator.Create(playgroundPoints, _frame.Width, _frame.Height, PLAYGROUND_WIDTH, PLAYGROUND_HEIGHT);

                if(_gameAnalyzerConfigurator != null)
                    DrawZoneOfInterest(_gameAnalyzerConfigurator);

                _objectDetector = new ColorObjectDetector("ball");
                _mot = new SOTBasedMOT<TeamPlayerResponse>();
                _selector = new ProjectionSelector<TeamPlayerResponse>(0, 1800, 100);

                _gameAnalyzer = new GameAnalyzer<TeamPlayerResponse>(openFileDialog.FileName,
                    _gameAnalyzerConfigurator, _objectDetector, _mot, _selector);

                _gameAnalyzer.FrameChanged += _gameAnalyzer_FrameChanged;
                _gameAnalyzer.ProcessingFinished += _gameAnalyzer_ProcessingFinished;
                _selector.Selected += _selector_Selected;
            }
        }

        private void DrawZoneOfInterest(IGameAnalyzerConfigurator configurator, int diameter = 10)
        {
            var topLeftRectangle = new Rectangle()
            {
                X = configurator.TopLeft.X - diameter / 2,
                Y = configurator.TopLeft.Y - diameter / 2,
                Width = diameter,
                Height = diameter
            };
            CvInvoke.Rectangle(_frame, topLeftRectangle, new MCvScalar(0, 0, 255), diameter);
            var topRightRectangle = new Rectangle()
            {
                X = configurator.TopRight.X - diameter / 2,
                Y = configurator.TopRight.Y - diameter / 2,
                Width = diameter,
                Height = diameter
            };
            CvInvoke.Rectangle(_frame, topRightRectangle, new MCvScalar(0, 0, 255), diameter);
            var bottomLeftRectangle = new Rectangle()
            {
                X = configurator.BottomLeft.X - diameter / 2,
                Y = configurator.BottomLeft.Y - diameter / 2,
                Width = diameter,
                Height = diameter
            };
            CvInvoke.Rectangle(_frame, bottomLeftRectangle, new MCvScalar(0, 0, 255), diameter);
            var bottomRightRectangle = new Rectangle()
            {
                X = configurator.BottomRight.X - diameter / 2,
                Y = configurator.BottomRight.Y -diameter / 2,
                Width = diameter,
                Height = diameter
            };
            CvInvoke.Rectangle(_frame, bottomRightRectangle, new MCvScalar(0, 0, 255), diameter);
            VideoImageBox.Image = _frame;
        }

        private void _selector_Selected(object sender, SelectionEventArgs<TeamPlayerResponse> e)
        {
            var playground = _playgroundImageBoxBackground.Clone();
            var rectangle = new Rectangle()
            {
                X = e.SelectionStart.X,
                Y = e.SelectionStart.Y,
                Width = 10,
                Height = 10
            };
            CvInvoke.Rectangle(playground, rectangle, new MCvScalar(255, 255, 255), 5);
            PlaygroundImageBox.Image = playground;
        }

        private void _gameAnalyzer_ProcessingFinished(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Process was finished");
        }

        private void _gameAnalyzer_FrameChanged(object sender, EventArgs e)
        {
            try
            {
                if (_gameAnalyzer.Status == GameAnalyzerStatus.Processing)
                {
                    var image = _gameAnalyzer.CurrentFrame;
                    if (image != null && image.Data != null)
                    {
                        VideoImageBox.Image = image;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void AddTrackingObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedROI != Rectangle.Empty)
            {
                double horizontalScale;
                double verticalScale;
                if (_gameAnalyzer.CurrentFrame != null)
                {
                    horizontalScale = (double)_gameAnalyzer.CurrentFrame.Width / (double)VideoImageBox.Width;
                    verticalScale = (double)_gameAnalyzer.CurrentFrame.Height / (double)VideoImageBox.Height;
                }
                else
                {
                    horizontalScale = (double)_frame.Width / (double)VideoImageBox.Width;
                    verticalScale = (double)_frame.Height / (double)VideoImageBox.Height;
                }

                Rectangle rectangle = new Rectangle((int)(_selectedROI.X * horizontalScale),
                                                    (int)(_selectedROI.Y * verticalScale),
                                                    (int)(_selectedROI.Width * horizontalScale),
                                                    (int)(_selectedROI.Height * verticalScale));
                if (_gameAnalyzer.CurrentFrame != null)
                {
                    _mot.Add(new TeamPlayerResponse(), _gameAnalyzer.CurrentFrame.Mat, rectangle);
                }
                else
                {
                    _mot.Add(new TeamPlayerResponse(), _frame, rectangle);
                }
                _selectedROI = Rectangle.Empty;
                VideoImageBox.Invalidate();
            }
        }

        private void RemoveAllTrackingObjectsButton_Click(object sender, RoutedEventArgs e)
        {
            _mot.RemoveAll();
        }

        private async void DetectPlayersApiButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var image = new Image<Bgr, byte>(openFileDialog.FileName);
                var objectDetector = new CustomVisionObjectDetector(new List<string>() { "TopLeft", "TopRight", "BottomLeft", "BottomRight" });
                var detectedCategories = await objectDetector.Detect(image.Mat);
                foreach(var key in detectedCategories.Keys)
                {
                    var rectangles = detectedCategories[key];
                    foreach(var rec in rectangles)
                    {
                        CvInvoke.Rectangle(image, rec, new MCvScalar(255, 0, 0), 3);
                    }
                }
                VideoImageBox.Image = image;
            }
        }

        private async void DetectPlayersOnnxButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var image = new Image<Bgr, byte>(openFileDialog.FileName);
                var objectDetector = new ONNXObjectDetector(new List<string>() { "player" });
                var detectedCategories = await objectDetector.Detect(image.Mat);
                foreach (var key in detectedCategories.Keys)
                {
                    var rectangles = detectedCategories[key];
                    foreach (var rec in rectangles)
                    {
                        CvInvoke.Rectangle(image, rec, new MCvScalar(255, 0, 0), 3);
                    }
                }
                VideoImageBox.Image = image;
            }
        }
    }
}
