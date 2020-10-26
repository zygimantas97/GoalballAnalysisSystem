using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Responses
{
    public class ProjectionResponse
    {
        public long Id { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public double Speed { get; set; }
        public int GameId { get; set; }
        public int? GamePlayerId { get; set; }
    }
}
