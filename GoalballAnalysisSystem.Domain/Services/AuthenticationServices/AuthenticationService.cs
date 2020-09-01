using GoalballAnalysisSystem.Domain.Exceptions;
using GoalballAnalysisSystem.Domain.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserDataService _userDataService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IUserDataService userDataService, IPasswordHasher passwordHasher)
        {
            _userDataService = userDataService;
            _passwordHasher = passwordHasher;
        } 

        public async Task<User> Login(string email, string password)
        {
            User storedUser = await _userDataService.GetByEmail(email);
            if(storedUser == null)
            {
                throw new UserNotFoundException(email);
            }
            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedUser.PasswordHash, password);
            if(passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(email, password);
            }
            return storedUser;
        }

        public async Task<RegistrationResult> Register(string name, string surname, string email, string password, string confirmPassword)
        {
            RegistrationResult result = RegistrationResult.Success;

            if(password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            User emailUser = await _userDataService.GetByEmail(email);
            if(emailUser != null)
            {
                result = RegistrationResult.EmailAlreadyExists;
            }

            if(result == RegistrationResult.Success)
            {
                string hashedPassword = _passwordHasher.HashPassword(password);
                User user = new User()
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    PasswordHash = hashedPassword,
                    Role = 1
                };
                await _userDataService.Create(user);
            }

            return result;
        }
    }
}
