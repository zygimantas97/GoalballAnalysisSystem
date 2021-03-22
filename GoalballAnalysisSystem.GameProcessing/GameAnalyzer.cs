using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using GoalballAnalysisSystem.GameProcessing.PlayFieldTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using GoalballAnalysisSystem.GameProcessing.ObjectDetection;
using GoalballAnalysisSystem.GameProcessing.ObjectTracking;
using GoalballAnalysisSystem.GameProcessing.Models;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;

namespace GoalballAnalysisSystem.GameProcessing
{
    public enum GameAnalyzerStatus
    {
        Paused,
        Processing,
        Finished
    }

    public class GameAnalyzer
    {
        private readonly VideoCapture _videoCapture;
        private readonly Mat _cameraFeed = new Mat();
        private readonly IGameAnalyzerConfigurator _gameZoneConfigurator;
        private readonly IObjectDetector _objectDetector;
        private readonly IMOT<CreateGamePlayerRequest> _mot;

        public GameAnalyzerStatus Status { get; private set; }
        public int FPS { get; private set; }
        public int FrameCount { get; private set; }

        public event EventHandler FrameChanged;
        public event EventHandler ProcessingFinished;
        public event EventHandler ProjectionDetected;

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

        public GameAnalyzer(string fileName,
                            IGameAnalyzerConfigurator gameAnalyzerConfigurator,
                            IObjectDetector objectDetector,
                            IMOT<CreateGamePlayerRequest> mot)
        {
            _videoCapture = new VideoCapture(fileName);
            FPS = (int)_videoCapture.GetCaptureProperty(CapProp.Fps);
            FrameCount = (int)_videoCapture.GetCaptureProperty(CapProp.FrameCount);

            _objectDetector = objectDetector;
            _mot = mot;
        }

        public void Process()
        {
            if(Status == GameAnalyzerStatus.Paused)
            {
                Status = GameAnalyzerStatus.Processing;
                ProcessVideoStream();
            }
        }

        public void Pause()
        {
            if(Status == GameAnalyzerStatus.Processing)
            {
                Status = GameAnalyzerStatus.Paused;
            }
        }

        public void Finish()
        {
            if(Status == GameAnalyzerStatus.Paused)
            {
                Status = GameAnalyzerStatus.Finished;
                ProcessingFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        private async void ProcessVideoStream()
        {
            while (Status == GameAnalyzerStatus.Processing)
            {
                _videoCapture.Read(_cameraFeed);
                if(_cameraFeed != null)
                {
                    var detectedCategories = await _objectDetector.Detect(_cameraFeed);
                    foreach (var key in detectedCategories.Keys)
                    {
                        var rectangles = detectedCategories[key];
                        foreach(var rec in rectangles)
                        {
                            CvInvoke.Rectangle(_cameraFeed, rec, new MCvScalar(255, 0, 0), 3);
                        }
                    }

                    var trackingObjects = await _mot.Update(_cameraFeed);
                    foreach (var obj in trackingObjects.Keys)
                    {
                        var rectangle = trackingObjects[obj];
                        CvInvoke.Rectangle(_cameraFeed, rectangle, new MCvScalar(255, 0, 0), 3);
                    }

                    CurrentFrame = _cameraFeed.ToImage<Bgr, byte>();
                    await Task.Delay(30);
                }
                else
                {
                    Pause();
                    Finish(); 
                }
            }
        }
    }
}
