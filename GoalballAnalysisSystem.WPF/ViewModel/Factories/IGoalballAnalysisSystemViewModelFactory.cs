using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public interface IGoalballAnalysisSystemViewModelFactory<T> where T : BaseViewModel
    {
        T CreateViewModel();
    }
}
