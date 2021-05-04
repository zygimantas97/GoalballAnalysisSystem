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
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
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
using GoalballAnalysisSystem.GameProcessing.ObjectTracking.Classification;
using System.Windows.Controls;
using GoalballAnalysisSystem.WPF.ViewModel;

namespace GoalballAnalysisSystem.WPF.View
{
    /// <summary>
    /// Interaction logic for CalibrationView.xaml
    /// </summary>
    public partial class ProcessingView : UserControl
    {
        private const int PLAYGROUND_WIDTH = 900;
        private const int PLAYGROUND_HEIGHT = 1800;
        private const int YZONEHEIGHT = 300;
        private const int SELECTION_ZONE_TOP = 300;
        private const int SELECTION_ZONE_BOTTOM = 1200;
        private const int MAX_SELECTION_DISTANCE = 200;
        private int[] xZones = { 0, 50, 275, 325, 575, 625, 850, 900 };

        private bool _isSelecting = false;
        private Rectangle _selectedROI = Rectangle.Empty;
        private System.Drawing.Point _selectionStart;
        OpenFileDialog openFileDialog;

        private IGameAnalyzer<TeamPlayerResponse> _gameAnalyzer;
        private IGameAnalyzerConfigurator _gameAnalyzerConfigurator;
        private IObjectDetector _objectDetector;
        private IMOT<TeamPlayerResponse> _mot;
        private ISelector<TeamPlayerResponse> _selector;
        private Mat _frame = new Mat();
        private double _analysedFrames;
        private ProcessingViewModel _dataContext;

        public event PropertyChangedEventHandler PropertyChanged;


        private readonly Image<Bgr, byte> _playgroundImageBoxBackground;

        private bool _manualCalibration = false;
        private List<System.Drawing.Point> _manualCalibrationPoints;


        public ProcessingView()
        {
            InitializeComponent();

            _playgroundImageBoxBackground = Playground();
            PlaygroundImageBox.Image = _playgroundImageBoxBackground;
            _analysedFrames = 0;
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartResumeButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.VideoStatusTitle = "Analysing video";
            _gameAnalyzer.Process();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.VideoStatusTitle = "Video analysis paused";
            _gameAnalyzer.Pause();

        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.VideoStatusTitle = "Video analysis finished";
            _gameAnalyzer.Finish();
        }

        private void VideoImageBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_manualCalibration && _manualCalibrationPoints.Count() < 4)
            {
                System.Drawing.Point coordinates = e.Location;

                double horizontalScale = (double)_frame.Width / (double)VideoImageBox.Width;
                double verticalScale = (double)_frame.Height / (double)VideoImageBox.Height;

                System.Drawing.Point scaledCoordinate = new System.Drawing.Point((int)((coordinates.X) * horizontalScale), (int)((coordinates.Y) * verticalScale));

                Rectangle rectangle = new Rectangle(scaledCoordinate.X-(int)(7* horizontalScale),
                                                    scaledCoordinate.Y- (int)(7 * horizontalScale),
                                                    (int)(14 * horizontalScale),
                                                    (int)(14 * verticalScale));

                _manualCalibrationPoints.Add(scaledCoordinate);
                CvInvoke.Rectangle(_frame, rectangle, new MCvScalar(200, 0, 0), 6);
                VideoImageBox.Image = _frame;

                if(_manualCalibrationPoints.Count == 4)
                {
                    _dataContext.CalibrationSuccessful = true;
                    _dataContext.CalibrationIsFinished = true;
                    _dataContext.VideoStatusTitle = "Calibration was successful!";
                    _gameAnalyzerConfigurator = GameAnalyzerConfigurator.Create(_manualCalibrationPoints, _frame.Width, _frame.Height, PLAYGROUND_WIDTH, PLAYGROUND_HEIGHT);
                    DrawSelectedZoneOfInterest(_gameAnalyzerConfigurator);
                }
            }
            else
            {
                if (_gameAnalyzer.Status == GameAnalyzerStatus.Paused)
                {
                    _dataContext.CanBePlayerSelected = false;

                    _isSelecting = true;
                    _selectionStart = e.Location;
                }
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
                _dataContext.CanBePlayerSelected = true;
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
            _dataContext = (ProcessingViewModel)this.DataContext;

            openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _dataContext.VideoIsSelected = true;
                _dataContext.CanBeVideoSelected = false;

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

                if (_gameAnalyzerConfigurator != null && AutoCalibrationWasSuccessful(_gameAnalyzerConfigurator))
                {
                    DrawSelectedZoneOfInterest(_gameAnalyzerConfigurator);
                    _dataContext.VideoStatusTitle = "Auto calibration was successful!";
                    _dataContext.CalibrationIsFinished = true;
                    _dataContext.CalibrationSuccessful = true;
                }
                else
                {
                    _dataContext.VideoStatusTitle = "Auto calibration is not available";
                    _dataContext.CalibrationIsFinished = true;
                    _dataContext.CalibrationSuccessful = false;
                }
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var capture = new VideoCapture(openFileDialog.FileName);
            capture.Read(_frame);
            VideoImageBox.Image = _frame;

            _manualCalibration = false;
            _dataContext.CalibrationSuccessful = false; //to disable next button
            _dataContext.VideoStatusTitle = "Mark players for tracking";
            _dataContext.CalibrationIsFinished = false;
            _dataContext.CanBeTrackingObjectsDeleted = true;
            _dataContext.CanVideoBePlayed = true;

            _objectDetector = new ColorObjectDetector("ball");
            _mot = new SOTBasedMOT<TeamPlayerResponse>();
            _selector = new ProjectionSelector<TeamPlayerResponse>(SELECTION_ZONE_TOP, SELECTION_ZONE_BOTTOM, MAX_SELECTION_DISTANCE);
            _gameAnalyzer = new GameAnalyzer<TeamPlayerResponse>(openFileDialog.FileName,
            _gameAnalyzerConfigurator, _objectDetector, _mot, _selector);

            _gameAnalyzer.FrameChanged += _gameAnalyzer_FrameChanged;
            _gameAnalyzer.ProcessingFinished += _gameAnalyzer_ProcessingFinished;
            _selector.Selected += _selector_Selected;
        }

        private async void ManualCalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CalibrationSuccessful = false;
            _dataContext.CalibrationIsFinished = false;

            _dataContext.VideoStatusTitle = "Select four playground corners";

            _manualCalibrationPoints = new List<System.Drawing.Point>();
            var capture = new VideoCapture(openFileDialog.FileName);
            capture.Read(_frame);
            VideoImageBox.Image = _frame;
            _manualCalibration = true;

        }

        private void DrawSelectedZoneOfInterest(IGameAnalyzerConfigurator configurator, int diameter = 10)
        {
            int Xlt = configurator.TopLeft.X - diameter / 2;
            int Ylt = configurator.TopLeft.Y - diameter / 2;

            int Xrt = configurator.TopRight.X - diameter / 2;
            int Yrt = configurator.TopRight.Y - diameter / 2;

            int Xlb = configurator.BottomLeft.X - diameter / 2;
            int Ylb = configurator.BottomLeft.Y - diameter / 2;

            int Xrb = configurator.BottomRight.X - diameter / 2;
            int Yrb = configurator.BottomRight.Y - diameter / 2;

            //top
            CvInvoke.Line(_frame, new System.Drawing.Point(Xlt, Ylt), new System.Drawing.Point(Xrt, Yrt), new MCvScalar(200, 0, 0), 4);

            //bottom
            CvInvoke.Line(_frame, new System.Drawing.Point(Xlb, Ylb), new System.Drawing.Point(Xrb, Yrb), new MCvScalar(200, 0, 0), 4);

            //left
            CvInvoke.Line(_frame, new System.Drawing.Point(Xlt, Ylt), new System.Drawing.Point(Xlb, Ylb), new MCvScalar(200, 0, 0), 4);

            //right
            CvInvoke.Line(_frame, new System.Drawing.Point(Xrt, Yrt), new System.Drawing.Point(Xrb, Yrb), new MCvScalar(200, 0, 0), 4);

            VideoImageBox.Image = _frame;
        }

        private bool AutoCalibrationWasSuccessful(IGameAnalyzerConfigurator gameAnalyzerConfigurator)
        {
            if (gameAnalyzerConfigurator.TopLeft.X != 0 && gameAnalyzerConfigurator.TopLeft.Y != 0 &&
               gameAnalyzerConfigurator.TopRight.X != 0 && gameAnalyzerConfigurator.TopRight.Y != 0 &&
               gameAnalyzerConfigurator.BottomLeft.X != 0 && gameAnalyzerConfigurator.BottomLeft.Y != 0 &&
               gameAnalyzerConfigurator.BottomRight.X != 0 && gameAnalyzerConfigurator.BottomRight.Y != 0)
                 return true;
            else return false;
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

            _dataContext.CreateProjection(e.SelectionStartObject, e.SelectionEndObject, (int)startX, (int)endX, (int)startY, (int)endY);
            CvInvoke.ArrowedLine(playground, startPoint, endPoint, new MCvScalar(0, 0, 0), 5, Emgu.CV.CvEnum.LineType.Filled, 0, 0.02);
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
                        _analysedFrames++;
                        VideoImageBox.Image = image;
                        _dataContext.FPS = _gameAnalyzer.FPS;
                        _dataContext.Progress = _analysedFrames/_gameAnalyzer.FrameCount*100;
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
                    _mot.Add(_dataContext.SelectedTeamPlayer, _gameAnalyzer.CurrentFrame.Mat, rectangle);
                }
                else
                {
                    _mot.Add(_dataContext.SelectedTeamPlayer, _frame, rectangle);
                }

                _dataContext.CanBePlayerSelected = false;
                _dataContext.CreateGamePlayer();

                _selectedROI = Rectangle.Empty;
                VideoImageBox.Invalidate();
            }
        }

        private void RemoveAllTrackingObjectsButton_Click(object sender, RoutedEventArgs e)
        {
            _mot.RemoveAll();
            var viewModel = (ProcessingViewModel)this.DataContext;
            viewModel.RefreshPlayersList();
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

        private Image<Bgr, byte> Playground()
        {
            int XPADDING = 0;
            int YPADDING = 0;
            Image<Bgr, byte> playgroundImageBoxBackground = new Image<Bgr, byte>(PLAYGROUND_WIDTH + 2 * XPADDING, PLAYGROUND_HEIGHT + 2 * YPADDING, new Bgr(255, 255, 255)); //two times from both sides

            //top
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, YPADDING), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, YPADDING), new MCvScalar(255, 160, 160), 10);

            //bottom
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, PLAYGROUND_HEIGHT + YPADDING), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 10);

            //left
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, YPADDING), new System.Drawing.Point(XPADDING, PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 10);

            //right
            CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, YPADDING), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 10);

            int zonesCount = PLAYGROUND_HEIGHT / YZONEHEIGHT;
            for (int i = 0; i < zonesCount; i++)
            {
                CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING, YPADDING + (i * YZONEHEIGHT)), new System.Drawing.Point(PLAYGROUND_WIDTH + XPADDING, YPADDING + (i * YZONEHEIGHT)), new MCvScalar(255, 160, 160), 4);
            }

            for (int i = 0; i < xZones.Length; i++)
            {
                //top
                CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING + xZones[i], YPADDING), new System.Drawing.Point(XPADDING + xZones[i], YPADDING + YZONEHEIGHT), new MCvScalar(255, 160, 160), 4);

                //bottom
                CvInvoke.Line(playgroundImageBoxBackground, new System.Drawing.Point(XPADDING + xZones[i], PLAYGROUND_HEIGHT + YPADDING - YZONEHEIGHT), new System.Drawing.Point(XPADDING + xZones[i], PLAYGROUND_HEIGHT + YPADDING), new MCvScalar(255, 160, 160), 4);
            }

            return playgroundImageBoxBackground;
        }

    }
}
