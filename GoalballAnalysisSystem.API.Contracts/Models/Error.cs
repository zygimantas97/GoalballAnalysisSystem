using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.Models
{
    public class Error
    {
        //[JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
