using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
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
                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = new ProcessingViewModel();
                        break;
                    case ViewType.Games:
                        _navigator.CurrentViewModel = new GamesViewModel();
                        break;
                    case ViewType.Teams:
                        _navigator.CurrentViewModel = new TeamsViewModel();
                        break;
                    case ViewType.Players:
                        _navigator.CurrentViewModel = new PlayersViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}