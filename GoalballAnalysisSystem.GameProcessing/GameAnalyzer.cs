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
using GoalballAnalysisSystem.GameProcessing.Selector;
using System.Linq;

namespace GoalballAnalysisSystem.GameProcessing
{
    public enum GameAnalyzerStatus
    {
        Paused,
        Processing,
        Finished
    }

    public class GameAnalyzer<T> where T : class
    {
        private readonly VideoCapture _videoCapture;
        private readonly Mat _cameraFeed = new Mat();
        private readonly IGameAnalyzerConfigurator _gameAnalyzerConfigurator;
        private readonly IObjectDetector _objectDetector;
        private readonly IMOT<T> _mot;
        private readonly ISelector<T> _selector;

        public GameAnalyzerStatus Status { get; private set; }
        public int FPS { get; private set; }
        public int FrameCount { get; private set; }

        public event EventHandler FrameChanged;
        public event EventHandler ProcessingFinished;

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
                            IMOT<T> mot,
                            ISelector<T> selector)
        {
            _videoCapture = new VideoCapture(fileName);
            FPS = (int)_videoCapture.GetCaptureProperty(CapProp.Fps);
            FrameCount = (int)_videoCapture.GetCaptureProperty(CapProp.FrameCount);

            _gameAnalyzerConfigurator = gameAnalyzerConfigurator;
            _objectDetector = objectDetector;
            _mot = mot;
            _selector = selector;
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
                        var category = detectedCategories[key];
                        foreach(var rec in category)
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

                    var locations = detectedCategories
                        .SelectMany(c => c.Value)
                        .Select(rec => Geometry.GetMiddlePoint(rec))
                        .Where(p => _gameAnalyzerConfigurator.IsPointInZoneOfInterest(p));

                    var playgroundObjects = trackingObjects
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => _gameAnalyzerConfigurator.GetPlaygroundOXY(Geometry.GetMiddlePoint(kvp.Value)));
                    foreach (var loc in locations)
                    {
                        var playgroundLocation = _gameAnalyzerConfigurator.GetPlaygroundOXY(loc);
                        _selector.AddPoint(playgroundLocation, playgroundObjects);
                    }
                        
                    CurrentFrame = _cameraFeed.ToImage<Bgr, byte>();
                    await Task.Delay(1000/FPS);
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
