using GoalballAnalysisSystem.WPF.ViewModel.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Navigators
{
    public class Renavigator : IRenavigator
    {
        private readonly INavigator _navigator;
        private readonly IGoalballAnalysisSystemViewModelFactory _viewModelFactory;

        public Renavigator(INavigator navigator, IGoalballAnalysisSystemViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public void Renavigate(ViewType viewType)
        {
            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
        }
    }
}
