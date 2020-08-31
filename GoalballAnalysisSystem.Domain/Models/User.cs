using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime SubscriptionExpire { get; set; }
        public int Role { get; set; }

        public UserRole RoleNavigation { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Game> Games { get; set; }

    }
}
