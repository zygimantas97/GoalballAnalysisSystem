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

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isSelecting = false;
        private Rectangle _selectedROI = Rectangle.Empty;
        private System.Drawing.Point _selectionStart;

        private GameAnalyzer _gameAnalyzer;
        private IObjectDetector _objectDetector;
        private IMOT<CreateGamePlayerRequest> _mot;
        private IGameAnalyzerConfigurator _gameAnalyzerConfigurator;
        private Mat _frame = new Mat();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            Image<Bgr, byte> imageBoxBackground = new Image<Bgr, byte>(VideoImageBox.Width, VideoImageBox.Height, new Bgr(0, 0, 0));
            VideoImageBox.Image = imageBoxBackground;
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

        private void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var capture = new VideoCapture(openFileDialog.FileName);
                capture.Read(_frame);
                VideoImageBox.Image = _frame;

                _gameAnalyzerConfigurator = new GameAnalyzerConfigurator();
                _objectDetector = new ColorObjectDetector("ball");
                _mot = new SOTBasedMOT<CreateGamePlayerRequest>();

                _gameAnalyzer = new GameAnalyzer(openFileDialog.FileName,
                    _gameAnalyzerConfigurator, _objectDetector, _mot);

                _gameAnalyzer.FrameChanged += _gameAnalyzer_FrameChanged;
                _gameAnalyzer.ProcessingFinished += _gameAnalyzer_ProcessingFinished;
                _gameAnalyzer.ProjectionDetected += _gameAnalyzer_ProjectionDetected;
            }
        }

        private void _gameAnalyzer_ProjectionDetected(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Projection was detected");
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
                    _mot.Add(new CreateGamePlayerRequest(), _gameAnalyzer.CurrentFrame.Mat, rectangle);
                }
                else
                {
                    _mot.Add(new CreateGamePlayerRequest(), _frame, rectangle);
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
