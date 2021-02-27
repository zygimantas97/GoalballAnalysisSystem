using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Requests
{
    public class CreateGamePlayerRequest
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long TeamId { get; set; }
        public long PlayerId { get; set; }
        public long GameId { get; set; }
    }
}
