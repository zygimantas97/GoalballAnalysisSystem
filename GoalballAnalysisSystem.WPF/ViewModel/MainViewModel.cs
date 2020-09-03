using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel.Factories;
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
        private readonly IGoalballAnalysisSystemViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;
        private readonly IAuthenticator _authenticator;

        public bool IsLoggedIn => _authenticator.IsLoggedIn;
        public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;

        public ICommand UpdateCurrentViewModelCommand { get; }

        public MainViewModel(INavigator navigator, IAuthenticator authenticator, IGoalballAnalysisSystemViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            _authenticator = authenticator;

            _navigator.StateChanged += Navigator_StateChanged;
            _authenticator.StateChanged += Authenticator_StateChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        private void Authenticator_StateChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
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
