using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.State.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IIdentityService _identityService;
        private readonly IUserStore _userStore;

        public Authenticator(IIdentityService identityService, IUserStore userStore)
        {
            _identityService = identityService;
            _userStore = userStore;
        }

        public AuthenticationResponse CurrentUser
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
                CurrentUser = await _identityService.LoginAsync(email, password);
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public async Task<AuthenticationResponse> Register(string userName, string surname, string email, string password, string confirmPassword)
        {
            return await _identityService.RegisterAsync(userName, email, password);
        }
    }
}
