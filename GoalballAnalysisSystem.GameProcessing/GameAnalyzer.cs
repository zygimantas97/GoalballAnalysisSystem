using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.BallTracker;
using GoalballAnalysisSystem.GameProcessing.PlayersTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private IBallTracker _ballTracker;
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

        public GameAnalyzer(string fileName,
                            Point topLeftCorner,
                            Point topRightCorner,
                            Point bottomRightCorner,
                            Point bottomLeftCorner,
                            IBallTracker ballTracker,
                            IMOT playersTracker)
        {
            _videoCapture = new VideoCapture(fileName);
            FPS = (int)_videoCapture.GetCaptureProperty(CapProp.Fps);
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
            _gameAnalyzerTask.Start();
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
            while (Status != GameAnalyzerStatus.Stopped)
            {
                if(Status == GameAnalyzerStatus.Active)
                {
                    _videoCapture.Read(_cameraFeed);
                    if(_cameraFeed != null)
                    {
                        Point ballPosition = _ballTracker.GetBallPosition(_cameraFeed);


                        
                        List<Rectangle> rois = _playersTracker.UpdateTrackingObjects(_cameraFeed);
                        
                        // Drawing bounding boxes of players
                        foreach (var roi in rois)
                        {
                            CvInvoke.Rectangle(_cameraFeed, roi, new MCvScalar(255, 0, 0), 3);
                        }
                        
                    
                        
                        

 
                        //Drawing of coordinates in frame
                        if(ballPosition.X != -1 && ballPosition.Y != -1)
                            CvInvoke.PutText(_cameraFeed, ballPosition.X.ToString() + "," + ballPosition.Y.ToString(), new Point(ballPosition.X, ballPosition.Y + 100), FontFace.HersheySimplex, 1, new MCvScalar(255, 0, 0), 2);

                        Rectangle rect = new Rectangle(ballPosition.X, ballPosition.Y, 30, 30);
                        CvInvoke.Rectangle(_cameraFeed, rect, new MCvScalar(0, 0, 255), 5);

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
