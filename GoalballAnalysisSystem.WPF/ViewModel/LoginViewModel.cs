﻿using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Model;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel.DatabaseServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; }

        public LoginViewModel(IAuthenticator authenticator, IRenavigator renavigator)
        {
            LoginCommand = new LoginCommand(this, authenticator, renavigator);
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(renavigator);
        }

    }
}
