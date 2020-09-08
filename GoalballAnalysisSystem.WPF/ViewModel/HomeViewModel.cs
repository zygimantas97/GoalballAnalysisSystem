using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class HomeViewModel: BaseViewModel
    {

        public ICommand UpdateCurrentViewModelCommand { get; }

        public HomeViewModel(IRenavigator renavigator)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(renavigator);
        }


    }
}
