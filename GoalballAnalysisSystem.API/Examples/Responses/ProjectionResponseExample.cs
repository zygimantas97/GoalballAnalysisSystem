using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class ProjectionResponseExample : IExamplesProvider<ProjectionResponse>
    {
        public ProjectionResponse GetExamples()
        {
            return new ProjectionResponse
            {
                Id = 1,
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
