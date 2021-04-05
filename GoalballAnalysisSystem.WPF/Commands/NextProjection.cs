﻿using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    class NextProjection : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private GamesViewModel _viewModel;

        public NextProjection(GamesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.NextProjection();
        }
    }
}