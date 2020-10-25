using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class GamePlayerResponseExample : IExamplesProvider<GamePlayerResponse>
    {
        public GamePlayerResponse GetExamples()
        {
            return new GamePlayerResponse
            {
                Id = 1,
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now,
                TeamId = 1,
                PlayerId = 1,
                GameId = 1
            };
        }
    }
}
