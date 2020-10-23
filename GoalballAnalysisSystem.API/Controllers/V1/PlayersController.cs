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
    public class PlayersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PlayersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<PlayerResponse>>(await _context.Players.Where(p => p.IdentityUserId == userId).ToListAsync()));
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPlayer(long playerId)
        {
            var userId = HttpContext.GetUserId();
            var player = await _context.Players
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlayerResponse>(player));
        }

        [HttpPut("{playerId}")]
        public async Task<IActionResult> UpdatePlayer(long playerId, PlayerRequest request)
        {
            var userId = HttpContext.GetUserId();
            var player = await _context.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);

            if(player == null)
            {
                return NotFound();
            }

            var updatePlayer = _mapper.Map<Player>(request);
            updatePlayer.Id = playerId;
            updatePlayer.IdentityUserId = userId;

            _context.Players.Update(updatePlayer);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(PlayerRequest request)
        {
            var player = _mapper.Map<Player>(request);
            player.IdentityUserId = HttpContext.GetUserId();
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { playerId = player.Id }, _mapper.Map<PlayerResponse>(player));
        }

        [HttpDelete("{playerId}")]
        public async Task<IActionResult> DeletePlayer(long playerId)
        {
            var userId = HttpContext.GetUserId();
            var player = await _context.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<PlayerResponse>(player));
        }
    }
}
