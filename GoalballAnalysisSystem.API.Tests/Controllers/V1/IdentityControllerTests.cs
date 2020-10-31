using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class IdentityControllerTests : ControllerTestBase
    {
        private Mock<IIdentityService> _mockIIdentityService;

        public IdentityControllerTests()
        {
            _mockIIdentityService = mockRepository.Create<IIdentityService>();
        }

        private IdentityController CreateIdentityController()
        {
            return new IdentityController(_mockIIdentityService.Object);
        }

        [Test]
        public async Task Register_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var identityController = this.CreateIdentityController();
            UserRequest request = null;

            // Act
            var result = await identityController.Register(
                request);

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task Login_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var identityController = this.CreateIdentityController();
            UserRequest request = null;

            // Act
            var result = await identityController.Login(
                request);

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task RefreshToken_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var identityController = this.CreateIdentityController();
            RefreshTokenRequest request = null;

            // Act
            var result = await identityController.RefreshToken(
                request);

            // Assert
            Assert.Fail();
        }
    }
}
