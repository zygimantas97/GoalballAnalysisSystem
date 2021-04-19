using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Models;
using GoalballAnalysisSystem.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class IdentityControllerTests : ControllerTestBase
    {
        [Test]
        public async Task Register_WithValidEmailUsernameAndPassword_ReturnsOk()
        {
            // Arrange
            string email = "TestEmail";
            string username = "TestUsername";
            string password = "TestPassword";

            var request = new RegistrationRequest
            {
                Email = email,
                Password = password,
                UserName = username

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.RegisterAsync(email, password, username)).ReturnsAsync(new AuthenticationResult() { Success = true });
            
            var identityController = new IdentityController(mock.Object);
            
            // Act
            var actionResult = await identityController.Register(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [Test]
        public async Task Register_WithNotValidEmailUsernameAndPassword_ReturnsBadRequest()
        {
            // Arrange
            string email = "TestEmail";
            string username = "TestUsername";
            string password = "TestPassword";

            var request = new RegistrationRequest
            {
                Email = email,
                Password = password,
                UserName = username

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.RegisterAsync(email, password, username)).ReturnsAsync(new AuthenticationResult() { Success = false, Errors = new List<string>() { "error" } });

            var identityController = new IdentityController(mock.Object);
            
            // Act
            var actionResult = await identityController.Register(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [Test]
        public async Task Login_WithValidEmailUsernameAndPassword_ReturnsOk()
        {
            // Arrange
            string email = "TestEmail";
            string password = "TestPassword";

            var request = new LoginRequest
            {
                Email = email,
                Password = password,
            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.LoginAsync(email, password)).ReturnsAsync(new AuthenticationResult() { Success = true });

            var identityController = new IdentityController(mock.Object);

            // Act
            var actionResult = await identityController.Login(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [Test]
        public async Task Login_WithNotValidEmailUsernameAndPassword_ReturnsBadRequest()
        {
            // Arrange
            string email = "TestEmail";
            string password = "TestPassword";

            var request = new LoginRequest
            {
                Email = email,
                Password = password,
            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.LoginAsync(email, password)).ReturnsAsync(new AuthenticationResult() { Success = false, Errors = new List<string>() { "error" } });

            var identityController = new IdentityController(mock.Object);

            // Act
            var actionResult = await identityController.Login(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [Test]
        public async Task RefreshToken_WithValidTokens_ReturnsOk()
        {
            // Arrange
            string token = "TestToken";
            string refreshToken = "TestTokenRefresh";

            var request = new RefreshTokenRequest
            {
                Token = token,
                RefreshToken = refreshToken

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.RefreshTokenAsync(token, refreshToken)).ReturnsAsync(new AuthenticationResult() { Success = true });
            
            var identityController = new IdentityController(mock.Object);

            // Act
            var actionResult = await identityController.RefreshToken(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [Test]
        public async Task RefreshTokens_WithNotValidTokens_ReturnsBadRequest()
        {
            // Arrange
            string token = "TestToken";
            string refreshToken = "TestTokenRefresh";

            var request = new RefreshTokenRequest
            {
                Token = token,
                RefreshToken = refreshToken
            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.RefreshTokenAsync(token, refreshToken)).ReturnsAsync(new AuthenticationResult() { Success = false, Errors = new List<string>() { "error" } });

            var identityController = new IdentityController(mock.Object);

            // Act
            var actionResult = await identityController.RefreshToken(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }
    }
}
