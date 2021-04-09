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

        public async Task Login(string email, string password)
        {
            CurrentUser = await _identityService.LoginAsync(email, password);
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public async Task Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new Exception("Passwords does not match");
            var result = await _identityService.RegisterAsync(username, email, password);
        }
    }
}
