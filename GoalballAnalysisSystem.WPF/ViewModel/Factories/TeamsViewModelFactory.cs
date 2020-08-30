using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public class TeamsViewModelFactory : IGoalballAnalysisSystemViewModelFactory<TeamsViewModel>
    {
        public TeamsViewModel CreateViewModel()
        {
            return new TeamsViewModel();
        }
    }
}
