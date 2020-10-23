using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Models
{
    public class Team
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string IdentityUserId { get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        public IdentityUser IdentityUser { get; set; }
        //public IEnumerable<Game> Games1 { get; set; }
        //public IEnumerable<Game> Games2 { get; set; }
        //public IEnumerable<TeamPlayer> TeamPlayers { get; set; }
    }
}
