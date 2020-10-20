using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class AuthenticationResponseExample : IExamplesProvider<AuthenticationResponse>
    {
        public AuthenticationResponse GetExamples()
        {
            return new AuthenticationResponse
            {
                Token = "Your authentication token",
                RefreshToken = "Your refresh token"
            };
        }
    }
}
