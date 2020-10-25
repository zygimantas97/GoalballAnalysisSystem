using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class GameRequestExample : IExamplesProvider<GameRequest>
    {
        public GameRequest GetExamples()
        {
            return new GameRequest
            {
                Title = "Rio 2016 Final",
                Comment = "Very hard game",
                HomeTeamId = null,
                GuestTeamId = null
            };
        }
    }
}
