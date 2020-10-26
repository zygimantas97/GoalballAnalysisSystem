using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class UserRequestExample : IExamplesProvider<UserRequest>
    {
        public UserRequest GetExamples()
        {
            return new UserRequest
            {
                UserName = "User1",
                Email = "user1@gas.com",
                Password = "Password123!"
            };
        }
    }
}
