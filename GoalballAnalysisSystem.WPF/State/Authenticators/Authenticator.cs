using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.WPF.Models;
using GoalballAnalysisSystem.WPF.State.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserStore _userStore;

        public Authenticator(IAuthenticationService authenticationService, IUserStore userStore)
        {
            _authenticationService = authenticationService;
            _userStore = userStore;
        }

        public User CurrentUser
        {
            get { return _userStore.CurrentUser; }
            private set
            {
                _userStore.CurrentUser = value;
                StateChanged?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public event Action StateChanged;

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
