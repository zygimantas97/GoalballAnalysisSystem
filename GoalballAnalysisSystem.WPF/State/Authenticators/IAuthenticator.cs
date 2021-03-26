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

        Task<AuthenticationResponse> Register(string name, string surname, string email, string password, string confirmPassword);
        Task<bool> Login(string email, string password);
        void Logout();
    }
}
