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
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _renavigator;

        public event EventHandler CanExecuteChanged;

        public LogoutCommand(IAuthenticator authenticator, IRenavigator renavigator)
        {
            _authenticator = authenticator;
            _renavigator = renavigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _authenticator.Logout();
            _renavigator.Renavigate(ViewType.Login);
        }
    }
}
