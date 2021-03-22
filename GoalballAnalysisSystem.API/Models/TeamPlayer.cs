using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Models
{
    public class TeamPlayer
    {
        [Key]
        public long TeamId { get; set; }
        [Key]
        public long PlayerId { get; set; }
        public int Number { get; set; }
        public long? RoleId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        [ForeignKey(nameof(RoleId))]
        public PlayerRole Role { get; set; }
        
        public ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
