using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class GamesControllerTests : ControllerTestBase
    {
        private GamesController CreateGamesController()
        {
            return new GamesController(_context, _mapper);
        }

        [Test]
        public async Task GetGames_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesController = this.CreateGamesController();

            // Act
            var result = await gamesController.GetGames();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesController = this.CreateGamesController();
            int gameId = 0;

            // Act
            var result = await gamesController.GetGame(
                gameId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesController = this.CreateGamesController();
            int gameId = 0;
            GameRequest request = null;

            // Act
            var result = await gamesController.UpdateGame(
                gameId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesController = this.CreateGamesController();
            GameRequest request = null;

            // Act
            var result = await gamesController.CreateGame(
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesController = this.CreateGamesController();
            int gameId = 0;

            // Act
            var result = await gamesController.DeleteGame(
                gameId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
