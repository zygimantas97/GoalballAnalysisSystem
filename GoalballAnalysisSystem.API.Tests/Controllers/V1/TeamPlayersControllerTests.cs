using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    [TestFixture]
    public class TeamPlayersControllerTests : ControllerTestBase
    {
        [Test]
        public async Task GetTeamPlayersByTeamId_WithExistingTeamId_ReturnsListOfTeamPlayers()
        {
            // Arrange
            var countOfTeamPlayers = 5;
            var team = new Team
            {
                IdentityUserId = "test_user",
                Name = "Test Team"
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            _context.Entry(team).State = EntityState.Detached;

            for (int i = 0; i < countOfTeamPlayers; i++)
            {
                var player = new Player
                {
                    IdentityUserId = "test_user",
                    Name = "Test Player"
                };
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                _context.Entry(player).State = EntityState.Detached;

                var teamPlayer = new TeamPlayer
                {
                    TeamId = team.Id,
                    PlayerId = player.Id
                };
                _context.TeamPlayers.Add(teamPlayer);
                await _context.SaveChangesAsync();
                _context.Entry(teamPlayer).State = EntityState.Detached;
            }
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.GetTeamPlayersByTeam(team.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<TeamPlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeamPlayers, (objectResult.Value as List<TeamPlayerResponse>).Count);
        }

        [Test]
        public async Task GetTeamPlayersByTeamId_WithNotExistingTeamId_ReturnsEmptyListOfTeamPlayers()
        {
            // Arrange
            var teamId = 1;
            var countOfTeamPlayers = 0;
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.GetTeamPlayersByTeam(teamId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<TeamPlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeamPlayers, (objectResult.Value as List<TeamPlayerResponse>).Count);
        }

        [Test]
        public async Task GetTeamPlayersByPlayerId_WithExistingPlayerId_ReturnsListOfTeamPlayers()
        {
            // Arrange
            var countOfTeamPlayers = 5;
            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            for (int i = 0; i < countOfTeamPlayers; i++)
            {
                var team = new Team
                {
                    IdentityUserId = "test_user",
                    Name = "Test Team"
                };
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                _context.Entry(team).State = EntityState.Detached;

                var teamPlayer = new TeamPlayer
                {
                    TeamId = i + 1,
                    PlayerId = player.Id
                };
                _context.TeamPlayers.Add(teamPlayer);
                await _context.SaveChangesAsync();
                _context.Entry(teamPlayer).State = EntityState.Detached;
            }
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.GetTeamPlayersByPlayer(player.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<TeamPlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeamPlayers, (objectResult.Value as List<TeamPlayerResponse>).Count);
        }

        [Test]
        public async Task GetTeamPlayersByPlayerId_WithNotExistingPlayerId_ReturnsEmptyListOfTeamPlayers()
        {
            // Arrange
            var playerId = 1;
            var countOfTeamPlayers = 0;
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.GetTeamPlayersByPlayer(playerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<List<TeamPlayerResponse>>(objectResult.Value);
            Assert.AreEqual(countOfTeamPlayers, (objectResult.Value as List<TeamPlayerResponse>).Count);
        }

        [Test]
        public async Task GetTeamPlayer_WithExistingTeamPlayer_ReturnsTeamPlayer()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.GetTeamPlayer(team.Id, player.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<TeamPlayerResponse>(objectResult.Value);
            Assert.AreEqual(teamPlayer.TeamId, (objectResult.Value as TeamPlayerResponse).TeamId);
            Assert.AreEqual(teamPlayer.PlayerId, (objectResult.Value as TeamPlayerResponse).PlayerId);
        }

        [Test]
        public async Task GetTeamPlayer_WithNotExistingTeamPlayer_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;
            var playerId = 1;
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.GetTeamPlayer(teamId, playerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task UpdateTeamPlayer_WithExistingTeamPlayerAndExistingRoleAndCorrectNumber_ReturnsNoContent()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id,
                RoleId = role.Id,
                Number = 1
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = teamPlayer.RoleId,
                Number = 100
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.UpdateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            var updatedTeamPlayer = await _context.TeamPlayers.SingleOrDefaultAsync(tp => tp.TeamId == team.Id && tp.PlayerId == player.Id);
            Assert.NotNull(statusCodeResult);
            Assert.AreEqual(204, statusCodeResult.StatusCode);
            Assert.AreEqual(teamPlayerRequest.Number, updatedTeamPlayer.Number);
        }

        [Test]
        public async Task UpdateTeamPlayer_WithNotExistingTeamPlayer_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;
            var playerId = 1;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = role.Id,
                Number = 100
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.UpdateTeamPlayer(teamId, playerId, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task UpdateTeamPlayer_WithNotExistingRole_ReturnsNotFound()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var roleId = await _context.PlayerRoles.MaxAsync(pr => pr.Id);

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id,
                RoleId = roleId,
                Number = 1
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = roleId + 1,
                Number = 100
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.UpdateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task UpdateTeamPlayer_WithIncorrectNumber_ReturnsBadRequest()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id,
                RoleId = role.Id,
                Number = 1
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = teamPlayer.RoleId,
                Number = -1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.UpdateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [Test]
        public async Task CreateTeamPlayer_WithExistingTeamExistingPlayerExistingRoleAndCorrectNumber_ReturnsCreatedTeamPlayer()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = role.Id,
                Number = 1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.CreateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(201, objectResult.StatusCode);
            Assert.IsInstanceOf<TeamPlayerResponse>(objectResult.Value);
            Assert.AreEqual(team.Id, (objectResult.Value as TeamPlayerResponse).TeamId);
            Assert.AreEqual(player.Id, (objectResult.Value as TeamPlayerResponse).PlayerId);
            Assert.AreEqual(1, _context.TeamPlayers.Count());
        }

        [Test]
        public async Task CreateTeamPlayer_WithNotExistingTeam_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = role.Id,
                Number = 1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.CreateTeamPlayer(teamId, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task CreateTeamPlayer_WithNotExistingPlayer_ReturnsNotFound()
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

            var playerId = 1;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = role.Id,
                Number = 1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.CreateTeamPlayer(team.Id, playerId, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task CreateTeamPlayer_WithAlreadyExistingTeamPlayer_ReturnsBadRequest()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id,
                RoleId = role.Id,
                Number = 1
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = role.Id,
                Number = 1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.CreateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [Test]
        public async Task CreateTeamPlayer_WithNotExistingRole_ReturnsNotFound()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var roleId = await _context.PlayerRoles.MaxAsync(pr => pr.Id);
            roleId++;

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = roleId,
                Number = 1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.CreateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task CreateTeamPlayer_WithIncorrectNumber_ReturnsBadRequest()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var role = await _context.PlayerRoles.FirstOrDefaultAsync();

            var teamPlayerRequest = new TeamPlayerRequest
            {
                RoleId = role.Id,
                Number = -1
            };
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.CreateTeamPlayer(team.Id, player.Id, teamPlayerRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [Test]
        public async Task DeleteTeamPlayer_WithExistingTeamPlayer_ReturnsDeletedTeamPlayer()
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

            var player = new Player
            {
                IdentityUserId = "test_user",
                Name = "Test Player"
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            _context.Entry(player).State = EntityState.Detached;

            var teamPlayer = new TeamPlayer
            {
                TeamId = team.Id,
                PlayerId = player.Id
            };
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            _context.Entry(teamPlayer).State = EntityState.Detached;
            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.DeleteTeamPlayer(team.Id, player.Id);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsInstanceOf<TeamPlayerResponse>(objectResult.Value);
            Assert.AreEqual(teamPlayer.TeamId, (objectResult.Value as TeamPlayerResponse).TeamId);
            Assert.AreEqual(teamPlayer.PlayerId, (objectResult.Value as TeamPlayerResponse).PlayerId);
            Assert.AreEqual(0, _context.TeamPlayers.Count());
        }

        [Test]
        public async Task DeleteTeamPlayer_WithNotExistingTeamPlayer_ReturnsNotFound()
        {
            // Arrange
            var teamId = 1;
            var playerId = 1;

            var teamPlayersController = CreateController<TeamPlayersController>();

            // Act
            var actionResult = await teamPlayersController.DeleteTeamPlayer(teamId, playerId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            Assert.AreEqual(404, objectResult.StatusCode);
        }
    }
}
