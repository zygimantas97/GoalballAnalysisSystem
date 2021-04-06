using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        AuthenticationResponse CurrentUser { get; }
        bool IsLoggedIn { get; }
        event Action StateChanged;

        Task Register(string username, string email, string password, string confirmPassword);
        Task Login(string email, string password);
        void Logout();
    }
}
