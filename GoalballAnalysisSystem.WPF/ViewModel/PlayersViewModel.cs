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
        #endregion

        #region Definitions
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
        #endregion

        public PlayersViewModel()
        {
            _listOfPlayers = AddFakeData();
            if (_listOfPlayers.Count != 0)
                SelectedPlayer = _listOfPlayers[0];

            ChangeSelectedObjectCommand = new ChangeSelecedInterfaceObject(this);
            DeleteSelectedObjectCommand = new DeleteSelecedInterfaceObject(this);
            EditSelectedObjectCommand = new TurnEditMode(this);

            CanNotBeEdited = true;
        }

        private static ObservableCollection<PlayerResponse> AddFakeData()
        {
            ObservableCollection<PlayerResponse> list = new ObservableCollection<PlayerResponse>();
            

            for(int i= 0; i< 50; i++)
            {
                PlayerResponse a = new PlayerResponse();
                a.Name = "Vardas " + i;
                a.Surname = "Pavarde " + i;
                a.Id = i;
                list.Add(a);
            }

            return list;
        }


        public void ChangeEditMode()
        {
            CanNotBeEdited = !CanNotBeEdited;
        }

        void ISelectableProperties.ChangeSelectedObject(object parameter)
        {
            if (parameter is PlayerResponse)
                SelectedPlayer = (PlayerResponse)parameter;
        }

        void ISelectableProperties.DeleteSelectedObject(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
