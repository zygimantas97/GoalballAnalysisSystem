using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;


namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class GamesViewModel : BaseViewModel, ISelectableProperties
    {
        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand ChangeSelectedObjectCommand { get; }
        public ICommand EditSelectedObjectCommand { get; }
        public ICommand DeleteSelectedObjectCommand { get; }
        public ICommand PreviousProjectionCommand { get; }
        public ICommand CreateNewObjectCommand { get; }
        public ICommand NextProjectionCommand { get; }
        #endregion

        #region Definitions

        private SynchronizationContext _uiContext;
        private GamesService _gamesService;
        private ProjectionsService _projectionsService;
        private GamePlayersService _gamePlayersService;
        private TeamsService _teamsService;
        private TeamPlayersService _teamPlayersService;
        private PlayersService _playersService;

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
                RefreshProjectionsList();
                RefreshPlayersList();

                if (value != null)
                {
                    CanBeEditedGame = true;
                    CanBeDeletedGame = true;
                }
                else
                {
                    CanBeEditedGame = false;
                    CanBeDeletedGame = false;
                }
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

        private TeamResponse _selecteGuestTeam;
        public TeamResponse SelectedGuestTeam
        {
            get
            {
                return _selecteGuestTeam;
            }
            set
            {
                _selecteGuestTeam = value;
                OnPropertyChanged(nameof(SelectedGuestTeam));
            }
        }

        private Rectangle _selectedGameZone;
        public Rectangle SelectedGameZone
        {
            get
            {
                return _selectedGameZone;
            }
            set
            {
                _selectedGameZone = value;
                OnPropertyChanged(nameof(SelectedGameZone));

            }
        }

        private ProjectionResponse _selectedProjection;
        public ProjectionResponse SelectedProjection
        {
            get
            {
                return _selectedProjection;
            }
            set
            {
                _selectedProjection = value;
                OnPropertyChanged(nameof(SelectedProjection));
                RefreshProjectionPlayers();

            }
        }

        private PlayerResponse _topPlayer;
        public PlayerResponse TopPlayer
        {
            get
            {
                return _topPlayer;
            }
            set
            {
                _topPlayer = value;
                OnPropertyChanged(nameof(TopPlayer));

            }
        }

        private PlayerResponse _bottomPlayer;
        public PlayerResponse BottomPlayer
        {
            get
            {
                return _bottomPlayer;
            }
            set
            {
                _bottomPlayer = value;
                OnPropertyChanged(nameof(BottomPlayer));

            }
        }

        private int _currentProjectionIndex;

        private readonly ObservableCollection<GameResponse> _listOfGames;
        public ObservableCollection<GameResponse> ListOfGames
        {
            get { return _listOfGames; }
        }

        private ObservableCollection<ProjectionResponse> _listOfProjections;
        public ObservableCollection<ProjectionResponse> ListOfProjections
        {
            get { return _listOfProjections; }
        }

        private ObservableCollection<TeamPlayerResponse> _listOfHomeTeamPlayers;
        public ObservableCollection<TeamPlayerResponse> ListOfHomeTeamPlayers
        {
            get { return _listOfHomeTeamPlayers; }
        }

        private ObservableCollection<TeamPlayerResponse> _listOfGuestTeamPlayers;
        public ObservableCollection<TeamPlayerResponse> ListOfGuestTeamPlayers
        {
            get { return _listOfGuestTeamPlayers; }
        }

        private ObservableCollection<PlayerResponse> _listOfHomePlayers;
        public ObservableCollection<PlayerResponse> ListOfHomePlayers
        {
            get { return _listOfHomePlayers; }
        }

        private ObservableCollection<PlayerResponse> _listOfGuestPlayers;
        public ObservableCollection<PlayerResponse> ListOfGuestPlayers
        {
            get { return _listOfGuestPlayers; }
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

        private bool _canBeEditedGame;
        public bool CanBeEditedGame
        {
            get
            {
                return _canBeEditedGame;
            }
            set
            {
                _canBeEditedGame = value;
                OnPropertyChanged(nameof(CanBeEditedGame));
            }
        }

        private bool _canBeDeletedGame;
        public bool CanBeDeletedGame
        {
            get
            {
                return _canBeDeletedGame;
            }
            set
            {
                _canBeDeletedGame = value;
                OnPropertyChanged(nameof(CanBeDeletedGame));
            }
        }
        #endregion

        public GamesViewModel(GamesService gameService, ProjectionsService projectionService, GamePlayersService gamePlayersService, TeamsService teamsService, TeamPlayersService teamPlayersService, PlayersService playersService)
        {
            _uiContext = SynchronizationContext.Current;
            _gamesService = gameService;
            _projectionsService = projectionService;
            _gamePlayersService = gamePlayersService;
            _teamsService = teamsService;
            _teamPlayersService = teamPlayersService;
            _playersService = playersService;


            _listOfGames = new ObservableCollection<GameResponse>();
            _listOfProjections = new ObservableCollection<ProjectionResponse>();
            _listOfHomeTeamPlayers = new ObservableCollection<TeamPlayerResponse>();
            _listOfGuestTeamPlayers = new ObservableCollection<TeamPlayerResponse>();
            _listOfHomePlayers = new ObservableCollection<PlayerResponse>();
            _listOfGuestPlayers = new ObservableCollection<PlayerResponse>();

            SelectedGameZone = new Rectangle();

            CreateNewObjectCommand = new CreateObjectCommand(this);
            ChangeSelectedObjectCommand = new SelectObjectCommand(this);
            DeleteSelectedObjectCommand = new DeleteObjectCommand(this);
            EditSelectedObjectCommand = new TurnEditModeCommand(this);
            // PreviousProjectionCommand = new PreviousProjection(this);
            // NextProjectionCommand = new NextProjection(this);

            EditModeOff = true;
            _currentProjectionIndex = -1;
            RefreshGameList();

            EditModeOff = true;
            CanBeDeletedGame = false;
            CanBeEditedGame = false;
        }

        public async void ChangeEditMode(object parameter)
        {
            EditModeOff = !EditModeOff;
            CanBeDeletedGame = EditModeOff;

            if (EditModeOff && SelectedGame != null) //edit has been finished
            {
                GameRequest gameToEdit = new GameRequest
                {
                    Title = SelectedGame.Title,
                    Comment = SelectedGame.Comment,
                    GuestTeamId = SelectedGame.GuestTeamId,
                    HomeTeamId = SelectedGame.HomeTeamId
                };

                await _gamesService.UpdateGameAsync(SelectedGame.Id, gameToEdit);
            }
        }

        public void ChangeSelectedObject(object parameter)
        {
            if (parameter is GameResponse)
                SelectedGame = (GameResponse)parameter;
        }

        public async void DeleteSelectedObject(object parameter)
        {
            if (parameter is GameResponse)
            {
                var success = await _gamesService.DeleteGameAsync(SelectedGame.Id);
                if (success != null)
                {
                    SelectedGame = null;
                    Task.Run(() => this.RefreshGameList()).Wait();

                }
            }
        }

        public ProjectionResponse FirstProjection()
        {
            if (ListOfProjections.Count > 0)
            {
                _currentProjectionIndex = 0;
                SelectedProjection = ListOfProjections[_currentProjectionIndex];
                return SelectedProjection;
            }

            return null;
        }

        public ProjectionResponse NextProjection()
        {
            if (_currentProjectionIndex < ListOfProjections.Count - 1 && ListOfProjections.Count > 0)
            {
                _currentProjectionIndex++;
                SelectedProjection = ListOfProjections[_currentProjectionIndex];
                return SelectedProjection;
            }

            return null;
        }
        public ProjectionResponse PreviousProjection()
        {
            if (_currentProjectionIndex > 0 && ListOfProjections.Count > 0)
            {
                _currentProjectionIndex--;
                SelectedProjection = ListOfProjections[_currentProjectionIndex];
                return SelectedProjection;
            }

            return null;
        }

        private async void RefreshProjectionPlayers()
        {
            if (SelectedProjection != null)
            {
                GamePlayerResponse topPlayer = null;
                GamePlayerResponse bottomPlayer = null;
                TopPlayer = null;
                BottomPlayer = null;

                if (SelectedProjection.Y1 > 900)
                {
                    if(SelectedProjection.DefenseGamePlayerId != null)
                        topPlayer = await _gamePlayersService.GetGamePlayerAsync(Convert.ToInt64(SelectedProjection.DefenseGamePlayerId));
                    if (SelectedProjection.OffenseGamePlayerId != null)
                        bottomPlayer = await _gamePlayersService.GetGamePlayerAsync(Convert.ToInt64(SelectedProjection.OffenseGamePlayerId));
                }
                else
                {
                    if (SelectedProjection.DefenseGamePlayerId != null)
                        bottomPlayer = await _gamePlayersService.GetGamePlayerAsync(Convert.ToInt64(SelectedProjection.DefenseGamePlayerId));
                    if (SelectedProjection.OffenseGamePlayerId != null)
                        topPlayer = await _gamePlayersService.GetGamePlayerAsync(Convert.ToInt64(SelectedProjection.OffenseGamePlayerId));
                }

                if (topPlayer != null)
                    TopPlayer = topPlayer.TeamPlayer.Player;
                if (bottomPlayer != null)
                    BottomPlayer = bottomPlayer.TeamPlayer.Player;
            }
        }

        public async void RefreshGameList()
        {
            var gamesList = await _gamesService.GetGamesAsync();

            _uiContext.Send(x => _listOfGames.Clear(), null);

            foreach (var game in gamesList)
            {
                _uiContext.Send(x => _listOfGames.Add(game), null);
            }
        }

        public async void RefreshProjectionsList()
        {
            _uiContext.Send(x => _listOfProjections.Clear(), null);

            if (SelectedGame != null)
            {
                var projectionsList = await _projectionsService.GetProjectionsByGameAsync(SelectedGame.Id);

                if (SelectedGameZone == null || SelectedGameZone == new Rectangle())
                {

                    foreach (var projection in projectionsList)
                    {
                        _uiContext.Send(x => _listOfProjections.Add(projection), null);
                    }
                }
                else
                {
                    foreach (var projection in projectionsList)
                    {
                        if ((projection.X1 >= SelectedGameZone.X && projection.X1 <= SelectedGameZone.X+SelectedGameZone.Width && projection.Y1 >= SelectedGameZone.Y && projection.Y1 <= SelectedGameZone.Y + SelectedGameZone.Height) || (projection.X2 >= SelectedGameZone.X && projection.X2 <= SelectedGameZone.X + SelectedGameZone.Width && projection.Y2 >= SelectedGameZone.Y && projection.Y2 <= SelectedGameZone.Y + SelectedGameZone.Height))
                        {
                            _uiContext.Send(x => _listOfProjections.Add(projection), null);
                        }
                    }
                }
                _currentProjectionIndex = -1;
                SelectedProjection = null;
            }
        }

        public async void RefreshPlayersList()
        {
            _uiContext.Send(x => _listOfHomePlayers.Clear(), null);
            _uiContext.Send(x => _listOfGuestPlayers.Clear(), null);

            if (SelectedGame != null)
            {
                var gamePlayersList = await _gamePlayersService.GetGamePlayersByGameAsync(SelectedGame.Id);
                var homeTeamId = SelectedGame.HomeTeamId;
                var guestTeamId = SelectedGame.GuestTeamId;

                foreach (var gamePlayer in gamePlayersList)
                {
                    var player = gamePlayer.TeamPlayer.Player;
                    var teamId = gamePlayer.TeamPlayer.TeamId;
                    if (teamId == homeTeamId)
                    {
                        _listOfHomePlayers.Add(player);
                    }
                    else if (teamId == guestTeamId)
                    {
                        _listOfGuestPlayers.Add(player);
                    }
                }
            }
        }

        public async void CreateNewObject()
        {
            EditModeOff = !EditModeOff;
            Random rnd = new Random();

            if (!EditModeOff)
            {
                SelectedGame = new GameResponse();
            }

            if (EditModeOff) //edit has been finished
            {

                var allTeams = await _teamsService.GetTeamsAsync(); //butina tureti dvi sukurtas komandas
                var homeTeam = allTeams[allTeams.Count - 2];
                var guestTeam = allTeams[allTeams.Count - 1];

                var newGame = new GameRequest
                {
                    Title = SelectedGame.Title,
                    HomeTeamId = homeTeam.Id,
                    GuestTeamId = guestTeam.Id
                };

                var createdGame = await _gamesService.CreateGameAsync(newGame);

                var homeTeamPlayers = await _teamPlayersService.GetTeamPlayersByTeamAsync(homeTeam.Id);
                var guestTeamPlayers = await _teamPlayersService.GetTeamPlayersByTeamAsync(guestTeam.Id);

                var homeGamePlayers = new List<GamePlayerResponse>();
                var guestGamePlayers = new List<GamePlayerResponse>();

                foreach (var teamPlayer in homeTeamPlayers)
                {
                    var gamePlayerRequest = new CreateGamePlayerRequest
                    {
                        GameId = createdGame.Id,
                        TeamId = teamPlayer.TeamId,
                        PlayerId = teamPlayer.PlayerId
                    };

                    var gamePlayer = await _gamePlayersService.CreateGamePlayerAsync(gamePlayerRequest);
                    homeGamePlayers.Add(gamePlayer);
                }

                foreach (var teamPlayer in guestTeamPlayers)
                {
                    var gamePlayerRequest = new CreateGamePlayerRequest
                    {
                        GameId = createdGame.Id,
                        TeamId = teamPlayer.TeamId,
                        PlayerId = teamPlayer.PlayerId
                    };

                    var gamePlayer = await _gamePlayersService.CreateGamePlayerAsync(gamePlayerRequest);
                    guestGamePlayers.Add(gamePlayer);
                }

                for (int i = 0; i < Math.Min(homeGamePlayers.Count, guestGamePlayers.Count); i++)
                {
                    var newProjection = new ProjectionRequest
                    {
                        X1 = rnd.Next(1, 900),
                        X2 = rnd.Next(1, 900),
                        Y1 = rnd.Next(1, 1800),
                        Y2 = rnd.Next(1, 1800),
                        GameId = createdGame.Id,
                        DefenseGamePlayerId = guestGamePlayers[i].Id,
                        OffenseGamePlayerId = homeGamePlayers[i].Id
                    };

                    var createdProjection = await _projectionsService.CreateProjectionAsync(newProjection);
                }

                SelectedGame = createdGame;
                RefreshGameList();
            }
        }
    }
}
