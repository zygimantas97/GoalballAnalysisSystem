using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Models
{
    public class GamePlayer
    {
        [Key]
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long TeamId { get; set; }
        public long PlayerId { get; set; }
        public long GameId { get; set; }

        public TeamPlayer TeamPlayer { get; set; }
        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }
        
        //public IEnumerable<Throw> Throws { get; set; }
    }
}
