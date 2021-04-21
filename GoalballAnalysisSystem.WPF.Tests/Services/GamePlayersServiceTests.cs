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
    public class GamePlayersServiceTests
    {
        [Test]
        public async Task GetGamePlayersByGameAsyn_WithAuthentication_ReturnsListOfGamePlayers()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createGameResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var getGamePlayerResponse = await gamePlayersService.GetGamePlayersByGameAsync(createGameResponse.Id);

            // Assert
            Assert.NotNull(getGamePlayerResponse);
            Assert.IsInstanceOf<List<GamePlayerResponse>>(getGamePlayerResponse);
        }

        [Test]
        public async Task GetGamePlayersByGameAsyn_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var gamePlayersService = new GamePlayersService(identityService2);

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
            Assert.ThrowsAsync<Exception>(async () => await gamePlayersService.GetGamePlayersByGameAsync(createGameResponse.Id));
        }

        [Test]
        public async Task GetGamePlayersByTeamPlayerAsync_WithAuthentication_ReturnsListOfGamePlayers()
        {
            // Arrange
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);

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
            var getGamePlayerResponse = await gamePlayersService.GetGamePlayersByTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id);

            // Assert
            Assert.NotNull(getGamePlayerResponse);
            Assert.IsInstanceOf<List<GamePlayerResponse>>(getGamePlayerResponse);
        }

        [Test]
        public async Task GetGamePlayersByTeamPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService1);
            var gamePlayersService = new GamePlayersService(identityService2);

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
            var createTeamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id, new TeamPlayerRequest
            {
                RoleId = 1,
                Number = 1
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamePlayersService.GetGamePlayersByTeamPlayerAsync(createTeamResponse.Id, createPlayerResponse.Id));
        }

        [Test]
        public async Task GetGamePlayerAsync_WithAuthentication_ReturnsGamePlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);

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
            var getGamePlayerResponse = await gamePlayersService.GetGamePlayerAsync(createGamePlayerResponse.Id);

            // Assert
            Assert.NotNull(getGamePlayerResponse);
            Assert.IsInstanceOf<GamePlayerResponse>(getGamePlayerResponse);
        }

        [Test]
        public async Task GetGamePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService1);
            var gamePlayersService1 = new GamePlayersService(identityService1);
            var gamePlayersService2 = new GamePlayersService(identityService2);

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
            Assert.ThrowsAsync<Exception>(async () => await gamePlayersService2.GetGamePlayerAsync(createGamePlayerResponse.Id));
        }

        [Test]
        public async Task UpdateGamePlayerAsync_WithAuthentication_NotThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);

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
            await gamePlayersService.UpdateGamePlayerAsync(createGamePlayerResponse.Id, new UpdateGamePlayerRequest
            {
                StartTime = DateTime.Now.AddMinutes(-15),
                EndTime = DateTime.Now
            });

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task UpdateGamePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService1);
            var gamePlayersService1 = new GamePlayersService(identityService1);
            var gamePlayersService2 = new GamePlayersService(identityService2);

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
            Assert.ThrowsAsync<Exception>(async () => await gamePlayersService2.UpdateGamePlayerAsync(createGamePlayerResponse.Id, new UpdateGamePlayerRequest
            {
                StartTime = DateTime.Now.AddMinutes(-15),
                EndTime = DateTime.Now
            }));
        }

        [Test]
        public async Task CreateGamePlayerAsync_WithAuthentication_ReturnsGamePlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);

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

            // Assert
            Assert.NotNull(createGamePlayerResponse);
            Assert.IsInstanceOf<GamePlayerResponse>(createGamePlayerResponse);
        }

        [Test]
        public async Task CreateGamePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService1);
            var gamePlayersService = new GamePlayersService(identityService2);

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

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamePlayersService.CreateGamePlayerAsync(new CreateGamePlayerRequest
            {
                GameId = createGameResponse.Id,
                TeamId = createTeamResponse.Id,
                PlayerId = createPlayerResponse.Id,
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now
            }));
        }

        [Test]
        public async Task DeleteGamePlayerAsync_WithAuthentication_ReturnsGamePlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);

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
            var deleteGamePlayerResponse = await gamePlayersService.DeleteGamePlayerAsync(createGamePlayerResponse.Id);

            // Assert
            Assert.NotNull(deleteGamePlayerResponse);
            Assert.IsInstanceOf<GamePlayerResponse>(deleteGamePlayerResponse);
        }

        [Test]
        public async Task DeleteGamePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService = new GamesService(identityService1);
            var teamsService = new TeamsService(identityService1);
            var playersService = new PlayersService(identityService1);
            var teamPlayersService = new TeamPlayersService(identityService1);
            var gamePlayersService1 = new GamePlayersService(identityService1);
            var gamePlayersService2 = new GamePlayersService(identityService2);

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
            Assert.ThrowsAsync<Exception>(async () => await gamePlayersService2.DeleteGamePlayerAsync(createGamePlayerResponse.Id));
        }
    }
}
