using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class GamePlayer
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team { get; set; }
        public int Player { get; set; }
        public int Game { get; set; }

        public TeamPlayer TeamPlayerNavigation { get; set; }
        public Game GameNavigation { get; set; }
        public IEnumerable<Throw> Throws { get; set; }
    }
}
