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
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class GamePlayersControllerTests : ControllerTestBase
    {
       
        [Test]
        public async Task GetGamePlayersByGameId_WithExistingGameId_ReturnsListOfGamePlayers()
        {
            // Arrange
            var countOfGamePlayers = 3;
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            for (int i = 0; i < countOfGamePlayers; i++)
            {
                var gamePlayer = new GamePlayer
                {
                    GameId = game.Id,
                    PlayerId = i + 1
                };
                _context.GamePlayers.Add(gamePlayer);
                await _context.SaveChangesAsync();
                _context.Entry(gamePlayer).State = EntityState.Detached;
            }
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.GetGamePlayersByGameId(game.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<GamePlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfGamePlayers, (objectResult.Value as List<GamePlayerResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGamePlayersByGameId_WithNotExistingGameId_ReturnsEmptyListOfGamePlayers()
        {
            // Arrange
            var gameId = 1;
            var countOfGamePlayers = 0;
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.GetGamePlayersByGameId(gameId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<GamePlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfGamePlayers, (objectResult.Value as List<GamePlayerResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGamePlayersByTeamPlayerId_WithExistingTeamPlayerId_ReturnsListOfGamePlayers()
        {
            // Arrange
            var countOfTeamPlayers = 5;
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            for (int i = 0; i < countOfTeamPlayers; i++)
            {
                var gamePlayer = new GamePlayer
                {
                    TeamId = team.Id,
                    GameId = game.Id,
                    PlayerId = teamPlayer.PlayerId
                };
                _context.GamePlayers.Add(gamePlayer);
                await _context.SaveChangesAsync();
                _context.Entry(gamePlayer).State = EntityState.Detached;
            }
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.GetGamePlayersByTeamPlayerId(
                team.Id,
                teamPlayer.PlayerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<GamePlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeamPlayers, (objectResult.Value as List<GamePlayerResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGamePlayersByTeamPlayerId_WithNotExistingTeamPlayerId_ReturnsEmptyListOfGamePlayers()
        {
            // Arrange
            var teamPlayerId = 0;
            var teamId = 1;
            var countOfTeamPlayers = 0;
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.GetGamePlayersByTeamPlayerId(
                teamId,
                teamPlayerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<GamePlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeamPlayers, (objectResult.Value as List<GamePlayerResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGamePlayer_WithExistingGamePlayer_ReturnsGamePlayer()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var gamePlayer = new GamePlayer
            {
                GameId = game.Id,
                PlayerId = player.Id
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.GetGamePlayer(
                gamePlayer.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<GamePlayerResponse>(objectResult.Value);
            Assert.AreEqual(gamePlayer.GameId, (objectResult.Value as GamePlayerResponse).GameId);
            Assert.AreEqual(gamePlayer.PlayerId, (objectResult.Value as GamePlayerResponse).PlayerId);
            this.mockRepository.VerifyAll();
        }
        
        [Test]
        public async Task GetGamePlayer_WithNotExistingGamePlayer_ReturnsNotFound()
        {
            // Arrange
            var gamePlayerId = 1;
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.GetGamePlayer(gamePlayerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task PutGamePlayer_WithExistingGamePlayerAndCorrectStartAndEndTime_ReturnsNoContent()
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

            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var gamePlayer = new GamePlayer
            {
                PlayerId = player.Id,
                GameId = game.Id,
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var updateGamePlayerRequest = new UpdateGamePlayerRequest
            {
                StartTime = new DateTime(2020, 11, 1, 11, 0, 0),
                EndTime = new DateTime(2020, 12, 1, 12, 0, 0)
            };

            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.PutGamePlayer(gamePlayer.Id,updateGamePlayerRequest);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            var updatedGamePlayer = await _context.GamePlayers.SingleOrDefaultAsync(gp => gp.GameId == game.Id && gp.PlayerId == player.Id);
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(204, statusCodeResult.StatusCode);
            Assert.AreEqual(updateGamePlayerRequest.StartTime, updatedGamePlayer.StartTime);
            Assert.AreEqual(updateGamePlayerRequest.EndTime, updatedGamePlayer.EndTime);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task PutGamePlayer_WithNotExistingGamePlayer_ReturnsNotFound()
        {
            // Arrange
            var gamePlayerId = 1;

            var updateGamePlayerRequest = new UpdateGamePlayerRequest
            {
                StartTime = new DateTime(2020, 11, 1, 11, 0, 0),
                EndTime = new DateTime(2020, 12, 1, 12, 0, 0)
            };

            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.PutGamePlayer(gamePlayerId, updateGamePlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task PutGamePlayer_WithIncorrectTime_ReturnsBadRequest()
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

            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var gamePlayer = new GamePlayer
            {
                PlayerId = player.Id,
                GameId = game.Id,
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var updateGamePlayerRequest = new UpdateGamePlayerRequest
            {
                StartTime = new DateTime(2020, 12, 1, 12, 0, 0),
                EndTime = new DateTime(2020, 11, 1, 11, 0, 0)
            };

            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.PutGamePlayer(gamePlayer.Id, updateGamePlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGamePlayer_WithExistingGameTeamPlayerAndCorrectTime_ReturnsCreatedGamePlayer()
        {
            // Arrange
            var startTime = new DateTime(2020, 11, 1, 11, 0, 0);
            var endTime = new DateTime(2020, 12, 1, 12, 0, 0);

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var createGamePlayerRequest = new CreateGamePlayerRequest
            {
                GameId = game.Id,
                PlayerId = player.Id,
                TeamId = team.Id,
                StartTime = startTime,
                EndTime = endTime
            };
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.CreateGamePlayer(createGamePlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<GamePlayerResponse>(objectResult.Value);
            Assert.AreEqual(team.Id, (objectResult.Value as GamePlayerResponse).TeamId);
            Assert.AreEqual(player.Id, (objectResult.Value as GamePlayerResponse).PlayerId);
            Assert.AreEqual(game.Id, (objectResult.Value as GamePlayerResponse).GameId);
            Assert.AreEqual(startTime, (objectResult.Value as GamePlayerResponse).StartTime);
            Assert.AreEqual(endTime, (objectResult.Value as GamePlayerResponse).EndTime);
            Assert.AreEqual(1, _context.GamePlayers.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGamePlayer_WithNotExistingGame_ReturnsNotFound()
        {
            // Arrange
            var startTime = new DateTime(2020, 11, 1, 11, 0, 0);
            var endTime = new DateTime(2020, 12, 1, 12, 0, 0);
            var gameId = 1;

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var createGamePlayerRequest = new CreateGamePlayerRequest
            {
                GameId = gameId,
                PlayerId = player.Id,
                TeamId = team.Id,
                StartTime = startTime,
                EndTime = endTime
            };
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.CreateGamePlayer(createGamePlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGamePlayer_WithNotExistingTeamPlayer_ReturnsNotFound()
        {
            // Arrange
            var startTime = new DateTime(2020, 11, 1, 11, 0, 0);
            var endTime = new DateTime(2020, 12, 1, 12, 0, 0);
            var gameId = 1;

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;

            var createGamePlayerRequest = new CreateGamePlayerRequest
            {
                GameId = gameId,
                PlayerId = player.Id,
                TeamId = team.Id,
                StartTime = startTime,
                EndTime = endTime
            };
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.CreateGamePlayer(createGamePlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGamePlayer_WithIncorrectTime_ReturnsBadRequest()
        {
            // Arrange
            var endTime = new DateTime(2020, 11, 1, 11, 0, 0);
            var startTime = new DateTime(2020, 12, 1, 12, 0, 0);

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var createGamePlayerRequest = new CreateGamePlayerRequest
            {
                GameId = game.Id,
                PlayerId = player.Id,
                TeamId = team.Id,
                StartTime = startTime,
                EndTime = endTime
            };
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.CreateGamePlayer(createGamePlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteGamePlayer_WithExistingGamePlayer_ReturnsDeletedGamePlayer()
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

            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var gamePlayer = new GamePlayer
            {
                Id = player.Id,
                GameId = game.Id,
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.DeleteGamePlayer(gamePlayer.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<GamePlayerResponse>(objectResult.Value);
            Assert.AreEqual(gamePlayer.Id, (objectResult.Value as GamePlayerResponse).Id);
            Assert.AreEqual(0, _context.GamePlayers.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteGamePlayer_WithNotExistingGamePlayer_ReturnsNotFoundr()
        {
            // Arrange
            var gamePlayerId = 1;
            var gamePlayersController = CreateController<GamePlayersController>();

            // Act
            var actionResult = await gamePlayersController.DeleteGamePlayer(gamePlayerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }
    }
}
