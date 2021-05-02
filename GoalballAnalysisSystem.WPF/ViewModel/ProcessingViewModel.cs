using GoalballAnalysisSystem.API.Contracts.V1.Requests;
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
    public class ProcessingViewModel : BaseViewModel
    {

        public ICommand CreateNewGameCommand { get; set; }
        public ICommand IncreaseWindowSizeCommand { get; set; }
        public ICommand DecreaseWindowSizeCommand { get; set; }

        private TeamsService _teamsService;
        private PlayersService _playersService;
        private TeamPlayersService _teamplayersService;
        private GamePlayersService _gamePlayersService;
        private ProjectionsService _projectionService;

        SynchronizationContext uiContext;

        private ObservableCollection<TeamResponse> _listOfAvailableHomeTeams;
        public ObservableCollection<TeamResponse> ListOfAvailableHomeTeams
        {
            get { return _listOfAvailableHomeTeams; }
        }

        private ObservableCollection<TeamPlayerResponse> _listOfAvailableTeamPlayers;
        public ObservableCollection<TeamPlayerResponse> ListOfAvailableTeamPlayers
        {
            get { return _listOfAvailableTeamPlayers; }
        }

        private List<GamePlayerResponse> _gamePlayers;

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

        private TeamPlayerResponse _selecteTeamPlayer;
        public TeamPlayerResponse SelectedTeamPlayer
        {
            get
            {
                return _selecteTeamPlayer;
            }
            set
            {
                _selecteTeamPlayer = value;
                OnPropertyChanged(nameof(SelectedTeamPlayer));
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

        private int _FPS;
        public int FPS
        {
            get
            {
                return _FPS;
            }
            set
            {
                _FPS = value;
                OnPropertyChanged(nameof(FPS));
            }
        }

        private double _progress;
        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = Math.Round((double)value, 0);
                OnPropertyChanged(nameof(Progress));
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

        private bool _calibrationIsFinished;
        public bool CalibrationIsFinished
        {
            get
            {
                return _calibrationIsFinished;
            }
            set
            {
                _calibrationIsFinished = value;
                OnPropertyChanged(nameof(CalibrationIsFinished));
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
        private bool _canVideoBePlayed;
        public bool CanVideoBePlayed
        {
            get
            {
                return _canVideoBePlayed;
            }
            set
            {
                _canVideoBePlayed = value;
                OnPropertyChanged(nameof(CanVideoBePlayed));
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

        public ProcessingViewModel(GamesService gamesService, GamePlayersService gamePlayersService, TeamsService teamService, PlayersService playersService, TeamPlayersService teamPlayersService, ProjectionsService projectionService)
        {
            _gamePlayersService = gamePlayersService;
            _teamsService = teamService;
            _playersService = playersService;
            _teamplayersService = teamPlayersService;
            _projectionService = projectionService;
            uiContext = SynchronizationContext.Current;

            _listOfAvailableHomeTeams = new ObservableCollection<TeamResponse>();
            _listOfAvailableTeamPlayers = new ObservableCollection<TeamPlayerResponse>();
            _gamePlayers = new List<GamePlayerResponse>();
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
            CalibrationIsFinished = false;
            CalibrationSuccessful = false;
            CanBeTrackingObjectsDeleted = false;
            CanVideoBePlayed = false;

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
            uiContext.Send(x => _listOfAvailableTeamPlayers.Clear(), null);
            if (SelectedGame != null)
            {

                if(SelectedGame.HomeTeamId != null)
                {
                    var teamPlayers = await _teamplayersService.GetTeamPlayersByTeamAsync((long)SelectedGame.HomeTeamId);

                    foreach (var teamPlayer in teamPlayers)
                    {
                        uiContext.Send(x => _listOfAvailableTeamPlayers.Add(teamPlayer), null);
                    }
                }

                if (SelectedGame.GuestTeamId != null)
                {
                    var teamPlayers = await _teamplayersService.GetTeamPlayersByTeamAsync((long)SelectedGame.GuestTeamId);

                    foreach (var teamPlayer in teamPlayers)
                    {
                        uiContext.Send(x => _listOfAvailableTeamPlayers.Add(teamPlayer), null);
                    }
                }
            }

        }
        public async void CreateGamePlayer()
        {
            var gamePlayer = new CreateGamePlayerRequest
            {
                TeamId = SelectedTeamPlayer.TeamId,
                PlayerId = SelectedTeamPlayer.PlayerId,
                GameId = SelectedGame.Id
            };
            

            var createdGamePlayer = await _gamePlayersService.CreateGamePlayerAsync(gamePlayer);
            _gamePlayers.Add(createdGamePlayer);

        }

        public async void CreateProjection(TeamPlayerResponse offensive, TeamPlayerResponse defensive, int x1, int x2, int y1, int y2)
        {
            GamePlayerResponse offensivePlayer = null;
            GamePlayerResponse defensivePlayer = null;

            //find created game player
            foreach (var gamePlayer in _gamePlayers)
            {
                if (offensive != null)
                {
                    if (gamePlayer.PlayerId == offensive.PlayerId)
                        offensivePlayer = gamePlayer;
                }

                if (defensive != null)
                {
                    if (gamePlayer.PlayerId == defensive.PlayerId)
                        defensivePlayer = gamePlayer;
                }

            }

            var projection = new ProjectionRequest
            {
                GameId = SelectedGame.Id,
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2
            };

            if (offensivePlayer != null)
                projection.OffenseGamePlayerId = offensivePlayer.Id;
            if (defensivePlayer != null)
                projection.DefenseGamePlayerId = defensivePlayer.Id;

            var createProjection = await _projectionService.CreateProjectionAsync(projection);

        }

    }
}
