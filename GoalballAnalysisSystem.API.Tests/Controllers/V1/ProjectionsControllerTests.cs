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
    public class ProjectionsControllerTests : ControllerTestBase
    {
        private ProjectionsController CreateProjectionsController()
        {
            return new ProjectionsController(_context, _mapper);
        }

        [Test]
        public async Task GetProjectionsByGameId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var projectionsController = this.CreateProjectionsController();
            long gameId = 0;

            // Act
            var result = await projectionsController.GetProjectionsByGameId(
                gameId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProjectionsByGamePlayerId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var projectionsController = this.CreateProjectionsController();
            long gamePlayerId = 0;

            // Act
            var result = await projectionsController.GetProjectionsByGamePlayerId(
                gamePlayerId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProjection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var projectionsController = this.CreateProjectionsController();
            long projectionId = 0;

            // Act
            var result = await projectionsController.GetProjection(
                projectionId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateProjectionw_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var projectionsController = this.CreateProjectionsController();
            long projectionId = 0;
            ProjectionRequest request = null;

            // Act
            var result = await projectionsController.UpdateProjectionw(
                projectionId,
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateProjection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var projectionsController = this.CreateProjectionsController();
            ProjectionRequest request = null;

            // Act
            var result = await projectionsController.CreateProjection(
                request);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteProjection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var projectionsController = this.CreateProjectionsController();
            long projectionId = 0;

            // Act
            var result = await projectionsController.DeleteProjection(
                projectionId);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
