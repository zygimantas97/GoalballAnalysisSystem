using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using Moq;
using NUnit.Framework;
using SQLitePCL;
using System;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class GamePlayersControllerTests : ControllerTestBase
    {
        private GamePlayersController CreateGamePlayersController()
        {
            return new GamePlayersController(_context, _mapper);
        }

        [Test]
        public async Task GetGamePlayersByGameId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamePlayersController = this.CreateGamePlayersController();
            long gameId = 0;

            // Act
            var result = await gamePlayersController.GetGamePlayersByGameId(
                gameId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGamePlayersByTeamPlayerId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamePlayersController = this.CreateGamePlayersController();
            long teamId = 0;
            long playerId = 0;

            // Act
            var result = await gamePlayersController.GetGamePlayersByTeamPlayerId(
                teamId,
                playerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGamePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamePlayersController = this.CreateGamePlayersController();
            long gamePlayerId = 0;

            // Act
            var result = await gamePlayersController.GetGamePlayer(
                gamePlayerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task PutGamePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamePlayersController = this.CreateGamePlayersController();
            long gamePlayerId = 0;
            UpdateGamePlayerRequest request = null;

            // Act
            var result = await gamePlayersController.PutGamePlayer(
                gamePlayerId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGamePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamePlayersController = this.CreateGamePlayersController();
            CreateGamePlayerRequest request = null;

            // Act
            var result = await gamePlayersController.CreateGamePlayer(
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteGamePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamePlayersController = this.CreateGamePlayersController();
            long gamePlayerId = 0;

            // Act
            var result = await gamePlayersController.DeleteGamePlayer(
                gamePlayerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
