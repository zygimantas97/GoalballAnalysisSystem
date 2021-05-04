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
    public class AnalysisViewModel : BaseViewModel, ISelectableProperties
    {
        #region Commands
        public ICommand ChangeSelectedObjectCommand { get; }
        public ICommand EditSelectedObjectCommand { get; }
        public ICommand DeleteSelectedObjectCommand { get; }
        public ICommand CreateNewObjectCommand { get; }
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
                //value can be selected only when record is not in edit mode
                if (EditModeOff)
                {
                    _selectedGame = value;
                    OnPropertyChanged(nameof(SelectedGame));

                    RefreshProjectionsList();
                    RefreshPlayersList();

                    SelectedPlayer = null;
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

        private PlayerResponse _selectedPlayer;
        public PlayerResponse SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        private int _incomingProjections;
        public int IncomingProjections
        {
            get
            {
                return _incomingProjections;
            }
            set
            {
                _incomingProjections = value;
                OnPropertyChanged(nameof(IncomingProjections));

            }
        }

        private bool _incomingProjectionsChecked;
        public bool IncomingProjectionsChecked
        {
            get
            {
                return _incomingProjectionsChecked;
            }
            set
            {
                _incomingProjectionsChecked = value;
                OnPropertyChanged(nameof(IncomingProjectionsChecked));

            }
        }

        private bool _outgoingProjectionsChecked;
        public bool OutgoingProjectionsChecked
        {
            get
            {
                return _outgoingProjectionsChecked;
            }
            set
            {
                _outgoingProjectionsChecked = value;
                OnPropertyChanged(nameof(OutgoingProjectionsChecked));

            }
        }

        private int _outgoingProjections;
        public int OutgoingProjections
        {
            get
            {
                return _outgoingProjections;
            }
            set
            {
                _outgoingProjections = value;
                OnPropertyChanged(nameof(OutgoingProjections));

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

        private Dictionary<long, GamePlayerResponse> _listOfGamePlayers;

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

        public AnalysisViewModel(GamesService gameService, ProjectionsService projectionService, GamePlayersService gamePlayersService, TeamsService teamsService, TeamPlayersService teamPlayersService, PlayersService playersService)
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
            _listOfHomePlayers = new ObservableCollection<PlayerResponse>();
            _listOfGuestPlayers = new ObservableCollection<PlayerResponse>();
            _listOfGamePlayers = new Dictionary<long, GamePlayerResponse>();

            SelectedGameZone = new Rectangle();

            CreateNewObjectCommand = new CreateObjectCommand(this);
            ChangeSelectedObjectCommand = new SelectObjectCommand(this);
            DeleteSelectedObjectCommand = new DeleteObjectCommand(this);
            EditSelectedObjectCommand = new TurnEditModeCommand(this);
            IncomingProjections = 0;
            OutgoingProjections = 0;
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
            else if (parameter is PlayerResponse)
                SelectedPlayer = (PlayerResponse)parameter;
        }

        public async void DeleteSelectedObject(object parameter)
        {
            if (parameter is GameResponse)
            {
                var success = await _gamesService.DeleteGameAsync(SelectedGame.Id);
                if (success != null)
                {
                    SelectedGame = null;
                    RefreshGameList();

                }
            }
            else if (parameter is PlayerResponse)
            {
                SelectedPlayer = null;
                RefreshProjectionsList();
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
                    if (SelectedProjection.DefenseGamePlayerId != null)
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

        public async Task RefreshProjectionsList()
        {
            IncomingProjections = 0;
            OutgoingProjections = 0;

            _uiContext.Send(x => _listOfProjections.Clear(), null);

            if (SelectedGame != null)
            {
                var projectionsList = await _projectionsService.GetProjectionsByGameAsync(SelectedGame.Id);

                if (SelectedGameZone == null || SelectedGameZone == new Rectangle())
                {
                    foreach (var projection in projectionsList)
                    {
                        if (SelectedPlayer == null)
                        {
                            _uiContext.Send(x => _listOfProjections.Add(projection), null);
                        }
                        else
                        {
                            if (_listOfGamePlayers.ContainsKey(Convert.ToInt64(projection.OffenseGamePlayerId)))
                            {
                                var offenseGamePlayer = _listOfGamePlayers[Convert.ToInt64(projection.OffenseGamePlayerId)];
                                if (offenseGamePlayer.TeamPlayer.Player.Id == SelectedPlayer.Id)
                                    _uiContext.Send(x => _listOfProjections.Add(projection), null);
                            }
                            else if (_listOfGamePlayers.ContainsKey(Convert.ToInt64(projection.DefenseGamePlayerId)))
                            {
                                var defenseeGamePlayer = _listOfGamePlayers[Convert.ToInt64(projection.DefenseGamePlayerId)];
                                if (defenseeGamePlayer.TeamPlayer.Player.Id == SelectedPlayer.Id)
                                    _uiContext.Send(x => _listOfProjections.Add(projection), null);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var projection in projectionsList)
                    {
                        bool playerFound = false;
                        if (SelectedPlayer != null)
                        {
                            if (_listOfGamePlayers.ContainsKey(Convert.ToInt64(projection.OffenseGamePlayerId)))
                            {
                                var gamePlayer = _listOfGamePlayers[Convert.ToInt64(projection.OffenseGamePlayerId)];
                                if (gamePlayer.TeamPlayer.Player.Id == SelectedPlayer.Id)
                                    playerFound = true;

                            }
                            else if (_listOfGamePlayers.ContainsKey(Convert.ToInt64(projection.DefenseGamePlayerId)))
                            {
                                var gamePlayer = _listOfGamePlayers[Convert.ToInt64(projection.DefenseGamePlayerId)];
                                if (gamePlayer.TeamPlayer.Player.Id == SelectedPlayer.Id)
                                    playerFound = true;
                            }
                        }

                        if (SelectedPlayer != null && playerFound || SelectedPlayer == null)
                        {
                            if (projection.X1 >= SelectedGameZone.X && projection.X1 <= SelectedGameZone.X + SelectedGameZone.Width && projection.Y1 >= SelectedGameZone.Y && projection.Y1 <= SelectedGameZone.Y + SelectedGameZone.Height && OutgoingProjectionsChecked)
                            {
                                _uiContext.Send(x => _listOfProjections.Add(projection), null);
                                OutgoingProjections++;
                            }
                            else if (projection.X2 >= SelectedGameZone.X && projection.X2 <= SelectedGameZone.X + SelectedGameZone.Width && projection.Y2 >= SelectedGameZone.Y && projection.Y2 <= SelectedGameZone.Y + SelectedGameZone.Height && IncomingProjectionsChecked)
                            {
                                _uiContext.Send(x => _listOfProjections.Add(projection), null);
                                IncomingProjections++;
                            }
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
            _uiContext.Send(x => _listOfGamePlayers.Clear(), null);

            if (SelectedGame != null)
            {
                var gamePlayersList = await _gamePlayersService.GetGamePlayersByGameAsync(SelectedGame.Id);
                var homeTeamId = SelectedGame.HomeTeamId;
                var guestTeamId = SelectedGame.GuestTeamId;

                foreach (var gamePlayer in gamePlayersList)
                {
                    var player = gamePlayer.TeamPlayer.Player;
                    var teamId = gamePlayer.TeamPlayer.TeamId;
                    _listOfGamePlayers.Add(gamePlayer.Id, gamePlayer);
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

        public void CreateNewObject()
        {
            throw new NotImplementedException();//create a new analysis is not available
        }
    }
}
