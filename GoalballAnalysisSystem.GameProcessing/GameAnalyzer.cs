using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.BallTracker;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection.ONNXModelBasedObjectDetection.MLBasedObjectDetection;
using GoalballAnalysisSystem.GameProcessing.PlayersTracker;
using GoalballAnalysisSystem.GameProcessing.PlayFieldTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.GameProcessing
{
    public enum GameAnalyzerStatus
    {
        NotStarted,
        Active,
        Paused,
        Stopped
    }

    public class GameAnalyzer
    {
        private VideoCapture _videoCapture;
        private Mat _cameraFeed = new Mat();
        private Task _gameAnalyzerTask;

        private Point _topLeftCorner;
        private Point _topRightCorner;
        private Point _bottomRightCorner;
        private Point _bottomLeftCorner;

        private IObjectDetectionStrategy _ballTracker;
        private IMOT _playersTracker;

        public GameAnalyzerStatus Status { get; private set; }

        public event EventHandler FrameChanged;

        private Image<Bgr, byte> _currentFrame;
        public Image<Bgr, byte> CurrentFrame
        {
            get { return _currentFrame; }
            private set
            {
                _currentFrame = value;
                if (FrameChanged != null)
                    FrameChanged(this, EventArgs.Empty);
            }
        }

        public int FPS { get; private set; }
        public int FrameCount { get; private set; }

        public GameAnalyzer(string fileName,
                            Point topLeftCorner,
                            Point topRightCorner,
                            Point bottomRightCorner,
                            Point bottomLeftCorner,
                            IObjectDetectionStrategy ballTracker,
                            IMOT playersTracker)
        {
            _videoCapture = new VideoCapture(fileName);
            FPS = (int)_videoCapture.GetCaptureProperty(CapProp.Fps);
            FrameCount = (int)_videoCapture.GetCaptureProperty(CapProp.FrameCount);
            _gameAnalyzerTask = new Task(GameAnalyzerTick);

            _topLeftCorner = topLeftCorner;
            _topRightCorner = topRightCorner;
            _bottomRightCorner = bottomRightCorner;
            _bottomLeftCorner = bottomLeftCorner;

            _ballTracker = ballTracker;
            _playersTracker = playersTracker;

        }

        public void Start()
        {
            Status = GameAnalyzerStatus.Active;
            //_gameAnalyzerTask.Start();
            GameAnalyzerTick();
        }

        public void Resume()
        {
            Status = GameAnalyzerStatus.Active;
        }

        public void Pause()
        {
            Status = GameAnalyzerStatus.Paused;
        }

        public void Stop()
        {
            Status = GameAnalyzerStatus.Stopped;
        }

        private void GameAnalyzerTick()
        {
            /*
            MLBasedObjectDetectionStrategy objectDetectionStrategy = new MLBasedObjectDetectionStrategy(new List<string>() { "player" }, 0.1f);
            int counter = 0;
            List<Rectangle> playersRectangles = new List<Rectangle>();
            */
            while (Status != GameAnalyzerStatus.Stopped)
            {
                if(Status == GameAnalyzerStatus.Active)
                {
                    _videoCapture.Read(_cameraFeed);
                    if(_cameraFeed != null)
                    {
                        //counter++;
                        /*
                        if(counter >= 3) //testavimui
                        {
                            playersRectangles = _playersTracker.UpdateTrackingObjects(_cameraFeed);
                            counter = 0;
                        }
                        */
                        // Drawing bounding boxes of players
                        /*
                        List<Rectangle> playersRectangles2 = objectDetectionStrategy.DetectAllObjects(_cameraFeed.ToBitmap());
                        foreach (var rectangle in playersRectangles2)
                        {
                            CvInvoke.Rectangle(_cameraFeed, rectangle, new MCvScalar(0, 0, 255), 3);
                        }
                        */
                        /*
                        foreach (var rectangle in playersRectangles)
                        {
                            CvInvoke.Rectangle(_cameraFeed, rectangle, new MCvScalar(255, 0, 0), 4);
                        }
                        */
                        // Rectangle ballRectangle = _ballTracker.DetectObject(_cameraFeed);

                        //Drawing of coordinates in frame
                        // if (ballRectangle != Rectangle.Empty)
                        // {
                        //     CvInvoke.PutText(_cameraFeed, ballRectangle.X.ToString() + "," + ballRectangle.Y.ToString(), new Point(ballRectangle.X, ballRectangle.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);
                        //      CvInvoke.Rectangle(_cameraFeed, ballRectangle, new MCvScalar(0, 0, 255), 5);
                        // }


                        //***********GameZoneCorders COLOR BASED drawing for testing purposes
                        // IPlayFieldTracker playFieldTracker = new ColorBasedPlayFieldTracker();
                        // var gameZoneCorners = playFieldTracker.GetPlayFieldCorners(_cameraFeed);
                        // for (int i = 0; i < gameZoneCorners.Length; i++)
                        // {
                        //     _cameraFeed = Drawing.EmguCVFiguresDrawing.DrawRectangle(_cameraFeed, gameZoneCorners[i], 25, 25);
                        // }
                        //***********GameZoneCorders COLOR BASED drawing for testing purposes


                        CurrentFrame = _cameraFeed.ToImage<Bgr, byte>();
                    }
                    else
                    {
                        Status = GameAnalyzerStatus.Stopped;
                    }
                }
            }   
        }
    }
}
