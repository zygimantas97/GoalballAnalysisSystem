using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.Commands
{
    public class RegisterCommand : ICommand
    {
        private readonly RegistrationViewModel _registrationViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _renavigator;

        public RegisterCommand(RegistrationViewModel registrationViewModel,
            IAuthenticator authenticator,
            IRenavigator renavigator)
        {
            _registrationViewModel = registrationViewModel;
            _authenticator = authenticator;
            _renavigator = renavigator;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            string[] passInfo = (string[])parameter;
            RegistrationResult result = await _authenticator.Register(_registrationViewModel.Name, _registrationViewModel.Surname, _registrationViewModel.Email, passInfo[0], passInfo[1]);
            if (result == RegistrationResult.Success)
            {
                _renavigator.Renavigate();
            }
        }
    }
}
