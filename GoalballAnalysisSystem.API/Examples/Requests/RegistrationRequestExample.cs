using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class RegistrationRequestExample : IExamplesProvider<RegistrationRequest>
    {
        public RegistrationRequest GetExamples()
        {
            return new RegistrationRequest
            {
                UserName = "User",
                Email = "user@gas.com",
                Password = "Password123!"
            };
        }
    }
}
