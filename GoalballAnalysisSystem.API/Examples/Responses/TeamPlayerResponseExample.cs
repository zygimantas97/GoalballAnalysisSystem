using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class TeamPlayerResponseExample : IExamplesProvider<TeamPlayerResponse>
    {
        public TeamPlayerResponse GetExamples()
        {
            return new TeamPlayerResponse
            {
                TeamId = 1,
                PlayerId = 1,
                Number = 1,
                RoleId = 1
            };
        }
    }
}
