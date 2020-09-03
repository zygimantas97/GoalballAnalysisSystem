using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using GoalballAnalysisSystem.WPF.ViewModel.Factories;

namespace GoalballAnalysisSystem.WPF.State.Navigators
{
    public class Navigator : INavigator
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
