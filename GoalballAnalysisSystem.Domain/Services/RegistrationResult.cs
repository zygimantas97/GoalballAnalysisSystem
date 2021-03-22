using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Services
{
    public enum RegistrationResult
    {
        Success,
        PasswordsDoNotMatch,
        EmailAlreadyExists
    }
}
