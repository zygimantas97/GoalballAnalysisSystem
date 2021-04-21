using GoalballAnalysisSystem.API.Contracts.V1.Requests;
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
    public class TeamsServiceTests
    {
        [Test]
        public async Task GetTeamsAsync_WithAuthentication_ReturnsListOfTeams()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var getResponse = await teamsService.GetTeamsAsync();

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<List<TeamResponse>>(getResponse);
        }

        [Test]
        public async Task GetTeamsAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamsService.GetTeamsAsync());
        }

        [Test]
        public async Task GetTeamAsync_WithAuthentication_ReturnsTeam()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var getResponse = await teamsService.GetTeamAsync(createResponse.Id);

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<TeamResponse>(getResponse);
        }

        [Test]
        public async Task GetTeamAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService1 = new TeamsService(identityService1);
            var teamsService2 = new TeamsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService1.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamsService2.GetTeamAsync(createResponse.Id));
        }

        [Test]
        public async Task UpdateTeamAsync_WithAuthentication_NotThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            await teamsService.UpdateTeamAsync(createResponse.Id, new TeamRequest
            {
                Name = "Test update",
                Description = "Description update",
                Country = "T update"
            });

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task UpdateTeamAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService1 = new TeamsService(identityService1);
            var teamsService2 = new TeamsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService1.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamsService2.UpdateTeamAsync(createResponse.Id, new TeamRequest
            {
                Name = "Test update",
                Description = "Description update",
                Country = "T update"
            }));
        }

        [Test]
        public async Task CreateTeamAsync_WithAuthentication_ReturnsTeam()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.NotNull(createResponse);
            Assert.IsInstanceOf<TeamResponse>(createResponse);
        }

        [Test]
        public async Task CreateTeamAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            }));
        }

        [Test]
        public async Task DeleteTeamAsync_WithAuthentication_ReturnsTeam()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var deleteResponse = await teamsService.DeleteTeamAsync(createResponse.Id);

            // Assert
            Assert.NotNull(deleteResponse);
            Assert.IsInstanceOf<TeamResponse>(deleteResponse);
        }

        [Test]
        public async Task DeleteTeamAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService1 = new TeamsService(identityService1);
            var teamsService2 = new TeamsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await teamsService1.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamsService2.DeleteTeamAsync(createResponse.Id));
        }
    }
}
