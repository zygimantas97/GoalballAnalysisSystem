using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    class DeleteObjectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ISelectableProperties _viewModel;
        public DeleteObjectCommand(ISelectableProperties viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.DeleteSelectedObject(parameter);
        }
    }
}
