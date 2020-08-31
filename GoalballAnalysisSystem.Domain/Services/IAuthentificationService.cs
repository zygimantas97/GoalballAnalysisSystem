using GoalballAnalysisSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.Domain.Services
{
    public interface IAuthentificationService
    {
        Task<RegistrationResult> Register(string name, string surname, string email, string password, string confirmPassword);
        Task<User> Login(string email, string password);
    }
}
