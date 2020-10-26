﻿using System;
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
    public class PlayersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PlayersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all user's players
        /// </summary>
        /// <response code="200">Returns all players</response>
        [HttpGet]
        [ProducesResponseType(typeof(PlayerResponse), 200)]
        public async Task<IActionResult> GetPlayers()
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<PlayerResponse>>(await _context.Players.Where(p => p.IdentityUserId == userId).ToListAsync()));
        }

        /// <summary>
        /// Returns user's player by Id
        /// </summary>
        /// <response code="200">Returns player by Id</response>
        /// <response code="404">Unable to find player by given Id</response>
        [HttpGet("{playerId}")]
        [ProducesResponseType(typeof(PlayerResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetPlayer(long playerId)
        {
            var userId = HttpContext.GetUserId();
            var player = await _context.Players
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);

            if (player == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find user's player by given Id" } } });
            }

            return Ok(_mapper.Map<PlayerResponse>(player));
        }

        /// <summary>
        /// Updates user's player by given Id
        /// </summary>
        /// <response code="204">Player was successfully updated</response>
        /// <response code="400">Unable to update player</response>
        /// <response code="404">Unable to find player by given Id</response>
        [HttpPut("{playerId}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> UpdatePlayer(long playerId, PlayerRequest request)
        {
            var userId = HttpContext.GetUserId();
            var player = await _context.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);

            if(player == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find user's player by given Id" } } });
            }

            var updatePlayer = _mapper.Map<Player>(request);
            updatePlayer.Id = playerId;
            updatePlayer.IdentityUserId = userId;

            _context.Players.Update(updatePlayer);
            var updated = await _context.SaveChangesAsync();
            if(updated > 0)
                return NoContent();
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to update user's player: database error" } } });
        }

        /// <summary>
        /// Creates user's player
        /// </summary>
        /// <response code="201">Player was successfully created</response>
        /// <response code="400">Unable to create player</response>
        [HttpPost]
        [ProducesResponseType(typeof(PlayerResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreatePlayer(PlayerRequest request)
        {
            var player = _mapper.Map<Player>(request);
            player.IdentityUserId = HttpContext.GetUserId();
            
            _context.Players.Add(player);
            var created = await _context.SaveChangesAsync();
            if(created > 0)
                return CreatedAtAction("GetPlayer", new { playerId = player.Id }, _mapper.Map<PlayerResponse>(player));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create player: database error" } } });
        }

        /// <summary>
        /// Deletes user's player by Id
        /// </summary>
        /// <response code="200">Player was successfully deleted</response>
        /// <response code="400">Unable to delete player</response>
        /// <response code="404">Unable to find player by given Id</response>
        [HttpDelete("{playerId}")]
        [ProducesResponseType(typeof(PlayerResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> DeletePlayer(long playerId)
        {
            var userId = HttpContext.GetUserId();
            var player = await _context.Players
                .Include(p => p.PlayerTeams)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == playerId && p.IdentityUserId == userId);
            if (player == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find player by given Id" } } });
            }

            _context.Players.Remove(player);
            var deleted = await _context.SaveChangesAsync();
            if(deleted > 0)
                return Ok(_mapper.Map<PlayerResponse>(player));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to delete player: database error" } } });
        }
    }
}