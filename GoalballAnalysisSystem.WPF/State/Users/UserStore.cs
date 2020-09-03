using GoalballAnalysisSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Users
{
    public class UserStore : IUserStore
    {
        public User CurrentUser { get; set; }
    }
}
