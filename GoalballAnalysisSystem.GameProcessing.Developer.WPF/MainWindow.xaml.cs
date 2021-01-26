using GoalballAnalysisSystem.GameProcessing.BallTracker;
using GoalballAnalysisSystem.GameProcessing.PlayersTracker;
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

namespace GoalballAnalysisSystem.GameProcessing.Developer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameAnalyzer GameAnalyzer { get; private set; }
        private Image<Bgr, byte> selectedPart;

        Rectangle _selectedROI = Rectangle.Empty;
        System.Drawing.Point _startROI;
        bool _isSelecting = false;

        private EmguCVTrackersBasedMOT _playersTracker;

        public MainWindow()
        {
            InitializeComponent();
            Image<Bgr, byte> imageBoxBackground = new Image<Bgr, byte>(imageBox.Width, imageBox.Height, new Bgr(0, 0, 0));
            imageBox.Image = imageBoxBackground;
        }

        private void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();
            if(result == true)
            {
                var topLeft = new System.Drawing.Point(0, 0);
                var topRight = new System.Drawing.Point(0, 0);
                var bottomRight = new System.Drawing.Point(0, 0);
                var bottomLeft = new System.Drawing.Point(0, 0);

                //IBallTracker ballTracker = new CNNBasedBallTracker();

                IObjectDetectionStrategy ballTracker = new ColorBasedObjectDetectionStrategy();
                _playersTracker = new EmguCVTrackersBasedMOT();

                GameAnalyzer = new GameAnalyzer(openFileDialog.FileName,
                                                topLeft, topRight, bottomRight, bottomLeft,
                                                ballTracker, _playersTracker);

                GameAnalyzer.FrameChanged += GameAnalyzer_FrameChanged;
                VideoStackPanel.Visibility = Visibility.Visible;
                GameAnalyzer.Start();
            }
        }

        private void GameAnalyzer_FrameChanged(object sender, EventArgs e)
        {
            imageBox.Image = GameAnalyzer.CurrentFrame;
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

        private void AddTrackerButton_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
