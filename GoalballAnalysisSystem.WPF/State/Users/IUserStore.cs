using GoalballAnalysisSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Users
{
    public interface IUserStore
    {
        User CurrentUser { get; set; }
        event Action StateChanged;
    }
}
