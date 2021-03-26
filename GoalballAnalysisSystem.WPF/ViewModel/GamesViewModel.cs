using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.Commands;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        public ICommand NextProjectionCommand { get; }
        #endregion

        #region Definitions
        private readonly ObservableCollection<GameResponse> _games;
        public ObservableCollection<GameResponse> ListOfGames
        {
            get { return _games; }
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
                AddFakeProjections();
            }
        }

        private ObservableCollection<ProjectionResponse> _projections;
        public ObservableCollection<ProjectionResponse> Projections
        {
            get { return _projections; }
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
            }
        }

        private int _currentProjectionIndex;


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

        public GamesViewModel()
        {
            _projections = new ObservableCollection<ProjectionResponse>();
            _games = AddFakeData();
            ChangeSelectedObjectCommand = new ChangeSelecedInterfaceObject(this);
            DeleteSelectedObjectCommand = new DeleteSelecedInterfaceObject(this);
            EditSelectedObjectCommand = new TurnEditMode(this);
            PreviousProjectionCommand = new PreviousProjection(this);
            NextProjectionCommand = new NextProjection(this);

            CanNotBeEdited = true;
            _currentProjectionIndex = 0;
        }

        private static ObservableCollection<GameResponse> AddFakeData()
        {
            ObservableCollection<GameResponse> list = new ObservableCollection<GameResponse>();


            for (int i = 0; i < 50; i++)
            {
                GameResponse a = new GameResponse();
                a.Title = "Pavadinimas " + i;
                a.Comment = "Zaidimas " + i;
                a.Id = i;
                list.Add(a);
            }

            return list;
        }

        /// <summary>
        /// Method is called when game is selected. Should fill projections observable collection with the values of specific game
        /// </summary>
        private void AddFakeProjections()
        {
            if (_projections.Count != 0)
                _projections.Clear();
            for (int i=0; i<10; i++)
            {
                ProjectionResponse proj = new ProjectionResponse();
                proj.X1 = i;
                proj.X2 = i;
                proj.Y1 = i + 1;
                proj.Y2 = i + 1;
                _projections.Add(proj);
            }

            if (_projections.Count != 0)
            {
                SelectedProjection = _projections[0];
            }
            else
            {
                SelectedProjection = null;
            }
            _currentProjectionIndex = 0;

        }


        public void ChangeEditMode()
        {
            CanNotBeEdited = !CanNotBeEdited;
        }

        void ISelectableProperties.ChangeSelectedObject(object parameter)
        {
            if (parameter is GameResponse)
                SelectedGame = (GameResponse)parameter;
        }

        void ISelectableProperties.DeleteSelectedObject(object parameter)
        {
            throw new NotImplementedException();
        }

        public void NextProjection()
        {
            if(_currentProjectionIndex < Projections.Count-1 && Projections[0]!=null)
            {
                _currentProjectionIndex++;
                SelectedProjection = Projections[_currentProjectionIndex];
            }
        }
        public void PreviousProjection()
        {
            if (_currentProjectionIndex > 0 && Projections[0] != null)
            {
                _currentProjectionIndex--;
                SelectedProjection = Projections[_currentProjectionIndex];
            }
        }
    }
}
