using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.ViewModel
{
    class BaseViewModel
    {
        
        public ICommand UpdateSelectedViewModelCommand { get; private set; }

        public BaseViewModel()
        {
            UpdateSelectedViewModelCommand = App.NavigationCommand;
        }
        
    }
}
