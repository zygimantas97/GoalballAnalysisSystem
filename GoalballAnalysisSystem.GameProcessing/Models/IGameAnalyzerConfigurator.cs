using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Models
{
    public interface IGameAnalyzerConfigurator
    {
        bool CheckPointInZoneOfInterest(Point point);
    }
}
