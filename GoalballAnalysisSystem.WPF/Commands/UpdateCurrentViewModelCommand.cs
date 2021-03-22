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
        private readonly IRenavigator _renavigator;

        public event EventHandler CanExecuteChanged;

        public UpdateCurrentViewModelCommand(IRenavigator renavigator)
        {
            _renavigator = renavigator;
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
                _renavigator.Renavigate(viewType);
            }
        }
    }
}