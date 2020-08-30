using GoalballAnalysisSystem.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public class GoalballAnalysisSystemViewModelAbstractFactory : IGoalballAnalysisSystemViewModelAbstractFactory
    {
        private readonly IGoalballAnalysisSystemViewModelFactory<HomeViewModel> _homeViewModelFactory;
        private readonly IGoalballAnalysisSystemViewModelFactory<GamesViewModel> _gamesViewModelFactory;
        private readonly IGoalballAnalysisSystemViewModelFactory<TeamsViewModel> _teamsViewModelFactory;
        private readonly IGoalballAnalysisSystemViewModelFactory<PlayersViewModel> _playersViewModelFactory;

        public GoalballAnalysisSystemViewModelAbstractFactory(IGoalballAnalysisSystemViewModelFactory<HomeViewModel> homeViewModelFactory, IGoalballAnalysisSystemViewModelFactory<GamesViewModel> gamesViewModelFactory, IGoalballAnalysisSystemViewModelFactory<TeamsViewModel> teamsViewModelFactory, IGoalballAnalysisSystemViewModelFactory<PlayersViewModel> playersViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _gamesViewModelFactory = gamesViewModelFactory;
            _teamsViewModelFactory = teamsViewModelFactory;
            _playersViewModelFactory = playersViewModelFactory;
        }

        public BaseViewModel CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _homeViewModelFactory.CreateViewModel();
                case ViewType.Games:
                    return _gamesViewModelFactory.CreateViewModel();
                case ViewType.Teams:
                    return _teamsViewModelFactory.CreateViewModel();
                case ViewType.Players:
                    return _playersViewModelFactory.CreateViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
