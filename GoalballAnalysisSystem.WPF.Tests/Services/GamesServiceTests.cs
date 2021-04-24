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
    public class GamesServiceTests
    {
        [Test]
        public async Task GetGamesAsync_WithAuthentication_ReturnsListOfGames()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var getResponse = await gamesService.GetGamesAsync();

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<List<GameResponse>>(getResponse);
        }

        [Test]
        public async Task GetGamesAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamesService.GetGamesAsync());
        }

        [Test]
        public async Task GetGameAsync_WithAuthentication_ReturnsGame()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });
            var getResponse = await gamesService.GetGameAsync(createResponse.Id);

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<GameResponse>(getResponse);
        }

        [Test]
        public async Task GetGameAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService1 = new GamesService(identityService1);
            var gamesService2 = new GamesService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService1.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamesService2.GetGameAsync(createResponse.Id));
        }

        [Test]
        public async Task UpdateGameAsync_WithAuthentication_NotThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService.CreateGameAsync(new GameRequest
            {
               Title = "Test",
               Comment = "Test Comment",
               HomeTeamId = null,
               GuestTeamId = null
            });
            await gamesService.UpdateGameAsync(createResponse.Id, new GameRequest
            {
                Title = "Test update",
                Comment = "Test Comment update",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task UpdateGameAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService1 = new GamesService(identityService1);
            var gamesService2 = new GamesService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService1.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamesService2.UpdateGameAsync(createResponse.Id, new GameRequest
            {
               Title = "Test update",
               Comment = "Test Comment update",
               HomeTeamId = null,
               GuestTeamId = null
            }));
        }

        [Test]
        public async Task CreateGameAsync_WithAuthentication_ReturnsGame()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.NotNull(createResponse);
            Assert.IsInstanceOf<GameResponse>(createResponse);
        }

        [Test]
        public async Task CreateGameAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamesService.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            }));
        }

        [Test]
        public async Task DeleteGameAsync_WithAuthentication_ReturnsGame()
        {
            // Arrange
            var identityService = new IdentityService();
            var gamesService = new GamesService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService.CreateGameAsync(new GameRequest
            {
               Title = "Test",
               Comment = "Test Comment",
               HomeTeamId = null,
               GuestTeamId = null
            });
            var deleteResponse = await gamesService.DeleteGameAsync(createResponse.Id);

            // Assert
            Assert.NotNull(deleteResponse);
            Assert.IsInstanceOf<GameResponse>(deleteResponse);
        }

        [Test]
        public async Task DeleteGameAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var gamesService1 = new GamesService(identityService1);
            var gamesService2 = new GamesService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await gamesService1.CreateGameAsync(new GameRequest
            {
                Title = "Test",
                Comment = "Test Comment",
                HomeTeamId = null,
                GuestTeamId = null
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await gamesService2.DeleteGameAsync(createResponse.Id));
        }
    }
}
