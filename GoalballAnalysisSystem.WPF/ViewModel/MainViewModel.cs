using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public INavigator Navigator { get; set; }
        public IAuthenticator Authenticator { get; }

        public MainViewModel(INavigator navigator, IAuthenticator authenticator)
        {
            Navigator = navigator;
            Authenticator = authenticator;
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }
        /*
        public ICommand UpdateSelectedViewModelCommand { get; private set; }

        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MainViewModel()
        {
            SelectedViewModel = new LoginViewModel();
            UpdateSelectedViewModelCommand = new UpdateSelectedViewModelCommand(this);
            App.NavigationCommand = UpdateSelectedViewModelCommand;
        }
        */
    }
}
