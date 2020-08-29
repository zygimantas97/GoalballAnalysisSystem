using GoalballAnalysisSystem.WPF.Model;
using GoalballAnalysisSystem.WPF.View;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public RegistrationViewModel VM { get; private set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RegisterCommand(RegistrationViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            var user = parameter as User;
            if (user == null)
                return false;
            if (string.IsNullOrWhiteSpace(user.Name))
                return false;
            if (string.IsNullOrWhiteSpace(user.Surname))
                return false;
            if (string.IsNullOrWhiteSpace(user.Email))
                return false;
            if (string.IsNullOrWhiteSpace(user.Password))
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Register();
        }
    }
}
