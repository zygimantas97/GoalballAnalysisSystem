using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    public class CreateGameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        
        private ProcessingViewModel _calibrationViewModel;
        private GamesService _gamesService;

        public CreateGameCommand(ProcessingViewModel gamesViewModel, GamesService gameService)
        {
            _calibrationViewModel = gamesViewModel;
            _gamesService = gameService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(_calibrationViewModel.SelectedGuestTeam.Id != null && _calibrationViewModel.SelectedHomeTeam.Id != null && _calibrationViewModel.SelectedGuestTeam.Id != _calibrationViewModel.SelectedHomeTeam.Id)
            {
                GameRequest gameToCreate = new GameRequest
                {
                    Title = _calibrationViewModel.SelectedGame.Title,
                    Comment = _calibrationViewModel.SelectedGame.Comment,
                    GuestTeamId = _calibrationViewModel.SelectedGuestTeam.Id,
                    HomeTeamId = _calibrationViewModel.SelectedHomeTeam.Id
                };

                var createdGame = await _gamesService.CreateGameAsync(gameToCreate);
                _calibrationViewModel.SelectedGame = createdGame; //sets id

                _calibrationViewModel.CanBeCreated = false;
                _calibrationViewModel.CanBeVideoSelected = true;
                _calibrationViewModel.EditModeOff = true;
            }
        }
    }
}
