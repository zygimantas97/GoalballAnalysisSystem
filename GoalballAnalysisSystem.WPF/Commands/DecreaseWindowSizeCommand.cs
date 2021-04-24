using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    class DecreaseWindowSizeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        ProcessingViewModel _calibrationViewModel;

        public DecreaseWindowSizeCommand(ProcessingViewModel calibrationViewModel)
        {
            _calibrationViewModel = calibrationViewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_calibrationViewModel.WindowHeight - 10 > 0 && _calibrationViewModel.WindowWidth - 10 > 0)
            {
                _calibrationViewModel.WindowHeight -= 10;
                _calibrationViewModel.WindowWidth -= 10;
            }
        }
    }
}
