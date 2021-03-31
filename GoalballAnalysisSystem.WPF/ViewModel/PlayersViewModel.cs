using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System.Threading;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class PlayersViewModel : BaseViewModel, ISelectableProperties
    {
        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand ChangeSelectedObjectCommand { get; }
        public ICommand EditSelectedObjectCommand { get; }
        public ICommand DeleteSelectedObjectCommand { get; }
        public ICommand CreateNewObjectCommand { get; }
        #endregion

        #region Definitions

        SynchronizationContext uiContext;
        private PlayersService _playersService;
        private TeamPlayersService _teamPlayersService;
        private TeamsService _teamsService;

        private readonly ObservableCollection<PlayerResponse> _listOfPlayers;
        public ObservableCollection<PlayerResponse> ListOfPlayers
        {
            get { return _listOfPlayers; }
        }

        private readonly ObservableCollection<TeamPlayerResponse> _listOfTeamPlayers;
        public ObservableCollection<TeamPlayerResponse> ListOfTeamPlayers
        {
            get { return _listOfTeamPlayers; }
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
                RefreshTeamPlayersList();
            }
        }

        private bool _canNotBeEdited;
        public bool CanNotBeEdited
        {
            get
            {
                return _canNotBeEdited;
            }
            set
            {
                _canNotBeEdited = value;
                OnPropertyChanged(nameof(CanNotBeEdited));
            }
        }

        private bool _canNotBeCreated;
        public bool CanNotBeCreated
        {
            get
            {
                return _canNotBeCreated;
            }
            set
            {
                _canNotBeCreated = value;
                OnPropertyChanged(nameof(CanNotBeCreated));
            }
        }
        #endregion

        public PlayersViewModel(PlayersService playersService, TeamPlayersService teamPlayersService, TeamsService teamsService)
        {
            uiContext = SynchronizationContext.Current;
            _playersService = playersService;
            _teamPlayersService = teamPlayersService;
            _teamsService = teamsService;
            _listOfPlayers = new ObservableCollection<PlayerResponse>();
            _listOfTeamPlayers = new ObservableCollection<TeamPlayerResponse>();

            Task.Run(() => this.RefreshPlayersList()).Wait();

            ChangeSelectedObjectCommand = new ChangeSelecedInterfaceObject(this);
            DeleteSelectedObjectCommand = new DeleteSelecedInterfaceObject(this);
            EditSelectedObjectCommand = new TurnEditMode(this);
            CreateNewObjectCommand = new CreateSelectedInterfaceObject(this);

            CanNotBeEdited = true;
            CanNotBeCreated = true;
        }


        public async void ChangeEditMode()
        {
            CanNotBeEdited = !CanNotBeEdited;

            if (CanNotBeEdited && SelectedPlayer != null) //edit has been finished
            {
                PlayerRequest playerToEdit = new PlayerRequest
                {
                    Name = SelectedPlayer.Name,
                    Surname = SelectedPlayer.Surname,
                    Country = SelectedPlayer.Country,
                    Description = SelectedPlayer.Description
                };

                await _playersService.UpdatePlayerAsync(SelectedPlayer.Id, playerToEdit);
            }
        }

        public void ChangeSelectedObject(object parameter)
        {
            if (parameter is PlayerResponse)
                SelectedPlayer = (PlayerResponse)parameter;
        }

        public async void DeleteSelectedObject(object parameter)
        {
            if (parameter is PlayerResponse)
            {
                var success = await _playersService.DeletePlayerAsync(SelectedPlayer.Id);
                if (success != null)
                {
                    Task.Run(() => this.RefreshPlayersList()).Wait();
                    SelectedPlayer = null;
                }

            }

            if (parameter is TeamPlayerResponse)
            {
                TeamPlayerResponse teamPlayerResponse = (TeamPlayerResponse)parameter;
                var teamPlayerSuccess = await _teamPlayersService.DeleteTeamPlayerAsync(teamPlayerResponse.TeamId, teamPlayerResponse.PlayerId);
                if (teamPlayerSuccess != null)
                {
                    Task.Run(() => this.RefreshTeamPlayersList()).Wait();
                }

            }
        }

        public async void RefreshPlayersList()
        {
            var playersList = await _playersService.GetPlayersAsync();

            uiContext.Send(x => _listOfPlayers.Clear(), null);

            foreach (var player in playersList)
            {
                uiContext.Send(x => _listOfPlayers.Add(player), null);
            }

        }

        public async void RefreshTeamPlayersList()
        {
            if (SelectedPlayer != null)
            {
                var teamPlayers = await _teamPlayersService.GetTeamPlayersByPlayerAsync(SelectedPlayer.Id);

                uiContext.Send(x => _listOfTeamPlayers.Clear(), null);

                foreach (var teamPlayer in teamPlayers)
                {
                    _listOfTeamPlayers.Add(teamPlayer);
                }
            }
        }

        public async void CreateNewObject()
        {

            CanNotBeEdited = !CanNotBeEdited;

            if (!CanNotBeEdited)
            {
                SelectedPlayer = new PlayerResponse();
            }

            if (CanNotBeEdited) //edit has been finished
            {

                var newPlayer = new PlayerRequest
                {
                    Name = SelectedPlayer.Name,
                    Surname = SelectedPlayer.Surname,
                    Country = SelectedPlayer.Country,
                    Description = SelectedPlayer.Description
                };

                var createdPlayer = await _playersService.CreatePlayerAsync(newPlayer);
                SelectedPlayer = createdPlayer;
                RefreshPlayersList();
            }


            //Test data package

            //PlayerRequest player = new PlayerRequest { Name = "Jonas", Surname = "Jonauskas", Country = "LT", Description = "Player of Lithaunia team" };
            //PlayerRequest player2 = new PlayerRequest { Name = "Saulius", Surname = "Sauliauskas", Country = "LT", Description = "Player of Lithuania team" };
            //PlayerRequest player3 = new PlayerRequest { Name = "Rimas", Surname = "Rimauskas", Country = "LT", Description = "Player of Lithuania team" };


            //PlayerRequest player4 = new PlayerRequest { Name = "John", Surname = "Jhonson", Country = "USA", Description = "Player of USA team" };
            //PlayerRequest player5 = new PlayerRequest { Name = "Sam", Surname = "Samith", Country = "USA", Description = "Player of USA team" };
            //PlayerRequest player6 = new PlayerRequest { Name = "Manny", Surname = "Manson", Country = "USA", Description = "Player of USA team" };

            //TeamRequest team = new TeamRequest { Name = "LTU One", Country = "Lithuania", Description = "First team of Lithuania" };
            //TeamRequest team2 = new TeamRequest { Name = "USA One", Country = "United States", Description = "First team of USA" };

            //var successTeam1 = await _teamsService.CreateTeamAsync(team);
            //var successTeam2 = await _teamsService.CreateTeamAsync(team2);

            //var successPlayer1 = await _playersService.CreatePlayerAsync(player);
            //var successPlayer2 = await _playersService.CreatePlayerAsync(player2);
            //var successPlayer3 = await _playersService.CreatePlayerAsync(player3);
            //var successPlayer4 = await _playersService.CreatePlayerAsync(player4);
            //var successPlayer5 = await _playersService.CreatePlayerAsync(player5);
            //var successPlayer6 = await _playersService.CreatePlayerAsync(player6);

            //Task.Run(() => this.RefreshPlayersList()).Wait();

            //await _teamPlayersService.CreateTeamPlayerAsync(successTeam1.Id, successPlayer1.Id, new TeamPlayerRequest { Number = 1 });
            //await _teamPlayersService.CreateTeamPlayerAsync(successTeam1.Id, successPlayer2.Id, new TeamPlayerRequest { Number = 2 });
            //await _teamPlayersService.CreateTeamPlayerAsync(successTeam1.Id, successPlayer3.Id, new TeamPlayerRequest { Number = 3 });
            //await _teamPlayersService.CreateTeamPlayerAsync(successTeam2.Id, successPlayer4.Id, new TeamPlayerRequest { Number = 1 });
            //await _teamPlayersService.CreateTeamPlayerAsync(successTeam2.Id, successPlayer5.Id, new TeamPlayerRequest { Number = 2 });
            //await _teamPlayersService.CreateTeamPlayerAsync(successTeam2.Id, successPlayer6.Id, new TeamPlayerRequest { Number = 3 });


        }

    }
}
