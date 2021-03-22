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
    public class TeamsControllerTests : ControllerTestBase
    {
        [Test]
        public async Task GetTeams_With5UserTeams_ReturnsListOf5Teams()
        {
            // Arrange
            var countOfTeams = 5;
            for (int i = 0; i < countOfTeams; i++)
            {
                _context.Teams.Add(new Team
                {
                    IdentityUserId = "test_user"
                });
            }
            await _context.SaveChangesAsync();
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.GetTeams();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<TeamResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeams, (objectResult.Value as List<TeamResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetTeams_WithNoUserTeams_ReturnsEmptyListOfTeams()
        {
            // Arrange
            var countOfTeams = 0;
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.GetTeams();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<TeamResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeams, (objectResult.Value as List<TeamResponse>).Count);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetTeam_WithExistingTeamId_ReturnsTeam()
        {
            // Arrange
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.GetTeam(team.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<TeamResponse>(objectResult.Value);
            Assert.AreEqual(team.Name, (objectResult.Value as TeamResponse).Name);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetTeam_WithNotExistingTeamId_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.GetTeam(teamId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateTeam_WithExistingTeamId_ReturnsNoContent()
        {
            // Arrange
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            var teamRequest = new TeamRequest
            {
                Name = "Test Team update",
                Description = "Test Description",
                Country = "T"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.UpdateTeam(team.Id, teamRequest);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            var updatedTeam = await _context.Teams.SingleOrDefaultAsync(t => t.Id == team.Id);
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(204, statusCodeResult.StatusCode);
            Assert.AreEqual(teamRequest.Name, updatedTeam.Name);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task UpdateTeam_WithNotExistingTeamId_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;
            var teamRequest = new TeamRequest
            {
                Name = "Test Team update",
                Description = "Test Description",
                Country = "T"
            };
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.UpdateTeam(teamId, teamRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task CreateTeam_ReturnsCreatedTeam()
        {
            // Arrange
            var teamRequest = new TeamRequest
            {
                Name = "Test Team",
                Description = "Test Description",
                Country = "T"
            };
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.CreateTeam(teamRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<TeamResponse>(objectResult.Value);
            Assert.AreEqual(teamRequest.Name, (objectResult.Value as TeamResponse).Name);
            Assert.AreEqual(1, _context.Teams.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteTeam_WithExistingTeamId_ReturnsDeletedTeam()
        {
            // Arrange
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.DeleteTeam(team.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<TeamResponse>(objectResult.Value);
            Assert.AreEqual(team.Name, (objectResult.Value as TeamResponse).Name);
            Assert.AreEqual(0, _context.Teams.Count());
            this.mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteTeam_WithNotExistingTeamId_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;
            var teamsController = CreateController<TeamsController>();

            // Act
            var actionResult = await teamsController.DeleteTeam(teamId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
            this.mockRepository.VerifyAll();
        }
    }
}
