﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel.Commands
{
    class UpdateSelectedViewModelCommand : ICommand
    {
        private MainViewModel _mainViewModel;

        public event EventHandler CanExecuteChanged;

        public UpdateSelectedViewModelCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string viewModelName = parameter as string;
            if (!string.IsNullOrEmpty(viewModelName))
            {
                /*
                if (viewModelName == "RegistrationViewModel")
                    _mainViewModel.SelectedViewModel = new RegistrationViewModel();
                if(viewModelName == "LoginViewModel")
                    _mainViewModel.SelectedViewModel = new LoginViewModel();
                if (viewModelName == "HomeViewModel")
                    _mainViewModel.SelectedViewModel = new HomeViewModel();
                if (viewModelName == "CalibrationViewModel")
                    _mainViewModel.SelectedViewModel = new CalibrationViewModel();
                if (viewModelName == "ScoutingMainViewModel")
                    _mainViewModel.SelectedViewModel = new ScoutingMainViewModel();
                */
            }
        }
    }
}
