using GoalballAnalysisSystem.WPF.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginViewModel VM { get; private set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LoginCommand(LoginViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            var user = parameter as User;
            if (user == null)
                return false;
            
            if (string.IsNullOrWhiteSpace(user.Email))
                return false;
            
            if (string.IsNullOrWhiteSpace(user.Password))
                return false;
            
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Login();
        }
    }
}
