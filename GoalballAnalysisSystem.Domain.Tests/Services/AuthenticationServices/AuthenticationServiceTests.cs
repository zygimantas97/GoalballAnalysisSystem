using GoalballAnalysisSystem.Domain.Exceptions;
using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.Domain.Tests.Services.AuthenticationServices
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        /*
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<IUserDataService> _mockUserDataService;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockUserDataService = new Mock<IUserDataService>();
            _authenticationService = new AuthenticationService(_mockUserDataService.Object, _mockPasswordHasher.Object);
        }

        [Test]
        public async Task Login_WithTheCorrectPasswordForExistingEmail_ReturnsUserForCorrectEmail()
        {
            //Arrange
            string expectedEmail = "TestEmail";
            string password = "TestPassword";
            _mockUserDataService.Setup(s => s.GetByEmail(expectedEmail)).ReturnsAsync(new User() { Email = expectedEmail, Role = 1});
            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Success);
            
            //Act
            User user = await _authenticationService.Login(expectedEmail, password);

            //Assert
            string actualEmail = user.Email;
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void Login_WithTheIncorrectPasswordForExistingEmail_ThrowsInvalidPasswordExceptionForEmail()
        {
            //Arrange
            string expectedEmail = "TestEmail";
            string password = "TestPassword";
            _mockUserDataService.Setup(s => s.GetByEmail(expectedEmail)).ReturnsAsync(new User() { Email = expectedEmail, Role = 1 });
            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);

            //Act
            InvalidPasswordException exception = Assert.ThrowsAsync<InvalidPasswordException>(() => _authenticationService.Login(expectedEmail, password));

            //Assert
            string actualEmail = exception.Email;
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public void Login_WithNonExistingEmail_ThrowsInvalidPasswordExceptionForEmail()
        {
            //Arrange
            string expectedEmail = "TestEmail";
            string password = "TestPassword";
            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);

            //Act
            UserNotFoundException exception = Assert.ThrowsAsync<UserNotFoundException>(() => _authenticationService.Login(expectedEmail, password));

            //Assert
            string actualEmail = exception.Email;
            Assert.AreEqual(expectedEmail, actualEmail);
        }

        [Test]
        public async Task Register_WithPasswordsNotMatching_ReturnsPasswordsDoNotMatch()
        {
            string password = "password";
            string confirmPassword = "confirmPassword";
            RegistrationResult expectedResult = RegistrationResult.PasswordsDoNotMatch;

            RegistrationResult actualResult = await _authenticationService.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), password, confirmPassword);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task Register_WithAlreadyExistingEmail_ReturnsEmailAlreadyExists()
        {
            string email = "email";
            string password = "password";
            RegistrationResult expectedResult = RegistrationResult.EmailAlreadyExists;
            _mockUserDataService.Setup(s => s.GetByEmail(email)).ReturnsAsync(new User());

            RegistrationResult actualResult = await _authenticationService.Register(It.IsAny<string>(), It.IsAny<string>(), email, password, password);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task Register_WithNonExistingEmailAndMatchingPasswords_ReturnsSuccess()
        {
            string password = "password";
            RegistrationResult expectedResult = RegistrationResult.Success;

            RegistrationResult actualResult = await _authenticationService.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), password, password);

            Assert.AreEqual(expectedResult, actualResult);
        }
        */
    }
}
