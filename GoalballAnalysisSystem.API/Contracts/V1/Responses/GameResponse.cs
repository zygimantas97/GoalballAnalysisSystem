using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Responses
{
    public class GameResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public long? HomeTeamId { get; set; }
        public long? GuestTeamId { get; set; }
    }
}
