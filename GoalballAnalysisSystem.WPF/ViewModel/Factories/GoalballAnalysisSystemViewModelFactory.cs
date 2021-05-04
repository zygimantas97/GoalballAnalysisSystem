using GoalballAnalysisSystem.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Factories
{
    public class GoalballAnalysisSystemViewModelFactory : IGoalballAnalysisSystemViewModelFactory
    {
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<AnalysisViewModel> _createGamesViewModel;
        private readonly CreateViewModel<TeamsViewModel> _createTeamsViewModel;
        private readonly CreateViewModel<PlayersViewModel> _createPlayersViewModel;
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<RegistrationViewModel> _createRegistrationViewModel;
        private readonly CreateViewModel<ProcessingViewModel> _createCalibrationViewModel;

        public GoalballAnalysisSystemViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel,
            CreateViewModel<AnalysisViewModel> createGamesViewModel,
            CreateViewModel<TeamsViewModel> createTeamsViewModel,
            CreateViewModel<PlayersViewModel> createPlayersViewModel,
            CreateViewModel<LoginViewModel> createLoginViewModel,
            CreateViewModel<RegistrationViewModel> createRegistrationViewModel,
            CreateViewModel<ProcessingViewModel> createCalibrationViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            _createGamesViewModel = createGamesViewModel;
            _createTeamsViewModel = createTeamsViewModel;
            _createPlayersViewModel = createPlayersViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createRegistrationViewModel = createRegistrationViewModel;
            _createCalibrationViewModel = createCalibrationViewModel;
        }

        public BaseViewModel CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Registration:
                    return _createRegistrationViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                case ViewType.Analysis:
                    return _createGamesViewModel();
                case ViewType.Teams:
                    return _createTeamsViewModel();
                case ViewType.Players:
                    return _createPlayersViewModel();
                case ViewType.Processing:
                    return _createCalibrationViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
