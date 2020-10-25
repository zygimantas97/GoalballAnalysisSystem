using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Requests
{
    public class TeamPlayerRequest
    {
        public int Number { get; set; }
        public long? RoleId { get; set; }
    }
}
