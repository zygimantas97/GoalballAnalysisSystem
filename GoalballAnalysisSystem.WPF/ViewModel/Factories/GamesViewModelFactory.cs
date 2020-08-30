using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public class GamesViewModelFactory : IGoalballAnalysisSystemViewModelFactory<GamesViewModel>
    {
        public GamesViewModel CreateViewModel()
        {
            return new GamesViewModel();
        }
    }
}
