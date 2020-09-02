using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Navigators
{
    public class ViewModelFactoryRenavigator<T> : IRenavigator where T : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IGoalballAnalysisSystemViewModelFactory<T> _viewModelFactory;

        public ViewModelFactoryRenavigator(INavigator navigator, IGoalballAnalysisSystemViewModelFactory<T> viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public void Renavigate()
        {
            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel();
        }
    }
}
