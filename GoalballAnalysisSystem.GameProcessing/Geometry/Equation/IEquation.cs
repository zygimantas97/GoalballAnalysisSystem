using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.GameProcessing.Geometry.Equation
{
    public interface IEquation
    {
        double GetX(double y);
        double GetY(double x);
    }
}
