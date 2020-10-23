using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class TeamResponseExample : IExamplesProvider<TeamResponse>
    {
        public TeamResponse GetExamples()
        {
            return new TeamResponse
            {
                Id = 1,
                Name = "Lithuania",
                Country = "LTU",
                Description = "Very good team"
            };
        }
    }
}
