using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Responses
{
    public class TeamResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public IEnumerable<TeamPlayerResponse> TeamPlayers { get; set; }
    }
}
