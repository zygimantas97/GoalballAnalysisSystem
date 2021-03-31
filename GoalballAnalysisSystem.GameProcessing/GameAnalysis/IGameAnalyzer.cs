using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace GoalballAnalysisSystem.GameProcessing.GameAnalysis
{
    public interface IGameAnalyzer<T> where T : class
    {
        Image<Bgr, byte> CurrentFrame { get; }
        int FPS { get; }
        int FrameCount { get; }
        GameAnalyzerStatus Status { get; }

        event EventHandler FrameChanged;
        event EventHandler ProcessingFinished;

        void Finish();
        void Pause();
        void Process();
    }
}