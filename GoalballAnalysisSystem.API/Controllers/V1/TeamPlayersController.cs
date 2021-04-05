using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GoalballAnalysisSystem.API.Extensions;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Microsoft.VisualBasic.CompilerServices;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.Models;

namespace GoalballAnalysisSystem.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PremiumUser")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TeamPlayersController : AbstractController
    {
        public TeamPlayersController(DataContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        /// <summary>
        /// Returns all user's team players by team Id
        /// </summary>
        /// <response code="200">Returns all team players</response>
        [HttpGet("ByTeam/{teamId}")]
        [ProducesResponseType(typeof(TeamPlayerResponse), 200)]
        public async Task<IActionResult> GetTeamPlayersByTeam(long teamId)
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<TeamPlayerResponse>>(await _context.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .Include(tp => tp.Role)
                .Where(tp => tp.Team.IdentityUserId == userId && tp.TeamId == teamId)
                .AsNoTracking()
                .ToListAsync()));
        }

        /// <summary>
        /// Returns all user's team players by player Id
        /// </summary>
        /// <response code="200">Returns all team players</response>
        [HttpGet("ByPlayer/{playerId}")]
        [ProducesResponseType(typeof(TeamPlayerResponse), 200)]
        public async Task<IActionResult> GetTeamPlayersByPlayer(long playerId)
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<TeamPlayerResponse>>(await _context.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .Include(tp => tp.Role)
                .Where(tp => tp.Player.IdentityUserId == userId && tp.PlayerId == playerId)
                .AsNoTracking()
                .ToListAsync()));
        }

        /// <summary>
        /// Returns user's team player by Ids
        /// </summary>
        /// <response code="200">Returns team player by Ids</response>
        /// <response code="404">Unable to find team player by given Ids</response>
        [HttpGet("{teamId}/{playerId}")]
        [ProducesResponseType(typeof(TeamPlayerResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetTeamPlayer(long teamId, long playerId)
        {
            var userId = HttpContext.GetUserId();
            var teamPlayer = await _context.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .Include(tp => tp.Role)
                .AsNoTracking()
                .SingleOrDefaultAsync(tp => tp.Team.IdentityUserId == userId && tp.TeamId == teamId && tp.Player.IdentityUserId == userId && tp.PlayerId == playerId);

            if (teamPlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find team player by given Ids" } } });
            }

            return Ok(_mapper.Map<TeamPlayerResponse>(teamPlayer));
        }

        /// <summary>
        /// Updates user's team player by Ids
        /// </summary>
        /// <response code="204">Team player was successfully updated</response>
        /// <response code="400">Unable to update team player</response>
        /// <response code="404">Unable to find</response>
        [HttpPut("{teamId}/{playerId}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> UpdateTeamPlayer(long teamId, long playerId, TeamPlayerRequest request)
        {
            var userId = HttpContext.GetUserId();
            var teamPlayer = await _context.TeamPlayers
                .Include(tp => tp.Team)
                .AsNoTracking()
                .SingleOrDefaultAsync(tp => tp.Team.IdentityUserId == userId && tp.TeamId == teamId && tp.PlayerId == playerId);

            if(teamPlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find team player by given Ids" } } });
            }

            var playerRole = await _context.PlayerRoles
                .AsNoTracking()
                .SingleOrDefaultAsync(pr => pr.Id == request.RoleId);
            if(playerRole == null && request.RoleId != null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find player role by given Id" } } });
            }

            if(request.Number < 0)
            {
                return BadRequest(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to update team player: wrong team players's number"} } });
            }

            var updateTeamPlayer = _mapper.Map<TeamPlayer>(request);
            updateTeamPlayer.TeamId = teamId;
            updateTeamPlayer.PlayerId = playerId;

            _context.TeamPlayers.Update(updateTeamPlayer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates user's team player
        /// </summary>
        /// <response code="201">Team player was successfully created</response>
        /// <response code="400">Unable to create team player</response>
        /// <response code="404">Unable to find</response>
        [HttpPost("{teamId}/{playerId}")]
        [ProducesResponseType(typeof(TeamPlayerResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> CreateTeamPlayer(long teamId, long playerId, TeamPlayerRequest request)
        {
            var userId = HttpContext.GetUserId();

            var team = await _context.Teams
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);
            if(team == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find team by given Id"} } });
            }

            var player = await _context.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);
            if(player == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find player by given Id"} } });
            }

            var existingTeamPlayer = await _context.TeamPlayers
                .Include(tp => tp.Team)
                .AsNoTracking()
                .SingleOrDefaultAsync(tp => tp.Team.IdentityUserId == userId && tp.TeamId == teamId && tp.PlayerId == playerId);
            if(existingTeamPlayer != null)
            {
                return BadRequest(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to create team player: this team player already exists"} } });
            }

            var playerRole = await _context.PlayerRoles
                .AsNoTracking()
                .SingleOrDefaultAsync(pr => pr.Id == request.RoleId);
            if (playerRole == null && request.RoleId != null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find player role by given Id" } } });
            }

            if (request.Number < 0)
            {
                return BadRequest(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to create team player: wrong team players's number" } } });
            }

            var teamPlayer = _mapper.Map<TeamPlayer>(request);
            teamPlayer.TeamId = teamId;
            teamPlayer.PlayerId = playerId;

            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
            teamPlayer.Team = team;
            teamPlayer.Player = player;
            teamPlayer.Role = playerRole;
            return CreatedAtAction("GetTeamPlayer", new { teamId = teamPlayer.TeamId, playerId = teamPlayer.PlayerId }, _mapper.Map<TeamPlayerResponse>(teamPlayer));
        }

        /// <summary>
        /// Deletes user's team player by Ids
        /// </summary>
        /// <response code="200">Team player was successfully deleted</response>
        /// <response code="404">Unable to find team player by given Ids</response>
        [HttpDelete("{teamId}/{playerId}")]
        [ProducesResponseType(typeof(TeamPlayerResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> DeleteTeamPlayer(long teamId, long playerId)
        {
            var userId = HttpContext.GetUserId();
            var teamPlayer = await _context.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .Include(tp => tp.Role)
                .Include(tp => tp.GamePlayers)
                .AsNoTracking()
                .SingleOrDefaultAsync(tp => tp.Team.IdentityUserId == userId && tp.TeamId == teamId && tp.Player.IdentityUserId == userId && tp.PlayerId == playerId);
            if (teamPlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error { Message = "Unable to find team player by given Ids"} } });
            }

            _context.TeamPlayers.Remove(teamPlayer);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<TeamPlayerResponse>(teamPlayer));
        }
    }
}
