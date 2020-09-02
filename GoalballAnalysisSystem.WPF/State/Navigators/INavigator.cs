using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.State.Navigators
{
    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
    }
}
