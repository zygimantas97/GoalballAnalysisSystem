using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class CalibrationViewModel : BaseViewModel
    {

        public ICommand CreateNewGameCommand { get; set; }

        private TeamsService _teamsService;

        SynchronizationContext uiContext;

        private ObservableCollection<TeamResponse> _listOfAvailableHomeTeams;
        public ObservableCollection<TeamResponse> ListOfAvailableHomeTeams
        {
            get { return _listOfAvailableHomeTeams; }
        }

        private GameResponse _selectedGame;
        public GameResponse SelectedGame
        {
            get
            {
                return _selectedGame;
            }
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        private TeamResponse _selectedHomeTeam;
        public TeamResponse SelectedHomeTeam
        {
            get
            {
                return _selectedHomeTeam;
            }
            set
            {
                _selectedHomeTeam = value;
                OnPropertyChanged(nameof(SelectedHomeTeam));
            }
        }

        private TeamResponse _selectedGuestTeam;
        public TeamResponse SelectedGuestTeam
        {
            get
            {
                return _selectedGuestTeam;
            }
            set
            {
                _selectedGuestTeam = value;
                OnPropertyChanged(nameof(SelectedGuestTeam));
            }
        }

        private bool _canBeCreated;
        public bool CanBeCreated
        {
            get
            {
                return _canBeCreated;
            }
            set
            {
                _canBeCreated = value;
                OnPropertyChanged(nameof(CanBeCreated));
            }
        }
        private bool _editModeOff;
        public bool EditModeOff
        {
            get
            {
                return _editModeOff;
            }
            set
            {
                _editModeOff = value;
                OnPropertyChanged(nameof(EditModeOff));
            }
        }

        public CalibrationViewModel(GamesService gamesService, TeamsService teamService)
        {
            _teamsService = teamService;
            uiContext = SynchronizationContext.Current;

            _listOfAvailableHomeTeams = new ObservableCollection<TeamResponse>();
            SelectedGame = new GameResponse();
            SelectedHomeTeam = new TeamResponse();
            SelectedGuestTeam = new TeamResponse();

            CreateNewGameCommand = new CreateGameCommand(this, gamesService);
            CanBeCreated = true;
            EditModeOff = false;

            RefreshTeamsList();
        }

        private async void RefreshTeamsList()
        {
            var teamsList = await _teamsService.GetTeamsAsync();

            uiContext.Send(x => _listOfAvailableHomeTeams.Clear(), null);

            foreach (var team in teamsList)
            {
                uiContext.Send(x => _listOfAvailableHomeTeams.Add(team), null);
            }

        }

    }
}
