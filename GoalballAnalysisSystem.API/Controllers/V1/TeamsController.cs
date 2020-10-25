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
using GoalballAnalysisSystem.API.Contracts.V1.Requests;

namespace GoalballAnalysisSystem.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "PremiumUser")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TeamsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all user's teams
        /// </summary>
        /// <response code="200">Returns all teams</response>
        [HttpGet]
        [ProducesResponseType(typeof(TeamResponse), 200)]
        public async Task<IActionResult> GetTeams()
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<TeamResponse>>(await _context.Teams.Where(t => t.IdentityUserId == userId).ToListAsync()));
        }

        /// <summary>
        /// Returns user's team by Id
        /// </summary>
        /// <response code="200">Returns team by Id</response>
        /// <response code="404">Unable to find team by given Id</response>
        [HttpGet("{teamId}")]
        [ProducesResponseType(typeof(TeamResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetTeam(long teamId)
        {
            var userId = HttpContext.GetUserId();
            var team = await _context.Teams
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);

            if (team == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find team by given Id" } } });
            }

            return Ok(_mapper.Map<TeamResponse>(team));
        }

        /// <summary>
        /// Updates user's team by Id
        /// </summary>
        /// <response code="204">Team was successfully updated</response>
        /// <response code="400">Unable to update team</response>
        /// <response code="404">Unable to find team by given Id</response>
        [HttpPut("{teamId}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> UpdateTeam(long teamId, TeamRequest request)
        {
            var userId = HttpContext.GetUserId();
            var team = await _context.Teams
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);

            if (team == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find team by given Id" } } });
            }

            var updateTeam = _mapper.Map<Team>(request);
            updateTeam.Id = teamId;
            updateTeam.IdentityUserId = userId;

            _context.Teams.Update(updateTeam);
            var updated = await _context.SaveChangesAsync();
            if(updated > 0)
                return NoContent();
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to update team: database error" } } });
        }

        /// <summary>
        /// Creates user's team
        /// </summary>
        /// <response code="201">Team was successfully created</response>
        /// <response code="400">Unable to create team</response>
        [HttpPost]
        [ProducesResponseType(typeof(TeamResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreateTeam(TeamRequest request)
        {
            var team = _mapper.Map<Team>(request);
            team.IdentityUserId = HttpContext.GetUserId();

            _context.Teams.Add(team);
            var created = await _context.SaveChangesAsync();
            if(created > 0)
                return CreatedAtAction("GetTeam", new { teamId = team.Id }, _mapper.Map<TeamResponse>(team));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create team: database error" } } });
        }

        /// <summary>
        /// Deletes user's team by Id
        /// </summary>
        /// <response code="200">Team was successfully deleted</response>
        /// <response code="400">Unable to delete team</response>
        /// <response code="404">Unable to find team by given Id</response>
        [HttpDelete("{teamId}")]
        [ProducesResponseType(typeof(TeamResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> DeleteTeam(long teamId)
        {
            var userId = HttpContext.GetUserId();
            var team = await _context.Teams
                .Include(t => t.HomeGames)
                .Include(t => t.GuestGames)
                .Include(t => t.TeamPlayers)
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);
            if (team == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find team by given Id" } } });
            }

            _context.Teams.Remove(team);
            var deleted = await _context.SaveChangesAsync();
            if(deleted > 0)
                return Ok(_mapper.Map<TeamResponse>(team));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to delete team: database error" } } });
        }
    }
}
