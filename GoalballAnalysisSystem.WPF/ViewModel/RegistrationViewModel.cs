using GoalballAnalysisSystem.WPF.Commands;
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
    public class RegistrationViewModel : BaseViewModel
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

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

        public ICommand RegisterCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; }
        public RegistrationViewModel(IAuthenticator authenticator, IRenavigator renavigator)
        {
            RegisterCommand = new RegisterCommand(this, authenticator, renavigator);
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(renavigator);
        }

    }
}
