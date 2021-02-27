using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Requests
{
    public class GameRequest
    {
        public string Title { get; set; }
        public string Comment { get; set; }
        public long? HomeTeamId { get; set; }
        public long? GuestTeamId { get; set; }
    }
}
