using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class RefreshTokenRequestExample : IExamplesProvider<RefreshTokenRequest>
    {
        public RefreshTokenRequest GetExamples()
        {
            return new RefreshTokenRequest
            {
                Token = "Your expired token",
                RefreshToken = "Your active refresh token"
            };
        }
    }
}
