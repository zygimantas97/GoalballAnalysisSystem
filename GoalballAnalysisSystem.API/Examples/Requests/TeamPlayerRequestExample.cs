using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class TeamPlayerRequestExample : IExamplesProvider<TeamPlayerRequest>
    {
        public TeamPlayerRequest GetExamples()
        {
            return new TeamPlayerRequest
            {
                Number = 1,
                RoleId = 1
            };
        }
    }
}
