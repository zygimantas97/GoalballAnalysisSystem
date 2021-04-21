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
    public class ProjectionsServiceTests
    {
        [Test]
        public async Task GetProjectionsByGameAsync_WithAuthentication_ReturnsListOfProjections()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var projectionsService = new ProjectionsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var getProjectionResponse = await projectionsService.GetProjectionsByGameAsync(createGameResponse.Id);

            // Assert
            Assert.NotNull(getProjectionResponse);
            Assert.IsInstanceOf<List<ProjectionResponse>>(getProjectionResponse);
        }

        [Test]
        public async Task GetProjectionsByGameAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var projectionsService = new ProjectionsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await projectionsService.GetProjectionsByGameAsync(createGameResponse.Id));
        }

        [Test]
        public async Task GetProjectionsByGamePlayerAsync_WithAuthentication_ReturnsListOfProjections()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);
            var projectionsService = new ProjectionsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
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
            var createGamePlayerResponse = await gamePlayersService.CreateGamePlayerAsync(new CreateGamePlayerRequest
            {
                GameId = createGameResponse.Id,
                TeamId = createTeamResponse.Id,
                PlayerId = createPlayerResponse.Id,
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now
            });
            var getProjectionResponse = await projectionsService.GetProjectionsByGamePlayerAsync(createGamePlayerResponse.Id);

            // Assert
            Assert.NotNull(getProjectionResponse);
            Assert.IsInstanceOf<List<ProjectionResponse>>(getProjectionResponse);
        }

        [Test]
        public async Task GetProjectionsByGamePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService1);
            var gamePlayersService1 = new GamePlayersService(identityService1);
            var projectionsService = new ProjectionsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
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
            var createGamePlayerResponse = await gamePlayersService1.CreateGamePlayerAsync(new CreateGamePlayerRequest
            {
                GameId = createGameResponse.Id,
                TeamId = createTeamResponse.Id,
                PlayerId = createPlayerResponse.Id,
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await projectionsService.GetProjectionsByGamePlayerAsync(createGamePlayerResponse.Id));
        }

        [Test]
        public async Task GetProjectionAsync_WithAuthentication_ReturnsProjection()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var projectionsService = new ProjectionsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });
            var getProjectionResponse = await projectionsService.GetProjectionAsync(createProjectionResponse.Id);

            // Assert
            Assert.NotNull(getProjectionResponse);
            Assert.IsInstanceOf<ProjectionResponse>(getProjectionResponse);
        }

        [Test]
        public async Task GetProjectionAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var projectionsService1 = new ProjectionsService(identityService1);
            var projectionsService2 = new ProjectionsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService1.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await projectionsService2.GetProjectionAsync(createProjectionResponse.Id));
        }

        [Test]
        public async Task UpdateProjectionAsync_WithAuthentication_NotThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var projectionsService = new ProjectionsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });
            await projectionsService.UpdateProjectionAsync(createProjectionResponse.Id, new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 10,
                Y2 = 10,
                Speed = 10,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task UpdateProjectionAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var projectionsService1 = new ProjectionsService(identityService1);
            var projectionsService2 = new ProjectionsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService1.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await projectionsService2.UpdateProjectionAsync(createProjectionResponse.Id, new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 10,
                Y2 = 10,
                Speed = 10,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            }));
        }

        [Test]
        public async Task CreateProjectionAsync_WithAuthentication_ReturnsProjection()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var projectionsService = new ProjectionsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });

            // Assert
            Assert.NotNull(createProjectionResponse);
            Assert.IsInstanceOf<ProjectionResponse>(createProjectionResponse);
        }

        [Test]
        public async Task CreateProjectionAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var projectionsService = new ProjectionsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await projectionsService.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            }));
        }

        [Test]
        public async Task DeleteProjectionAsync_WithAuthentication_ReturnsProjection()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var projectionsService = new ProjectionsService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });
            var deleteProjectionResponse = await projectionsService.DeleteProjectionAsync(createProjectionResponse.Id);

            // Assert
            Assert.NotNull(deleteProjectionResponse);
            Assert.IsInstanceOf<ProjectionResponse>(deleteProjectionResponse);
        }

        [Test]
        public async Task DeleteProjectionAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var projectionsService1 = new ProjectionsService(identityService1);
            var projectionsService2 = new ProjectionsService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var createProjectionResponse = await projectionsService1.CreateProjectionAsync(new ProjectionRequest
            {
                GameId = createGameResponse.Id,
                X1 = 0,
                Y1 = 0,
                X2 = 1,
                Y2 = 1,
                Speed = 0,
                OffenseGamePlayerId = null,
                DefenseGamePlayerId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await projectionsService2.DeleteProjectionAsync(createProjectionResponse.Id));
        }
    }
}
