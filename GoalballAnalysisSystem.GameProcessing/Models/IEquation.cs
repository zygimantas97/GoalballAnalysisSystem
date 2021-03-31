namespace GoalballAnalysisSystem.GameProcessing.Models
{
    public interface IEquation
    {
        double GetX(double y);
        double GetY(double x);
    }
}