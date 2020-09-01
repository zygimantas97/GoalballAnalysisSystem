﻿using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    class LoginCommand : ICommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            bool success = await _authenticator.Login(_loginViewModel.Email, parameter.ToString());
        }
    }
}
