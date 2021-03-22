using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class TeamPlayer
    {
        public int Number { get; set; }
        public int? Role { get; set; }
        public int Team { get; set; }
        public int Player { get; set; }

        public PlayerRole RoleNavigation { get; set; }
        public Team TeamNavigation { get; set; }
        public Player PlayerNavigation { get; set; }
        public IEnumerable<GamePlayer> GamePlayers { get; set; }
    }
}
