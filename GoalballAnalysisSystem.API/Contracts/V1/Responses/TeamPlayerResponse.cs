using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Responses
{
    public class TeamPlayerResponse
    {
        public long TeamId { get; set; }
        public long PlayerId { get; set; }
        public int Number { get; set; }
        public long? RoleId { get; set; }
    }
}
