using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class ProjectionRequestExample : IExamplesProvider<ProjectionRequest>
    {
        public ProjectionRequest GetExamples()
        {
            return new ProjectionRequest
            {
                X1 = 1,
                Y1 = 1,
                X2 = 2,
                Y2 = 2,
                Speed = 0,
                GameId = 1,
                GamePlayerId = 1
            };
        }
    }
}
