using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class TeamsControllerTests : ControllerTestBase
    {
        private TeamsController CreateTeamsController()
        {
            return new TeamsController(_context, _mapper);
        }

        [Test]
        public async Task GetTeams_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamsController = this.CreateTeamsController();

            // Act
            var result = await teamsController.GetTeams();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetTeam_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamsController = this.CreateTeamsController();
            long teamId = 0;

            // Act
            var result = await teamsController.GetTeam(
                teamId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateTeam_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamsController = this.CreateTeamsController();
            long teamId = 0;
            TeamRequest request = null;

            // Act
            var result = await teamsController.UpdateTeam(
                teamId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateTeam_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamsController = this.CreateTeamsController();
            TeamRequest request = null;

            // Act
            var result = await teamsController.CreateTeam(
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteTeam_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var teamsController = this.CreateTeamsController();
            long teamId = 0;

            // Act
            var result = await teamsController.DeleteTeam(
                teamId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
