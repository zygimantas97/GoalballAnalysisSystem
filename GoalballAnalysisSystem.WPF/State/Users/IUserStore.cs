using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Users
{
    public interface IUserStore
    {
        AuthenticationResponse CurrentUser { get; set; }
        event Action StateChanged;
    }
}
