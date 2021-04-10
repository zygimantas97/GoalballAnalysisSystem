﻿using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    class CreateObjectCommand : ICommand
    {
        private readonly ISelectableProperties _viewModel;
        public event EventHandler CanExecuteChanged;

        public CreateObjectCommand(ISelectableProperties viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.CreateNewObject();
        }
    }
}