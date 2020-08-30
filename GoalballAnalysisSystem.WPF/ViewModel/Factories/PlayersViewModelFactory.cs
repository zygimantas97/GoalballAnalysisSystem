using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public class PlayersViewModelFactory : IGoalballAnalysisSystemViewModelFactory<PlayersViewModel>
    {
        public PlayersViewModel CreateViewModel()
        {
            return new PlayersViewModel();
        }
    }
}
