using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int User { get; set; }
        public int? Team1 { get; set; }
        public int? Team2 { get; set; }

        public User UserNavigation { get; set; }
        public Team Team1Navigation { get; set; }
        public Team Team2Navigation { get; set; }
        public IEnumerable<Throw> Throws { get; set; }
        public IEnumerable<GamePlayer> GamePlayers { get; set; }
    }
}
