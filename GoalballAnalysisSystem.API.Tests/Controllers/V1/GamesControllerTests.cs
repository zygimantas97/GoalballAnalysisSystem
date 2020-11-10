using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
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
    public class GamesControllerTests : ControllerTestBase
    {
        [Test]
        public async Task GetGames_With5UserGames_ReturnsListOf5Games()
        {
            // Arrange
            var countOfGames = 5;
            for (int i = 0; i < countOfGames; i++)
            {
                _context.Games.Add(new Game
                {
                    IdentityUserId = "test_user"
                });
            }
            await _context.SaveChangesAsync();
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.GetGames();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<GameResponse>>(objectResult.Value);
            Assert.AreEqual(countOfGames, (objectResult.Value as List<GameResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGames_WithNoUserGames_ReturnsEmptyListOfGames()
        {
            // Arrange
            var countOfGames = 0;
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.GetGames();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<GameResponse>>(objectResult.Value);
            Assert.AreEqual(countOfGames, (objectResult.Value as List<GameResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGame_WithExistingGameId_ReturnsGame()
        {
            // Arrange
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.GetGame(game.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<GameResponse>(objectResult.Value);
            Assert.AreEqual(game.Title, (objectResult.Value as GameResponse).Title);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGame_WithNotExistingGameId_ReturnsNotFound()
        {
            // Arrange
            var gameId = 1;
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.GetGame(gameId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateGame_WithExistingGameId_ReturnsNoContent()
        {
            // Arrange
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            var gameRequest = new GameRequest
            {
                Title = "Test Game update",
                Comment = "Test Comment"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.UpdateGame(game.Id, gameRequest);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            var updatedGame = await _context.Games.SingleOrDefaultAsync(g => g.Id == game.Id);
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(204, statusCodeResult.StatusCode);
            Assert.AreEqual(gameRequest.Title, updatedGame.Title);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateGame_WithNotExistingGameId_ReturnsNotFound()
        {
            // Arrange
            var gameId = 1;
            var gameRequest = new GameRequest
            {
                Title = "Test Game update",
                Comment = "Test Comment"
            };
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.UpdateGame(gameId, gameRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGame_WithRegularUserRole_ReturnsCreatedGameWithoutTeams()
        {
            // Arrange
            var gameRequest = new GameRequest
            {
                Title = "Test Game",
                Comment = "Test Description",
                HomeTeamId = 1,
                GuestTeamId = 1
            };
            var gamesController = CreateController<GamesController>("RegularUser");

            // Act
            var actionResult = await gamesController.CreateGame(gameRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<GameResponse>(objectResult.Value);
            Assert.AreEqual(gameRequest.Title, (objectResult.Value as GameResponse).Title);
            Assert.Null((objectResult.Value as GameResponse).HomeTeamId);
            Assert.Null((objectResult.Value as GameResponse).GuestTeamId);
            Assert.AreEqual(1, _context.Games.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGame_WithPremiumUserRoleAndExistingTeamId_ReturnsCreatedGameWithTeams()
        {
            // Arrange
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;
            var gameRequest = new GameRequest
            {
                Title = "Test Game",
                Comment = "Test Description",
                HomeTeamId = team.Id,
                GuestTeamId = team.Id
            };
            var gamesController = CreateController<GamesController>("PremiumUser");

            // Act
            var actionResult = await gamesController.CreateGame(gameRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<GameResponse>(objectResult.Value);
            Assert.AreEqual(gameRequest.Title, (objectResult.Value as GameResponse).Title);
            Assert.AreEqual(gameRequest.HomeTeamId, (objectResult.Value as GameResponse).HomeTeamId);
            Assert.AreEqual(gameRequest.GuestTeamId, (objectResult.Value as GameResponse).GuestTeamId);
            Assert.AreEqual(1, _context.Games.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGame_WithPremiumUserRoleAndNotExistingTeam1Id_ReturnsNotFound()
        {
            // Arrange
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;
            var gameRequest = new GameRequest
            {
                Title = "Test Game",
                Comment = "Test Description",
                HomeTeamId = team.Id + 1,
                GuestTeamId = team.Id
            };
            var gamesController = CreateController<GamesController>("PremiumUser");

            // Act
            var actionResult = await gamesController.CreateGame(gameRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }
        [Test]
        public async Task CreateGame_WithPremiumUserRoleAndNotExistingTeam2Id_ReturnsNotFound()
        {
            // Arrange
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;
            var gameRequest = new GameRequest
            {
                Title = "Test Game",
                Comment = "Test Description",
                HomeTeamId = team.Id,
                GuestTeamId = team.Id + 1
            };
            var gamesController = CreateController<GamesController>("PremiumUser");

            // Act
            var actionResult = await gamesController.CreateGame(gameRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteGame_WithExistingGameId_ReturnsDeletedGame()
        {
            // Arrange
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.DeleteGame(game.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<GameResponse>(objectResult.Value);
            Assert.AreEqual(game.Title, (objectResult.Value as GameResponse).Title);
            Assert.AreEqual(0, _context.Games.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteGame_WithNotExistingGameId_ReturnsNotFound()
        {
            // Arrange
            var gameId = 1;
            var gamesController = CreateController<GamesController>();

            // Act
            var actionResult = await gamesController.DeleteGame(gameId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }
    }
}
