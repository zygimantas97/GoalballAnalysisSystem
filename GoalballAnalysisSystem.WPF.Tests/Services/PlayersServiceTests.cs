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
    public class PlayersServiceTests
    {
        [Test]
        public async Task GetPlayersAsync_WithAuthentication_ReturnsListOfPlayers()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var getResponse = await playersService.GetPlayersAsync();

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<List<PlayerResponse>>(getResponse);
        }

        [Test]
        public async Task GetPlayersAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await playersService.GetPlayersAsync());
        }

        [Test]
        public async Task GetPlayerAsync_WithAuthentication_ReturnsPlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var getResponse = await playersService.GetPlayerAsync(createResponse.Id);

            // Assert
            Assert.NotNull(getResponse);
            Assert.IsInstanceOf<PlayerResponse>(getResponse);
        }

        [Test]
        public async Task GetPlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var playersService1 = new PlayersService(identityService1);
            var playersService2 = new PlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService1.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await playersService2.GetPlayerAsync(createResponse.Id));
        }

        [Test]
        public async Task UpdatePlayerAsync_WithAuthentication_NotThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            await playersService.UpdatePlayerAsync(createResponse.Id, new PlayerRequest
            {
                Name = "Test update",
                Description = "Description update",
                Country = "T update"
            });

            // Assert
            Assert.Pass();
        }

        [Test]
        public async Task UpdatePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var playersService1 = new PlayersService(identityService1);
            var playersService2 = new PlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService1.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await playersService2.UpdatePlayerAsync(createResponse.Id, new PlayerRequest
            {
                Name = "Test update",
                Description = "Description update",
                Country = "T update"
            }));
        }

        [Test]
        public async Task CreatePlayerAsync_WithAuthentication_ReturnsPlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.NotNull(createResponse);
            Assert.IsInstanceOf<PlayerResponse>(createResponse);
        }

        [Test]
        public async Task CreatePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            }));
        }

        [Test]
        public async Task DeletePlayerAsync_WithAuthentication_ReturnsPlayer()
        {
            // Arrange
            var identityService = new IdentityService();
            var playersService = new PlayersService(identityService);

            // Act
            await identityService.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });
            var deleteResponse = await playersService.DeletePlayerAsync(createResponse.Id);

            // Assert
            Assert.NotNull(deleteResponse);
            Assert.IsInstanceOf<PlayerResponse>(deleteResponse);
        }

        [Test]
        public async Task DeletePlayerAsync_WithoutAuthentication_ThrowsException()
        {
            // Arrange
            var identityService1 = new IdentityService();
            var identityService2 = new IdentityService();
            var playersService1 = new PlayersService(identityService1);
            var playersService2 = new PlayersService(identityService2);

            // Act
            await identityService1.LoginAsync(Credentials.testEmail, Credentials.testPassword);
            var createResponse = await playersService1.CreatePlayerAsync(new PlayerRequest
            {
                Name = "Test",
                Description = "Test Description",
                Country = "T"
            });

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await playersService2.DeletePlayerAsync(createResponse.Id));
        }
    }
}
