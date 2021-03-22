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
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _surname;

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
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
