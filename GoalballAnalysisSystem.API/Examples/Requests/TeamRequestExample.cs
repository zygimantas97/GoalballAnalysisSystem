using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class TeamRequestExample : IExamplesProvider<TeamRequest>
    {
        public TeamRequest GetExamples()
        {
            return new TeamRequest
            {
                Name = "Lithuania",
                Country = "LTU",
                Description = "Very good team"
            };
        }
    }
}
