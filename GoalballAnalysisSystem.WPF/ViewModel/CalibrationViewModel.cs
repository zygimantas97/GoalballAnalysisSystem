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
        public ICommand IncreaseWindowSizeCommand { get; set; }
        public ICommand DecreaseWindowSizeCommand { get; set; }

        private TeamsService _teamsService;
        private PlayersService _playersService;
        private TeamPlayersService _teamplayersService;

        SynchronizationContext uiContext;

        private ObservableCollection<TeamResponse> _listOfAvailableHomeTeams;
        public ObservableCollection<TeamResponse> ListOfAvailableHomeTeams
        {
            get { return _listOfAvailableHomeTeams; }
        }

        private ObservableCollection<PlayerResponse> _listOfAvailablePlayers;
        public ObservableCollection<PlayerResponse> ListOfAvailablePlayers
        {
            get { return _listOfAvailablePlayers; }
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
                RefreshPlayersList();
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
                if (!_canBeCreated)
                    CanBeVideoSelected = true;

            }
        }

        private bool _canBeVideoSelected;
        public bool CanBeVideoSelected
        {
            get
            {
                return _canBeVideoSelected;
            }
            set
            {
                _canBeVideoSelected = value;
                OnPropertyChanged(nameof(CanBeVideoSelected));
            }
        }

        private bool _videoIsSelected;
        public bool VideoIsSelected
        {
            get
            {
                return _videoIsSelected;
            }
            set
            {
                _videoIsSelected = value;
                OnPropertyChanged(nameof(VideoIsSelected));
            }
        }

        private bool _automaticCalibrationIsFinished;
        public bool AutomaticCalibrationIsFinished
        {
            get
            {
                return _automaticCalibrationIsFinished;
            }
            set
            {
                _automaticCalibrationIsFinished = value;
                OnPropertyChanged(nameof(AutomaticCalibrationIsFinished));
            }
        }

        private bool _calibrationSuccessful;
        public bool CalibrationSuccessful
        {
            get
            {
                return _calibrationSuccessful;
            }
            set
            {
                _calibrationSuccessful = value;
                OnPropertyChanged(nameof(CalibrationSuccessful));
            }
        }

        private bool _canBePlayerSelected;
        public bool CanBePlayerSelected
        {
            get
            {
                return _canBePlayerSelected;
            }
            set
            {
                _canBePlayerSelected = value;
                OnPropertyChanged(nameof(CanBePlayerSelected));
            }
        }

        private bool _canBeTrackingObjectsDeleted;
        public bool CanBeTrackingObjectsDeleted
        {
            get
            {
                return _canBeTrackingObjectsDeleted;
            }
            set
            {
                _canBeTrackingObjectsDeleted = value;
                OnPropertyChanged(nameof(CanBeTrackingObjectsDeleted));
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

        private int _windowWidth;
        public int WindowWidth
        {
            get
            {
                return _windowWidth;
            }
            set
            {
                _windowWidth = value;
                OnPropertyChanged(nameof(WindowWidth));
            }
        }

        private int _windowHeight;
        public int WindowHeight
        {
            get
            {
                return _windowHeight;
            }
            set
            {
                _windowHeight = value;
                OnPropertyChanged(nameof(WindowHeight));
            }
        }

        private string _videoStatusTitle;
        public string VideoStatusTitle
        {
            get
            {
                return _videoStatusTitle;
            }
            set
            {
                _videoStatusTitle = value;
                OnPropertyChanged(nameof(VideoStatusTitle));
            }
        }



        public CalibrationViewModel(GamesService gamesService, TeamsService teamService, PlayersService playersService, TeamPlayersService teamPlayersService)
        {
            _teamsService = teamService;
            _playersService = playersService;
            _teamplayersService = teamPlayersService;
            uiContext = SynchronizationContext.Current;

            _listOfAvailableHomeTeams = new ObservableCollection<TeamResponse>();
            _listOfAvailablePlayers = new ObservableCollection<PlayerResponse>();
            SelectedGame = new GameResponse();
            SelectedHomeTeam = new TeamResponse();
            SelectedGuestTeam = new TeamResponse();

            CreateNewGameCommand = new CreateGameCommand(this, gamesService);
            IncreaseWindowSizeCommand = new IncreaseWindowSizeCommand(this);
            DecreaseWindowSizeCommand = new DecreaseWindowSizeCommand(this);
            CanBeCreated = true;
            EditModeOff = false;
            CanBeVideoSelected = false;
            CanBePlayerSelected = false;
            AutomaticCalibrationIsFinished = false;
            CalibrationSuccessful = false;
            CanBeTrackingObjectsDeleted = false;

            VideoStatusTitle = "Video stream is not ready";

            WindowWidth = 480;
            WindowHeight = 270;

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

        private async void RefreshPlayersList()
        {
            uiContext.Send(x => _listOfAvailablePlayers.Clear(), null);
            if (SelectedGame != null)
            {

                if(SelectedGame.HomeTeamId != null)
                {
                    var teamPlayers = await _teamplayersService.GetTeamPlayersByTeamAsync((long)SelectedGame.HomeTeamId);

                    foreach (var teamPlayer in teamPlayers)
                    {
                        var player = await _playersService.GetPlayerAsync(teamPlayer.PlayerId);
                        uiContext.Send(x => _listOfAvailablePlayers.Add(player), null);
                    }
                }

                if (SelectedGame.GuestTeamId != null)
                {
                    var teamPlayers = await _teamplayersService.GetTeamPlayersByTeamAsync((long)SelectedGame.GuestTeamId);

                    foreach (var teamPlayer in teamPlayers)
                    {
                        var player = await _playersService.GetPlayerAsync(teamPlayer.PlayerId);
                        uiContext.Send(x => _listOfAvailablePlayers.Add(player), null);
                    }
                }
            }

        }

    }
}
