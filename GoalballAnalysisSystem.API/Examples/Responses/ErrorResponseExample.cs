using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class ErrorResponseExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples()
        {
            return new ErrorResponse
            {
                Errors = new List<ErrorModel> { new ErrorModel { Message = "Error message" } }
            };
        }
    }
}
