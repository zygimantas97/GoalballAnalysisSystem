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
    public class TeamPlayersControllerTests : ControllerTestBase
    {
        private TeamPlayersController CreateTeamPlayersController()
        {
            return new TeamPlayersController(_context, _mapper);
        }

        [Test]
        public async Task GetTeamPlayersByTeamId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamPlayersController = this.CreateTeamPlayersController();
            long teamId = 0;

            // Act
            var result = await teamPlayersController.GetTeamPlayersByTeamId(
                teamId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetTeamPlayersByPlayerId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamPlayersController = this.CreateTeamPlayersController();
            long playerId = 0;

            // Act
            var result = await teamPlayersController.GetTeamPlayersByPlayerId(
                playerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetTeamPlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamPlayersController = this.CreateTeamPlayersController();
            long teamId = 0;
            long playerId = 0;

            // Act
            var result = await teamPlayersController.GetTeamPlayer(
                teamId,
                playerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateTeamPlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamPlayersController = this.CreateTeamPlayersController();
            long teamId = 0;
            long playerId = 0;
            TeamPlayerRequest request = null;

            // Act
            var result = await teamPlayersController.UpdateTeamPlayer(
                teamId,
                playerId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateTeamPlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamPlayersController = this.CreateTeamPlayersController();
            long teamId = 0;
            long playerId = 0;
            TeamPlayerRequest request = null;

            // Act
            var result = await teamPlayersController.CreateTeamPlayer(
                teamId,
                playerId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteTeamPlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamPlayersController = this.CreateTeamPlayersController();
            long teamId = 0;
            long playerId = 0;

            // Act
            var result = await teamPlayersController.DeleteTeamPlayer(
                teamId,
                playerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
