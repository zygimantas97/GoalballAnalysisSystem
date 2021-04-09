using System.Drawing;
using System.ComponentModel;
using System;
using System.Windows;
using Microsoft.Win32;
using Emgu.CV;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.Color;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking.SOT;
using GoalballAnalysisSystem.GameProcessing.Geometry.Equation;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.CustomVision;
using System.Collections.Generic;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNX;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System.Linq;
using GoalballAnalysisSystem.GameProcessing.Geometry;
using GoalballAnalysisSystem.GameProcessing.GameAnalysis;
using GoalballAnalysisSystem.GameProcessing.Selection;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking.CNN;

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isSelecting = false;
        private Rectangle _selectedROI = Rectangle.Empty;
        private System.Drawing.Point _selectionStart;

        private IGameAnalyzer<TeamPlayerResponse> _gameAnalyzer;
        private IGameAnalyzerConfigurator _gameAnalyzerConfigurator;
        private IObjectDetector _objectDetector;
        private IMOT<TeamPlayerResponse> _mot;
        private ISelector<TeamPlayerResponse> _selector;
        private Mat _frame = new Mat();

        public event PropertyChangedEventHandler PropertyChanged;

        private const int PLAYGROUND_WIDTH = 900;
        private const int PLAYGROUND_HEIGHT = 1800;
        private const int SELECTION_ZONE_TOP = 300;
        private const int SELECTION_ZONE_BOTTOM = 1200;
        private const int MAX_SELECTION_DISTANCE = 100;

        private readonly Image<Bgr, byte> _playgroundImageBoxBackground;

        public MainWindow()
        {
            InitializeComponent();
            //Image<Bgr, byte> videoImageBoxBackground = new Image<Bgr, byte>(VideoImageBox.Width, VideoImageBox.Height, new Bgr(0, 0, 0));
            //VideoImageBox.Image = videoImageBoxBackground;
            //_playgroundImageBoxBackground = new Image<Bgr, byte>(PLAYGROUND_WIDTH, PLAYGROUND_HEIGHT, new Bgr(0, 0, 0));
            //PlaygroundImageBox.Image = _playgroundImageBoxBackground;
            //DataContext = this;
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
                    .Select(rec => Calculations.GetMiddlePoint(rec))
                    .ToList();
                
                _gameAnalyzerConfigurator = GameAnalyzerConfigurator.Create(playgroundPoints, _frame.Width, _frame.Height, PLAYGROUND_WIDTH, PLAYGROUND_HEIGHT);

                if(_gameAnalyzerConfigurator != null)
                    DrawSelectedZoneOfInterest(_gameAnalyzerConfigurator);

                _objectDetector = new ColorObjectDetector("ball");
                _mot = new SOTBasedMOT<TeamPlayerResponse>();
                //_mot = new CNNBasedMOT<TeamPlayerResponse>(_objectDetector);
                _selector = new ProjectionSelector<TeamPlayerResponse>(SELECTION_ZONE_TOP, SELECTION_ZONE_BOTTOM, MAX_SELECTION_DISTANCE);

                _gameAnalyzer = new GameAnalyzer<TeamPlayerResponse>(openFileDialog.FileName,
                    _gameAnalyzerConfigurator, _objectDetector, _mot, _selector);

                _gameAnalyzer.FrameChanged += _gameAnalyzer_FrameChanged;
                _gameAnalyzer.ProcessingFinished += _gameAnalyzer_ProcessingFinished;
                _selector.Selected += _selector_Selected;
            }
        }

        private void DrawSelectedZoneOfInterest(IGameAnalyzerConfigurator configurator, int diameter = 10)
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

            double startY = e.SelectionStart.Y < e.SelectionEnd.Y ? 0 : PLAYGROUND_HEIGHT;
            double endY = e.SelectionStart.Y < e.SelectionEnd.Y ? PLAYGROUND_HEIGHT : 0;

            double startX = e.SelectionEquation.GetX(startY);
            double endX = e.SelectionEquation.GetX(endY);

            var startPoint = new System.Drawing.Point((int)Math.Round(startX), (int)Math.Round(startY));
            var endPoint = new System.Drawing.Point((int)Math.Round(endX), (int)Math.Round(endY));

            CvInvoke.Line(playground, startPoint, endPoint, new MCvScalar(255, 255, 255), 5);
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

        private async void DetectObjectsApiButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var image = new Image<Bgr, byte>(openFileDialog.FileName);
                var objectDetector = new CustomVisionObjectDetector(new List<string>() { "player" });
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

        private async void DetectObjectsOnnxButton_Click(object sender, RoutedEventArgs e)
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
