using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public interface IIdentityService
    {
        Task RegisterAsync(string userName, string email, string password);
        Task LoginAsync(string email, string password);
        Task<string> GetToken();
    }
}
