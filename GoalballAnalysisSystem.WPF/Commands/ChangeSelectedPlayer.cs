using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    public class ChangeSelectedPlayer : ICommand
    {
        private readonly PlayersViewModel _playersViewModel;
        public event EventHandler CanExecuteChanged;

        public ChangeSelectedPlayer(PlayersViewModel playersViewModel)
        {
            _playersViewModel = playersViewModel;
        }

        public bool CanExecute(object parameter)
        {
           return  true;
        }

        public void Execute(object parameter)
        {
            var player = (Player)parameter;
            _playersViewModel.SelectPlayer(player.Id);
        }
    }
}
