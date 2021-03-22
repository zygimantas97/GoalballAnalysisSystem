using GoalballAnalysisSystem.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public interface IGoalballAnalysisSystemViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewType viewType);
    }
}
