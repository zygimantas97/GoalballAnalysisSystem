using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Mapping;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class PlayersControllerTests : ControllerTestBase
    {
        private PlayersController CreatePlayersController()
        {
            return new PlayersController(_context, _mapper);
        }

        [Test]
        public async Task GetPlayers_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playersController = this.CreatePlayersController();

            // Act
            var result = await playersController.GetPlayers();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetPlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playersController = this.CreatePlayersController();
            long playerId = 0;

            // Act
            var result = await playersController.GetPlayer(
                playerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdatePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playersController = this.CreatePlayersController();
            long playerId = 0;
            PlayerRequest request = null;

            // Act
            var result = await playersController.UpdatePlayer(
                playerId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreatePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playersController = this.CreatePlayersController();
            PlayerRequest request = null;

            // Act
            var result = await playersController.CreatePlayer(
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeletePlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var playersController = this.CreatePlayersController();
            long playerId = 0;

            // Act
            var result = await playersController.DeletePlayer(
                playerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
