using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    class CreateNewTeamPlayer : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private TeamsViewModel _teamsViewModel;
        private TeamPlayersService _teamPlayersService;
        public CreateNewTeamPlayer(TeamsViewModel teamsViewModel, TeamPlayersService teamPlayerService)
        {
            _teamsViewModel = teamsViewModel;
            _teamPlayersService = teamPlayerService;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(_teamsViewModel.SelectedComboBoxItem != null)
            {
                TeamPlayerRequest playerRequest = new TeamPlayerRequest { Number = 0};
                var result = await _teamPlayersService.CreateTeamPlayerAsync(_teamsViewModel.SelectedTeam.Id, _teamsViewModel.SelectedComboBoxItem.Id, playerRequest);
                _teamsViewModel.RefreshPlayersList();
                _teamsViewModel.RefreshAvailablePlayersList();
            }
        }
    }
}
