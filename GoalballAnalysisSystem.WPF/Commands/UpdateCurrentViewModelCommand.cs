using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly IGoalballAnalysisSystemViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, IGoalballAnalysisSystemViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}