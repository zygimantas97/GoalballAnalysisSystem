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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<TeamResponse>>(await _context.Teams.Where(t => t.IdentityUserId == userId).ToListAsync()));
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeam(long teamId)
        {
            var userId = HttpContext.GetUserId();
            var team = await _context.Teams
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);

            if (team == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TeamResponse>(team));
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> UpdateTeam(long teamId, TeamRequest request)
        {
            var userId = HttpContext.GetUserId();
            var team = await _context.Teams
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);

            if (team == null)
            {
                return NotFound();
            }

            var updateTeam = _mapper.Map<Team>(request);
            updateTeam.Id = teamId;
            updateTeam.IdentityUserId = userId;

            _context.Teams.Update(updateTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamRequest request)
        {
            var team = _mapper.Map<Team>(request);
            team.IdentityUserId = HttpContext.GetUserId();
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { teamId = team.Id }, _mapper.Map<TeamResponse>(team));
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteTeam(long teamId)
        {
            var userId = HttpContext.GetUserId();
            var team = await _context.Teams
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == teamId && t.IdentityUserId == userId);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TeamResponse>(team));
        }
    }
}
