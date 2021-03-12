using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class PlayersViewModel : BaseViewModel
    {

        public ICommand LoginCommand { get; }
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand ChangeSelectedPlayer { get; }
        public ICommand EditSelectedPlayer { get; }
        public ICommand DeleteSelectedPlayer { get; }
        public ICommand AddNewPlayerPlayer { get; }

        private readonly ObservableCollection<Player> _listOfPlayers;

        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            private set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        public ObservableCollection<Player> ListOfPlayers
        {
            get { return _listOfPlayers; }
        }


        public PlayersViewModel()
        {
            _listOfPlayers = AddFakeData();
            if (_listOfPlayers.Count != 0)
                SelectedPlayer = _listOfPlayers[0];



            ChangeSelectedPlayer = new ChangeSelectedPlayer(this);
        }

        public ObservableCollection<Player> AddFakeData()
        {
            ObservableCollection<Player> list = new ObservableCollection<Player>();
            

            for(int i= 0; i< 50; i++)
            {
                Player a = new Player();
                a.Name = "Vardas " + i;
                a.Surname = "Pavarde " + i;
                a.Id = i;
                list.Add(a);
            }

            return list;
        }

        public void SelectPlayer(int Id)
        {
            foreach (Player item in _listOfPlayers)
            {
                if(item.Id == Id)
                {
                    SelectedPlayer = item;
                }
            }
        }

    }
}
