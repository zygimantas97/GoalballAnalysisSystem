using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class PlayerRoleResponseExample : IExamplesProvider<PlayerRoleResponse>
    {
        public PlayerRoleResponse GetExamples()
        {
            return new PlayerRoleResponse
            {
                Id = 1,
                Name = "LeftStriker"
            };
        }
    }
}
