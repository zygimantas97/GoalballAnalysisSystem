using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class PlayerResponseExample : IExamplesProvider<PlayerResponse>
    {
        public PlayerResponse GetExamples()
        {
            return new PlayerResponse
            {
                Id = 1,
                Name = "Povilas",
                Surname = "Povilaitis",
                Country = "LTU",
                Description = "Very good player"
            };
        }
    }
}
