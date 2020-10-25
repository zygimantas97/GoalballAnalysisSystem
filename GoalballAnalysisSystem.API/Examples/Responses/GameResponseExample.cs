using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class GameResponseExample : IExamplesProvider<GameResponse>
    {
        public GameResponse GetExamples()
        {
            return new GameResponse
            {
                Id = 1,
                Title = "Rio 2016 Final",
                Date = DateTime.Now,
                Comment = "Very hard game",
                HomeTeamId = null,
                GuestTeamId = null
            };
        }
    }
}
