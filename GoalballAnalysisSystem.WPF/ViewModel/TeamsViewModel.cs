using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.State.Users;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class TeamsViewModel : BaseViewModel, ISelectableProperties
    {
        private readonly IUserStore _userStore;

        public ICommand ChangeSelectedObjectCommand { get; }
        public ICommand EditSelectedObjectCommand { get; }
        public ICommand DeleteSelectedObjectCommand { get; }
        public ICommand AddNewTeam { get; }

        private readonly ObservableCollection<TeamResponse> _teams;

        public ObservableCollection<TeamResponse> ListOfTeams
        {
            get { return _teams; }
        }

        private ObservableCollection<PlayerResponse> _players;

        public ObservableCollection<PlayerResponse> ListOfPlayers
        {
            get { return _players; }
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
                _selectedTeam = value;
                OnPropertyChanged(nameof(SelectedTeam));
                FillPlayersFromSelectedTeam();
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


        public TeamsViewModel(IUserStore userStore)
        {
            _players = new ObservableCollection<PlayerResponse>();
            ChangeSelectedObjectCommand = new ChangeSelecedInterfaceObject(this);
            DeleteSelectedObjectCommand = new DeleteSelecedInterfaceObject(this);
            EditSelectedObjectCommand = new TurnEditMode(this);

            CanNotBeEdited = true;
            _teams = AddFakeData();
        }

        public ObservableCollection<TeamResponse> AddFakeData()
        {
            ObservableCollection<TeamResponse> list = new ObservableCollection<TeamResponse>();

            for (int i = 0; i < 50; i++)
            {
                TeamResponse a = new TeamResponse();
                a.Name = "Team" + i;
                a.Description = "Description" + i;
                a.Id = i;
                list.Add(a);

            }

            return list;
        }

        public void FillPlayersFromSelectedTeam()
        {
            if(_players.Count != 0)
                _players.Clear();

            for(int i=0; i<10; i++)
            {
                PlayerResponse a = new PlayerResponse();
                a.Name = "Petras" + i;
                a.Surname = "Petrauskas" + i;
                _players.Add(a);
            }

        }

        public void ChangeEditMode()
        {
            CanNotBeEdited = !CanNotBeEdited;
        }


        void ISelectableProperties.ChangeSelectedObject(object parameter)
        {
            if (parameter is TeamResponse)
                SelectedTeam = (TeamResponse)parameter;

            if (parameter is PlayerResponse)
                SelectedPlayer = (PlayerResponse)parameter;
        }

        void ISelectableProperties.DeleteSelectedObject(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
