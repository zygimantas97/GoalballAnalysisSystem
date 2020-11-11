using AutoMapper;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.Domain.Tests.Controllers.V1
{
    [TestFixture]
    public class PlayersControllerTests
    {
        private MockRepository mockRepository;

        private Mock<DataContext> mockDataContext;
        private Mock<IMapper> mockMapper;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockDataContext = this.mockRepository.Create<DataContext>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
        }

        private PlayersController CreatePlayersController()
        {
            return new PlayersController(
                this.mockDataContext.Object,
                this.mockMapper.Object);
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
