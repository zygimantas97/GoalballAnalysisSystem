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
        public ICommand ChangeSelectedObjectCommand { get; }
        public ICommand EditSelectedObjectCommand { get; }
        public ICommand DeleteSelectedObjectCommand { get; }
        public ICommand CreateNewObjectCommand { get; }
        #endregion

        #region Definitions
        private SynchronizationContext _uiContext;
        private PlayersService _playersService;

        private readonly ObservableCollection<PlayerResponse> _listOfPlayers;
        public ObservableCollection<PlayerResponse> ListOfPlayers
        {
            get { return _listOfPlayers; }
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
                if (EditModeOff)
                {
                    _selectedPlayer = value;
                    OnPropertyChanged(nameof(SelectedPlayer));

                    if (value != null)
                    {
                        CanBeEdited = true;
                        CanBeDeleted = true;
                    }
                    else
                    {
                        CanBeEdited = false;
                        CanBeDeleted = false;
                    }
                }

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

        private bool _canBeEdited;
        public bool CanBeEdited
        {
            get
            {
                return _canBeEdited;
            }
            set
            {
                _canBeEdited = value;
                OnPropertyChanged(nameof(CanBeEdited));
            }
        }

        private bool _canBeDeleted;
        public bool CanBeDeleted
        {
            get
            {
                return _canBeDeleted;
            }
            set
            {
                _canBeDeleted = value;
                OnPropertyChanged(nameof(CanBeDeleted));
            }
        }
        #endregion
        public PlayersViewModel(PlayersService playersService)
        {
            _uiContext = SynchronizationContext.Current;
            _playersService = playersService;
            _listOfPlayers = new ObservableCollection<PlayerResponse>();

            RefreshPlayersList();

            ChangeSelectedObjectCommand = new SelectObjectCommand(this);
            DeleteSelectedObjectCommand = new DeleteObjectCommand(this);
            EditSelectedObjectCommand = new TurnEditModeCommand(this);
            CreateNewObjectCommand = new CreateObjectCommand(this);

            EditModeOff = true;
            CanBeCreated = true;
            CanBeEdited = false;
            CanBeDeleted = false;
        }

        public async void ChangeEditMode(object parameter)
        {
            EditModeOff = !EditModeOff;
            CanBeCreated = !CanBeCreated;
            CanBeDeleted = !CanBeDeleted;

            if (parameter is PlayerResponse && EditModeOff && SelectedPlayer != null) //edit has been finished
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
                    RefreshPlayersList();
                    SelectedPlayer = null;
                }
            }
        }

        public async void RefreshPlayersList()
        {
            var playersList = await _playersService.GetPlayersAsync();

            _uiContext.Send(x => _listOfPlayers.Clear(), null);

            foreach (var player in playersList)
            {
                _uiContext.Send(x => _listOfPlayers.Add(player), null);
            }
        }
        public async void CreateNewObject()
        {
            if(EditModeOff)
                SelectedPlayer = new PlayerResponse(); //clear inout fields

            EditModeOff = !EditModeOff;

            if (!EditModeOff)
            {
                CanBeEdited = false;
                CanBeDeleted = false;
            }

            if (EditModeOff) //edit has been finished
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
        }
    }
}
