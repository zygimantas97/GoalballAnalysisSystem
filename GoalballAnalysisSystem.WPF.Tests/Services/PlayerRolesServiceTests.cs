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
    public class PlayerRolesServiceTests
    {
        [Test]
        public async Task GetPlayerRolesAsync_WithAuthentication_ReturnsListOfPlayerRoles()
        {
            // Arrange
            var identityService = new IdentityService();
            var playerRolesService = new PlayerRolesService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var getResponse = await playerRolesService.GetPlayerRolesAsync();

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<List<PlayerRoleResponse>>(getResponse);
        }

        [Test]
        public async Task GetPlayerRolesAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var playerRolesService = new PlayerRolesService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await playerRolesService.GetPlayerRolesAsync());
        }
    }
}
