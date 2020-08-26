using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class PlayerRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TeamPlayer> TeamPlayers { get; set; }
    }
}
