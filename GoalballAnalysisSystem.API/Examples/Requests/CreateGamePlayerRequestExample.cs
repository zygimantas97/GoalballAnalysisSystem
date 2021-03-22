using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class CreateGamePlayerRequestExample : IExamplesProvider<CreateGamePlayerRequest>
    {
        public CreateGamePlayerRequest GetExamples()
        {
            return new CreateGamePlayerRequest
            {
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now,
                TeamId = 1,
                PlayerId = 1,
                GameId = 1
            };
        }
    }
}
