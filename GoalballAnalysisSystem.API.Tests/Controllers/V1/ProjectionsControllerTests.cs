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
    public class ProjectionsControllerTests : ControllerTestBase
    {
        [Test]
        public async Task GetProjectionsByGameId_WithExistingGameId_ReturnsListOfProjections()
        {
            // Arrange
            var countOfProejctions = 3;
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            for (int i = 0; i < countOfProejctions; i++)
            {
                var projection = new Projection
                {
                    GameId = game.Id
                };
                _context.Projections.Add(projection);
                await _context.SaveChangesAsync();
                _context.Entry(projection).State = EntityState.Detached;
            }
            
            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.GetProjectionsByGameId(game.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<ProjectionResponse>>(objectResult.Value);
            Assert.AreEqual(countOfProejctions, (objectResult.Value as List<ProjectionResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProjectionsByGameId_WithNotExistingGameId_ReturnsEmptyListOfProjections()
        {
            // Arrange
            var countOfProjections = 0;
            var gameId = 1;

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.GetProjectionsByGameId(gameId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<ProjectionResponse>>(objectResult.Value);
            Assert.AreEqual(countOfProjections, (objectResult.Value as List<ProjectionResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProjectionsByGamePlayerId_WithExistingGamePlayeId_ReturnsListOfProjections()
        {
            // Arrange
            var countOfProejctions = 3;
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
                PlayerId = player.Id
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            for (int i = 0; i < countOfProejctions; i++)
            {
                var projection = new Projection
                {
                    GameId = game.Id,
                    GamePlayerId = gamePlayer.Id
                    
                };
                _context.Projections.Add(projection);
                await _context.SaveChangesAsync();
                _context.Entry(projection).State = EntityState.Detached;
            }

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.GetProjectionsByGamePlayerId(gamePlayer.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<ProjectionResponse>>(objectResult.Value);
            Assert.AreEqual(countOfProejctions, (objectResult.Value as List<ProjectionResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProjectionsByGamePlayerId_WithNotExistingGamePlayeId_ReturnsEmptyListOfProjections()
        {
            // Arrange
            var countOfProejctions = 0;
            var gamePlayerId = 1;
            
            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.GetProjectionsByGamePlayerId(gamePlayerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<ProjectionResponse>>(objectResult.Value);
            Assert.AreEqual(countOfProejctions, (objectResult.Value as List<ProjectionResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProjection_WithExistingProjection_ReturnsProjection()
        {
            // Arrange
            var projectionId = 1;
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var projection = new Projection
            {
                GameId = game.Id,
                Id = projectionId

            };
            _context.Projections.Add(projection);
            await _context.SaveChangesAsync();
            _context.Entry(projection).State = EntityState.Detached;

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.GetProjection(projectionId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<ProjectionResponse>(objectResult.Value);
            Assert.AreEqual(projection.Id, (objectResult.Value as ProjectionResponse).Id);
            Assert.AreEqual(projection.GameId, (objectResult.Value as ProjectionResponse).GameId);
            this.mockRepository.VerifyAll();
        }
        [Test]
        public async Task GetProjection_WithNotExistingProjection_ReturnsNotFound()
        {
            // Arrange
            var projectionId = 1;
           
            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.GetProjection(projectionId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateProjectionw_WithExistingProjection_ReturnsNoContent()
        {
            // Arrange
            var projectionId = 1;
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
                PlayerId = player.Id
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var projection = new Projection
            {
                GameId = game.Id,
                Id = projectionId,
                GamePlayerId = gamePlayer.Id,
                X1 = 0,
                X2 = 0
            };
            _context.Projections.Add(projection);
            await _context.SaveChangesAsync();
            _context.Entry(projection).State = EntityState.Detached;

            var request = new ProjectionRequest
            {
                X1 = 10,
                X2 = 20

            };

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.UpdateProjectionw(projectionId,request);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            var updatedProjection = await _context.Projections.SingleOrDefaultAsync(p => p.Id == projectionId && p.GameId == game.Id && p.GamePlayerId == gamePlayer.Id);
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(204, statusCodeResult.StatusCode);
            Assert.AreEqual(request.X1, updatedProjection.X1);
            Assert.AreEqual(request.X2, updatedProjection.X2);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateProjectionw_WithNotExistingProjection_ReturnsNotFound()
        {
            // Arrange
            var projectionId = 1;

            var request = new ProjectionRequest
            {
                X1 = 10,
                X2 = 20

            };

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.UpdateProjectionw(projectionId, request);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateProjection_WithExistingGameAndGamePlayer_ReturnsCreatedProjection()
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
                PlayerId = player.Id
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var projectionRequest = new ProjectionRequest
            {
                
                GamePlayerId = gamePlayer.Id,
                GameId = game.Id,
                X1 = 10,
                Y2 = 15
            };

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.CreateProjection(projectionRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<ProjectionResponse>(objectResult.Value);
            Assert.AreEqual(game.Id, (objectResult.Value as ProjectionResponse).GameId);
            Assert.AreEqual(projectionRequest.X1, (objectResult.Value as ProjectionResponse).X1);
            Assert.AreEqual(projectionRequest.X2, (objectResult.Value as ProjectionResponse).X2);
            Assert.AreEqual(1, _context.Projections.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateProjection_WithExistingGameAndGamePlayerDifferentRole_ReturnsCreatedProjection()
        {
            // Arrange
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };

            string userRole = "NotRegularUser";

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
                PlayerId = player.Id,
                GameId = game.Id
                
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var projectionRequest = new ProjectionRequest
            {

                GamePlayerId = gamePlayer.Id,
                GameId = game.Id,
                X1 = 10,
                Y2 = 15
            };

            var projectionsController = CreateController<ProjectionsController>(userRole);

            // Act
            var actionResult = await projectionsController.CreateProjection(projectionRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<ProjectionResponse>(objectResult.Value);
            Assert.AreEqual(game.Id, (objectResult.Value as ProjectionResponse).GameId);
            Assert.AreEqual(projectionRequest.X1, (objectResult.Value as ProjectionResponse).X1);
            Assert.AreEqual(projectionRequest.X2, (objectResult.Value as ProjectionResponse).X2);
            Assert.AreEqual(1, _context.Projections.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateProjection_WithNotExistingGamePlayerGameIdDifferentRole_ReturnsNotFound()
        {
            // Arrange
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };

            string userRole = "NotRegularUser";

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
                PlayerId = player.Id,
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var projectionRequest = new ProjectionRequest
            {

                GamePlayerId = gamePlayer.Id,
                GameId = game.Id,
                X1 = 10,
                Y2 = 15
            };

            var projectionsController = CreateController<ProjectionsController>(userRole);

            // Act
            var actionResult = await projectionsController.CreateProjection(projectionRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateProjection_WithNotExistingGame_ReturnsNotFound()
        {
            // Arrange
            var gameId = 1;
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
                PlayerId = player.Id
            };
            _context.GamePlayers.Add(gamePlayer);
            await _context.SaveChangesAsync();
            _context.Entry(gamePlayer).State = EntityState.Detached;

            var projectionRequest = new ProjectionRequest
            {
                GameId = gameId,
                GamePlayerId = gamePlayer.Id,
                X1 = 10,
                Y2 = 15
            };

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.CreateProjection(projectionRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteProjection_WithExistingProjection_ReturnsDeletedProjection()
        {
            // Arrange
            var projectionId = 1;
            var game = new Game
            {
                IdentityUserId = "test_user",
                Title = "Test Game"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            _context.Entry(game).State = EntityState.Detached;

            var projection = new Projection
            {
                GameId = game.Id,
                Id = projectionId

            };
            _context.Projections.Add(projection);
            await _context.SaveChangesAsync();
            _context.Entry(projection).State = EntityState.Detached;

            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.DeleteProjection(projectionId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<ProjectionResponse>(objectResult.Value);
            Assert.AreEqual(projection.GameId, (objectResult.Value as ProjectionResponse).GameId);
            Assert.AreEqual(projection.GamePlayerId, (objectResult.Value as ProjectionResponse).GamePlayerId);
            Assert.AreEqual(0, _context.Projections.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteProjection_WithNotExistingProjection_ReturnsNotFound()
        {
            // Arrange
            var projectionId = 1;
            
            var projectionsController = CreateController<ProjectionsController>();

            // Act
            var actionResult = await projectionsController.DeleteProjection(projectionId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }
    }
}
