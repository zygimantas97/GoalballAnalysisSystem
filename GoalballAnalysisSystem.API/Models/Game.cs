using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Models
{
    public class Game
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string IdentityUserId { get; set; }
        public long? HomeTeamId { get; set; }
        public long? GuestTeamId { get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser IdentityUser { get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; }
        [ForeignKey(nameof(GuestTeamId))]
        public Team GuestTeam { get; set; }

        public ICollection<Projection> Projections { get; set; }
        public ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
