using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.State.Users;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class TeamsViewModel : BaseViewModel, ISelectableProperties
    {
        #region Commands
        public ICommand ChangeSelectedObjectCommand { get; }
        public ICommand EditSelectedObjectCommand { get; }
        public ICommand DeleteSelectedObjectCommand { get; }
        public ICommand CreateSelectedObjectCommand { get; }
        public ICommand CreateNewTeamPlayerCommand { get; set; }
        #endregion

        #region Definitions

        SynchronizationContext uiContext;
        private TeamsService _teamsService;
        private TeamPlayersService _teamPlayersService;
        private PlayersService _playersService;
        private PlayerRolesService _playerRolesService;

        private readonly ObservableCollection<TeamResponse> _listOfTeams;
        public ObservableCollection<TeamResponse> ListOfTeams
        {
            get { return _listOfTeams; }
        }

        private ObservableCollection<PlayerResponse> _listOfPlayers;

        public ObservableCollection<PlayerResponse> ListOfPlayers
        {
            get { return _listOfPlayers; }
        }

        private ObservableCollection<PlayerResponse> _listOfAvailablePlayers;

        public ObservableCollection<PlayerResponse> ListOfAvailablePlayers
        {
            get { return _listOfAvailablePlayers; }
        }

        private ObservableCollection<PlayerRoleResponse> _listOfRoles;

        public ObservableCollection<PlayerRoleResponse> ListOfRoles
        {
            get { return _listOfRoles; }
        }

        private TeamResponse _selectedTeam;
        public TeamResponse SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                if (TeamEditModeOff && TeamPlayerEditModeOff)
                {
                    _selectedTeam = value;
                    OnPropertyChanged(nameof(SelectedTeam));
                    RefreshPlayersList();
                    RefreshAvailablePlayersList();

                    if (value != null)
                    {
                        CanBeEditedTeam = true;
                        CanBeDeletedTeam = true;
                    }
                    else
                    {
                        CanBeEditedTeam = false;
                        CanBeDeletedTeam = false;
                    }
                }
 
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
                RefreshTeamPlayer();
            }
        }

        private TeamPlayerResponse _selectedTeamPlayer;
        public TeamPlayerResponse SelectedTeamPlayer
        {
            get
            {
                return _selectedTeamPlayer;
            }
            set
            {
                if(TeamEditModeOff && TeamPlayerEditModeOff)
                {
                    _selectedTeamPlayer = value;
                    OnPropertyChanged(nameof(SelectedTeamPlayer));
                    RefreshAvailablePlayersList();

                    if (value != null)
                    {
                        CanBeEditedTeamPlayer = true;
                        if (TeamPlayerEditModeOff)
                            CanBeDeletedTeamPlayer = true;
                        SelectedRole = _selectedTeamPlayer.Role;
                    }
                    else
                    {
                        CanBeEditedTeamPlayer = false;
                        CanBeDeletedTeamPlayer = false;
                    }
                }
            }
        }

        private PlayerResponse _selectedComboBoxItem;
        public PlayerResponse SelectedComboBoxItem
        {
            get
            {
                return _selectedComboBoxItem;
            }
            set
            {
                _selectedComboBoxItem = value;
                OnPropertyChanged(nameof(SelectedComboBoxItem));
            }
        }

        private PlayerRoleResponse _selectedRole;
        public PlayerRoleResponse SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }


        private bool _teamEditModeOff;
        public bool TeamEditModeOff
        {
            get
            {
                return _teamEditModeOff;
            }
            set
            {
                _teamEditModeOff = value;
                OnPropertyChanged(nameof(TeamEditModeOff));
            }
        }

        private bool _teamPlayerEditModeOff;
        public bool TeamPlayerEditModeOff
        {
            get
            {
                return _teamPlayerEditModeOff;
            }
            set
            {
                _teamPlayerEditModeOff = value;
                OnPropertyChanged(nameof(TeamPlayerEditModeOff));
            }
        }

        private bool _canBeCreatedTeam;
        public bool CanBeCreatedTeam
        {
            get
            {
                return _canBeCreatedTeam;
            }
            set
            {
                _canBeCreatedTeam = value;
                OnPropertyChanged(nameof(CanBeCreatedTeam));
            }
        }

        private bool _canBeEditedTeam;
        public bool CanBeEditedTeam
        {
            get
            {
                return _canBeEditedTeam;
            }
            set
            {
                _canBeEditedTeam = value;
                OnPropertyChanged(nameof(CanBeEditedTeam));
            }
        }

        private bool _canBeEditedTeamPlayer;
        public bool CanBeEditedTeamPlayer
        {
            get
            {
                return _canBeEditedTeamPlayer;
            }
            set
            {
                _canBeEditedTeamPlayer = value;
                OnPropertyChanged(nameof(CanBeEditedTeamPlayer));
            }
        }

        private bool _canBeDeletedTeam;
        public bool CanBeDeletedTeam
        {
            get
            {
                return _canBeDeletedTeam;
            }
            set
            {
                _canBeDeletedTeam = value;
                OnPropertyChanged(nameof(CanBeDeletedTeam));
            }
        }

        private bool _canBeDeletedTeamPlayer;
        public bool CanBeDeletedTeamPlayer
        {
            get
            {
                return _canBeDeletedTeamPlayer;
            }
            set
            {
                _canBeDeletedTeamPlayer = value;
                OnPropertyChanged(nameof(CanBeDeletedTeamPlayer));
            }
        }
        #endregion

        public TeamsViewModel(TeamsService teamsService, TeamPlayersService teamPlayersService, PlayersService playersService, PlayerRolesService playerRolesService)
        {
            _teamsService = teamsService;
            _teamPlayersService = teamPlayersService;
            _playersService = playersService;
            _playerRolesService = playerRolesService;
            uiContext = SynchronizationContext.Current;

            _listOfPlayers = new ObservableCollection<PlayerResponse>();
            _listOfAvailablePlayers = new ObservableCollection<PlayerResponse>();
            _listOfTeams = new ObservableCollection<TeamResponse>();
            _listOfRoles = new ObservableCollection<PlayerRoleResponse>();

            ChangeSelectedObjectCommand = new SelectObjectCommand(this);
            DeleteSelectedObjectCommand = new DeleteObjectCommand(this);
            EditSelectedObjectCommand = new TurnEditModeCommand(this);
            CreateNewTeamPlayerCommand = new CreateNewTeamPlayer(this, teamPlayersService);
            CreateSelectedObjectCommand = new CreateObjectCommand(this);

            RefreshTeamsList();
            RefreshRolesList();

            TeamEditModeOff = true;
            TeamPlayerEditModeOff = true;
            CanBeCreatedTeam = true;
            CanBeEditedTeam = false;
            CanBeDeletedTeam = false;
            CanBeEditedTeamPlayer = false;
            CanBeDeletedTeamPlayer = false;
        }

        public async void ChangeEditMode(object parameter)
        {
            CanBeCreatedTeam = !CanBeCreatedTeam;
            CanBeDeletedTeam = !CanBeDeletedTeam;
            
            if(SelectedTeamPlayer != null)
                CanBeDeletedTeamPlayer = !CanBeDeletedTeamPlayer;

            if (parameter is TeamResponse)
            {
                TeamEditModeOff = !TeamEditModeOff;

                if (SelectedTeamPlayer != null)
                    CanBeEditedTeamPlayer = !CanBeEditedTeamPlayer;

                if (TeamEditModeOff)
                {
                    if (SelectedTeam != null)
                    {
                        TeamRequest teamToEdit = new TeamRequest
                        {
                            Name = SelectedTeam.Name,
                            Country = SelectedTeam.Country,
                            Description = SelectedTeam.Description
                        };

                        await _teamsService.UpdateTeamAsync(SelectedTeam.Id, teamToEdit);
                    }
                }
            }
            else if (parameter is TeamPlayerResponse)
            {
                TeamPlayerEditModeOff = !TeamPlayerEditModeOff;
                CanBeEditedTeam = !CanBeEditedTeam;

                if (TeamPlayerEditModeOff)
                {
                    if (SelectedTeamPlayer != null && SelectedPlayer != null)
                    {
                        TeamPlayerRequest teamPlayerToEdit = new TeamPlayerRequest
                        {
                            Number = SelectedTeamPlayer.Number
                        };

                        if(SelectedRole != null)
                        {
                            teamPlayerToEdit.RoleId = SelectedRole.Id;
                        }

                        await _teamPlayersService.UpdateTeamPlayerAsync(SelectedTeam.Id, SelectedPlayer.Id, teamPlayerToEdit);
                    }
                }
            }
        }

        public void ChangeSelectedObject(object parameter)
        {
            if (parameter is TeamResponse)
                SelectedTeam = (TeamResponse)parameter;

            else if (parameter is PlayerResponse)
                SelectedPlayer = (PlayerResponse)parameter;
        }

        public async void DeleteSelectedObject(object parameter)
        {
            if (parameter is TeamResponse)
            {
                var teamSuccess = await _teamsService.DeleteTeamAsync(SelectedTeam.Id);

                if (teamSuccess != null)
                {
                    RefreshTeamsList();
                    RefreshPlayersList();

                    SelectedTeam = null;
                    SelectedTeamPlayer = null;
                    SelectedPlayer = null;
                }
            }

            else if (parameter is TeamPlayerResponse)
            {
                var success = await _teamPlayersService.DeleteTeamPlayerAsync(SelectedTeam.Id, _selectedPlayer.Id);
                if (success != null)
                {
                    RefreshPlayersList();
                    SelectedTeamPlayer = null;
                    SelectedPlayer = null;
                    SelectedRole = null;
                }
            }
        }

        public async void CreateNewObject()
        {
            if (TeamEditModeOff)
            {
                SelectedTeam = new TeamResponse();
                SelectedTeamPlayer = null;
                SelectedPlayer = null;
            }

            TeamEditModeOff = !TeamEditModeOff;
            CanBeDeletedTeamPlayer = false;
            CanBeEditedTeamPlayer = false;

            if (!TeamEditModeOff)
            {
                CanBeEditedTeam = false;
                CanBeDeletedTeam = false;
            }

            if (TeamEditModeOff) //edit has been finished
            {
                var newTeam = new TeamRequest
                {
                    Name = SelectedTeam.Name,
                    Country = SelectedTeam.Country,
                    Description = SelectedTeam.Description
                };

                var createdTeam = await _teamsService.CreateTeamAsync(newTeam);
                SelectedTeam = createdTeam;
                RefreshTeamsList();
                RefreshPlayersList();
                RefreshAvailablePlayersList();
            }
        }
        private async void RefreshTeamsList()
        {
            var teamsList = await _teamsService.GetTeamsAsync();

            uiContext.Send(x => _listOfTeams.Clear(), null);

            foreach (var team in teamsList)
            {
                uiContext.Send(x => _listOfTeams.Add(team), null);
            }
        }

        public async void RefreshPlayersList()
        {
            if (SelectedTeam != null)
            {
                var teamPlayers = await _teamPlayersService.GetTeamPlayersByTeamAsync(SelectedTeam.Id);
                uiContext.Send(x => _listOfPlayers.Clear(), null);

                foreach (var teamPlayer in teamPlayers)
                {
                    var player = await _playersService.GetPlayerAsync(teamPlayer.PlayerId);
                    uiContext.Send(x => _listOfPlayers.Add(player), null);
                }
            }
        }
        public async void RefreshAvailablePlayersList()
        {
            if (SelectedTeam != null)
            {
                var players = await _playersService.GetPlayersAsync();
                var teamPlayers = await _teamPlayersService.GetTeamPlayersByTeamAsync(SelectedTeam.Id);

                uiContext.Send(x => _listOfAvailablePlayers.Clear(), null);

                bool playerIsAvailable;
                foreach (var player in players)
                {
                    playerIsAvailable = true;

                    foreach (var teamPlayer in teamPlayers)
                    {
                        if (teamPlayer.PlayerId == player.Id)
                        {
                            playerIsAvailable = false;
                        }
                    }

                    if (playerIsAvailable)
                        uiContext.Send(x => _listOfAvailablePlayers.Add(player), null);
                }
            }
        }
        private async void RefreshTeamPlayer()
        {
            if (SelectedPlayer != null && SelectedTeam != null)
            {
                var teamPlayers = await _teamPlayersService.GetTeamPlayerAsync(SelectedTeam.Id, SelectedPlayer.Id);
                SelectedTeamPlayer = teamPlayers;
            }
        }
        public async void RefreshRolesList()
        {
            var allRoles = await _playerRolesService.GetPlayerRolesAsync();

            foreach (var role in allRoles)
            {
                uiContext.Send(x => _listOfRoles.Add(role), null);
            }
        }
    }
}
