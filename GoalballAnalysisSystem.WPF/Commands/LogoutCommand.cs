using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    public class LogoutCommand : ICommand
    {
        private MainViewModel _mainVIewModel;
        private readonly IAuthenticator _authenticator;

        public event EventHandler CanExecuteChanged;

        public LogoutCommand(MainViewModel mainViewModel, IAuthenticator authenticator)
        {
            _mainVIewModel = mainViewModel;
            _authenticator = authenticator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _authenticator.Logout();
            _mainVIewModel.UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }
    }
}
