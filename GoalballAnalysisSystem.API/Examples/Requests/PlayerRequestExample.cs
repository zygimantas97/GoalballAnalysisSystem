using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class PlayerRequestExample : IExamplesProvider<PlayerRequest>
    {
        public PlayerRequest GetExamples()
        {
            return new PlayerRequest
            {
                Name = "Povilas",
                Surname = "Povilaitis",
                Country = "LTU",
                Description = "Very good player"
            };
        }
    }
}
