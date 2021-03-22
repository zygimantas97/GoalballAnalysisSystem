using GoalballAnalysisSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.Domain.Services
{
    public interface IUserDataService : IDataService<User>
    {
        Task<User> GetByEmail(string email);
    }
}
