using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class PlayerRolesControllerTests : ControllerTestBase
    {
        [Test]
        public async Task GetPlayerRoles_ReturnsListOfAllPlayerRoles()
        {
            // Arrange
            var playerRolesController = CreateController<PlayerRolesController>();

            // Act
            var actionResult = await playerRolesController.GetPlayerRoles();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<PlayerRoleResponse>>(objectResult.Value);
            Assert.AreEqual(_context.PlayerRoles.Count(), (objectResult.Value as List<PlayerRoleResponse>).Count);
        }
    }
}
