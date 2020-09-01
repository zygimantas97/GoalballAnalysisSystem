using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string Email { get; set; }

        public UserNotFoundException(string email)
        {
            Email = email;
        }

        public UserNotFoundException(string message, string email) : base(message)
        {
            Email = email;
        }

        public UserNotFoundException(string message, Exception innerException, string email) : base(message, innerException)
        {
            Email = email;
        }
    }
}
