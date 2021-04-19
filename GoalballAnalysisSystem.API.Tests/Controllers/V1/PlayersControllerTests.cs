using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Mapping;
using GoalballAnalysisSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class PlayersControllerTests : ControllerTestBase
    {
        [Test]
        public async Task GetPlayers_With5UserPlayers_ReturnsListOf5Players()
        {
            // Arrange
            var countOfPlayers = 5;
            for (int i = 0; i < countOfPlayers; i++)
            {
                _context.Players.Add(new Player
                {
                    IdentityUserId = "test_user"
                });
            }
            await _context.SaveChangesAsync();
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.GetPlayers();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<PlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfPlayers, (objectResult.Value as List<PlayerResponse>).Count);
        }

        [Test]
        public async Task GetPlayers_WithNoUserPlayers_ReturnsEmptyListOfPlayers()
        {
            // Arrange
            var countOfPlayers = 0;
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.GetPlayers();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<PlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfPlayers, (objectResult.Value as List<PlayerResponse>).Count);
        }

        [Test]
        public async Task GetPlayer_WithExistingPlayerId_ReturnsPlayer()
        {
            // Arrange
            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.GetPlayer(player.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<PlayerResponse>(objectResult.Value);
            Assert.AreEqual(player.Name, (objectResult.Value as PlayerResponse).Name);
        }

        [Test]
        public async Task GetPlayer_WithNotExistingPlayerId_ReturnsNotFound()
        {
            // Arrange
            var playerId = 1;
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.GetPlayer(playerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task UpdatePlayer_WithExistingPlayerId_ReturnsNoContent()
        {
            // Arrange
            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            var playerRequest = new PlayerRequest
            {
                Name = "Test Player update",
                Surname = "Test Player",
                Description = "Test Description",
                Country = "T",
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.UpdatePlayer(player.Id, playerRequest);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            var updatedPlayer = await _context.Players.SingleOrDefaultAsync(p => p.Id == player.Id);
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(204, statusCodeResult.StatusCode);
            Assert.AreEqual(playerRequest.Name, updatedPlayer.Name);
        }

        [Test]
        public async Task UpdatePlayer_WithNotExistingPlayerId_ReturnsNotFound()
        {
            // Arrange
            var playerId = 1;
            var playerRequest = new PlayerRequest
            {
                Name = "Test Player update",
                Surname = "Test Player",
                Description = "Test Description",
                Country = "T"
            };
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.UpdatePlayer(playerId, playerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task CreatePlayer_ReturnsCreatedPlayer()
        {
            // Arrange
            var playerRequest = new PlayerRequest
            {
                Name = "Test Player",
                Surname = "Test Player",
                Description = "Test Description",
                Country = "T"
            };
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.CreatePlayer(playerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<PlayerResponse>(objectResult.Value);
            Assert.AreEqual(playerRequest.Name, (objectResult.Value as PlayerResponse).Name);
            Assert.AreEqual(1, _context.Players.Count());
        }

        [Test]
        public async Task DeletePlayer_WithExistingPlayerId_ReturnsDeletedPlayer()
        {
            // Arrange
            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.DeletePlayer(player.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<PlayerResponse>(objectResult.Value);
            Assert.AreEqual(player.Name, (objectResult.Value as PlayerResponse).Name);
            Assert.AreEqual(0, _context.Players.Count());
        }

        [Test]
        public async Task DeletePlayer_WithNotExistingPlayerId_ReturnsNotFound()
        {
            // Arrange
            var playerId = 1;
            var playersController = CreateController<PlayersController>();

            // Act
            var actionResult = await playersController.DeletePlayer(playerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }
    }
}
