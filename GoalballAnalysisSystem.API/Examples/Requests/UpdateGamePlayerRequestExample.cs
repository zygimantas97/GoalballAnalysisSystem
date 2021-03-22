using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Requests
{
    public class UpdateGamePlayerRequestExample : IExamplesProvider<UpdateGamePlayerRequest>
    {
        public UpdateGamePlayerRequest GetExamples()
        {
            return new UpdateGamePlayerRequest
            {
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now
            };
        }
    }
}
