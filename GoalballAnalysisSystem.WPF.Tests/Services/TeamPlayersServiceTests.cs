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
    public class TeamPlayersServiceTests
    {
        [Test]
        public async Task GetTeamPlayersByTeamAsync_WithAuthentication_ReturnsListOfTeamPlayers()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var getTeamPlayerResponse = await teamPlayersService.GetTeamPlayersByTeamAsync(createTeamResponse.Id);

            // Assert
            Assert.NotNull(getTeamPlayerResponse);
            Assert.IsInstanceOf<List<TeamPlayerResponse>>(getTeamPlayerResponse);
        }

        [Test]
        public async Task GetTeamPlayersByTeamAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService = new TeamsService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamPlayersService.GetTeamPlayersByTeamAsync(createTeamResponse.Id));
        }

        [Test]
        public async Task GetTeamPlayersByPlayerAsync_WithAuthentication_ReturnsListOfTeamPlayers()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var getTeamPlayerResponse = await teamPlayersService.GetTeamPlayersByPlayerAsync(createPlayerResponse.Id);

            // Assert
            Assert.NotNull(getTeamPlayerResponse);
            Assert.IsInstanceOf<List<TeamPlayerResponse>>(getTeamPlayerResponse);
        }

        [Test]
        public async Task GetTeamPlayersByPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamPlayersService.GetTeamPlayersByPlayerAsync(createPlayerResponse.Id));
        }

        [Test]
        public async Task GetTeamPlayerAsync_WithAuthentication_ReturnsTeamPlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });
            var getTeamPlayerResponse = await teamPlayersService.GetTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id);

            // Assert
            Assert.NotNull(getTeamPlayerResponse);
            Assert.IsInstanceOf<TeamPlayerResponse>(getTeamPlayerResponse);
        }

        [Test]
        public async Task GetTeamPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService1 = new TeamPlayersService(identityService1);
            var teamPlayersService2 = new TeamPlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService1.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamPlayersService2.GetTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id));
        }

        [Test]
        public async Task UpdateTeamPlayerAsync_WithAuthentication_NotThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });
            await teamPlayersService.UpdateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 2
            });

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task UpdateTeamPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService1 = new TeamPlayersService(identityService1);
            var teamPlayersService2 = new TeamPlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService1.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamPlayersService2.UpdateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            }));
        }

        [Test]
        public async Task CreateTeamPlayerAsync_WithAuthentication_ReturnsTeamPlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });

            // Assert
            Assert.NotNull(createTeamPlayerResponse);
            Assert.IsInstanceOf<TeamPlayerResponse>(createTeamPlayerResponse);
        }

        [Test]
        public async Task CreateTeamPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamPlayersService.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            }));
        }

        [Test]
        public async Task DeleteTeamPlayerAsync_WithAuthentication_ReturnsTeamPlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });
            var deleteTeamPlayerResponse = await teamPlayersService.DeleteTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id);

            // Assert
            Assert.NotNull(deleteTeamPlayerResponse);
            Assert.IsInstanceOf<TeamPlayerResponse>(deleteTeamPlayerResponse);
        }

        [Test]
        public async Task DeleteTeamPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService1 = new TeamPlayersService(identityService1);
            var teamPlayersService2 = new TeamPlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createTeamResponse = await teamsService.CreateTeamAsync(new TeamRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createPlayerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var createTeamPlayerResponse = await teamPlayersService1.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await teamPlayersService2.DeleteTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id));
        }
    }
}
