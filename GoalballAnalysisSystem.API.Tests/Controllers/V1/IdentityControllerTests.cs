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
        /*
        [Test]
        public async Task Register_WithValidEmailUsernameAndPassword_ReturnsOk()
        {
            // Arrange
            string expectedEmail = "TestEmail";
            string expectedUsername = "TestUsername";
            string password = "TestPassword";

            var request = new RegistrationRequest
            {
                Email = expectedEmail,
                Password = password,
                UserName = expectedUsername

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.RegisterAsync(expectedEmail, password, expectedUsername)).ReturnsAsync(new AuthenticationResult() { Success = true });
            
            var identityController = new IdentityController(mock.Object);
            // Act
            var actionResult = await identityController.Register(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task Register_WithNotValidEmailUsernameAndPassword_ReturnsBadRequest()
        {
            // Arrange
            string expectedEmail = "TestEmail";
            string expectedUsername = "TestUsername";
            string password = "TestPassword";

            var request = new RegistrationRequest
            {
                Email = expectedEmail,
                Password = password,
                UserName = expectedUsername

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.RegisterAsync(expectedEmail, password, expectedUsername)).ReturnsAsync(new AuthenticationResult() { Success = false, Errors = new List<string>() { "error" } });

            var identityController = new IdentityController(mock.Object);
            // Act
            var actionResult = await identityController.Register(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task Login_WithValidEmailUsernameAndPassword_ReturnsOk()
        {
            // Arrange
            string expectedEmail = "TestEmail";
            string password = "TestPassword";
            string expectedUsername = "TestUsername";

            var request = new RegistrationRequest
            {
                Email = expectedEmail,
                Password = password,
                UserName = expectedUsername

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.LoginAsync(expectedEmail, password)).ReturnsAsync(new AuthenticationResult() { Success = true });

            var identityController = new IdentityController(mock.Object);

            // Act
            var actionResult = await identityController.Login(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task Login_WithNotValidEmailUsernameAndPassword_ReturnsBadRequest()
        {
            // Arrange
            string expectedEmail = "TestEmail";
            string password = "TestPassword";
            string expectedUsername = "TestUsername";

            var request = new RegistrationRequest
            {
                Email = expectedEmail,
                Password = password,
                UserName = expectedUsername

            };

            Mock<IIdentityService> mock = new Mock<IIdentityService>();
            mock.Setup(s => s.LoginAsync(expectedEmail, password)).ReturnsAsync(new AuthenticationResult() { Success = false, Errors = new List<string>() { "error" } });

            var identityController = new IdentityController(mock.Object);

            // Act
            var actionResult = await identityController.Login(request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task RefreshTokens_WithValidEmailUsernameAndPassword_ReturnsOk()
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
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task RefreshTokens_WithNotValidEmailUsernameAndPassword_ReturnsBadRequest()
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
            this.mockRepository.VerifyAll();
        }
        */
    }
}
