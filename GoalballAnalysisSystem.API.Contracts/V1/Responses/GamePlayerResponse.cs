using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Responses
{
    public class GamePlayerResponse
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long TeamId { get; set; }
        public long PlayerId { get; set; }
        public long GameId { get; set; }
        public GameResponse Game { get; set; }
        public TeamPlayerResponse TeamPlayer { get; set; }
    }
}
