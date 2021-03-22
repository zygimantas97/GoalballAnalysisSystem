using GoalballAnalysisSystem.API.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Responses
{
    public class ErrorResponse
    {
        //[JsonPropertyName("errors")]
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
