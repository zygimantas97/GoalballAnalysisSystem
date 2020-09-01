using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.WPF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.State.Authenticators
{
    public class Authenticator : ObservableObject, IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;

        public Authenticator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        private User user;

        public User CurrentUser
        {
            get { return user; }
            private set
            {
                user = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public async Task<bool> Login(string email, string password)
        {
            bool success = true;
            try
            {
                CurrentUser = await _authenticationService.Login(email, password);
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public async Task<RegistrationResult> Register(string name, string surname, string email, string password, string confirmPassword)
        {
            return await _authenticationService.Register(name, surname, email, password, confirmPassword);
        }
    }
}
