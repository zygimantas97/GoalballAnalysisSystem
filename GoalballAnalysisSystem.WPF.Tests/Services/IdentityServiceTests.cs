using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.WPF.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Tests.Services
{
    [TestFixture]
    public class IdentityServiceTests
    {
        [Test]
        public async Task RegisterAsync_WithCorrectCredentials_ReturnsAuthenticationResponse()
        {
            // Arrange
            var identityService = new IdentityService();
            var id = Guid.NewGuid().ToString();

            // Act
            var response = await identityService.RegisterAsync(id + "Test", id + "test@gas.com", "Password123!");

            // Assert
            Assert.IsInstanceOf<AuthenticationResponse>(response);
        }

        [Test]
        public async Task RegisterAsync_WithWrongCredentials_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var id = Guid.NewGuid().ToString();

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await identityService.RegisterAsync(id + "Test", id + "test@gas.com", "password"));
        }

        [Test]
        public async Task LoginAsync_WithCorrectCredentials_ReturnsAuthenticationResponse()
        {
            // Arrange
            var identityService = new IdentityService();

            // Act
            var response = await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);

            // Assert
            Assert.IsInstanceOf<AuthenticationResponse>(response);
        }

        [Test]
        public async Task LoginAsync_WithWrongCredentials_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await identityService.LoginAsync(Credentials.testEmail, "password"));
        }
    }
}
