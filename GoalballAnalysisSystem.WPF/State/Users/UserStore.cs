using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Users
{
    public class UserStore : IUserStore
    {
        private AuthenticationResponse _currentUser;

        public AuthenticationResponse CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
