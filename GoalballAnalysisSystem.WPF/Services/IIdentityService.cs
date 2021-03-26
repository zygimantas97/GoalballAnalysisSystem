using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> RegisterAsync(string userName, string email, string password);
        Task<AuthenticationResponse> LoginAsync(string email, string password);
        Task<string> GetToken();
    }
}
