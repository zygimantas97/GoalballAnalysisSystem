using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int User { get; set; }

        public User UserNavigation { get; set; }
        public IEnumerable<Game> Games1 { get; set; }
        public IEnumerable<Game> Games2 { get; set; }
        public IEnumerable<TeamPlayer> TeamPlayers { get; set; }
    }
}
