using Emgu.CV;
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

    public class GameAnalyzer : INotifyPropertyChanged
    {
        private VideoCapture _videoCapture;
        private Mat _cameraFeed = new Mat();
        private Task _gameAnalyzerTask;

        private Point _topLeftCorner;
        private Point _topRightCorner;
        private Point _bottomRightCorner;
        private Point _bottomLeftCorner;

        private IBallTracker _ballTracker;
        private IPlayersTracker _playersTracker;

        public GameAnalyzerStatus Status { get; private set; } = GameAnalyzerStatus.NotStarted;

        private Image<Bgr, byte> _currentFrame;

        public Image<Bgr, byte> CurrentFrame
        {
            get { return _currentFrame; }
            private set
            {
                _currentFrame = value;
                OnPropertyChanged(nameof(CurrentFrame));
            }
        }

        public GameAnalyzer(string fileName,
                            Point topLeftCorner,
                            Point topRightCorner,
                            Point bottomRightCorner,
                            Point bottomLeftCorner,
                            IBallTracker ballTracker,
                            IPlayersTracker playersTracker)
        {
            _videoCapture = new VideoCapture(fileName);
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
                        List<Point> playersPositions = _playersTracker.GetPlayersPositions(_cameraFeed);
                        CurrentFrame = _cameraFeed.ToImage<Bgr, byte>();
                    }
                    else
                    {
                        Status = GameAnalyzerStatus.Stopped;
                    }
                }
            }   
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
