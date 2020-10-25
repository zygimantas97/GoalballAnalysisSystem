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
    public class GamePlayersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GamePlayersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all user's game players by game Id
        /// </summary>
        /// <response code="200">Returns all game players</response>
        [HttpGet("ByGame/{gameId}")]
        [ProducesResponseType(typeof(GamePlayerResponse), 200)]
        public async Task<IActionResult> GetGamePlayersByGameId(long gameId)
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<GamePlayerResponse>>(await _context.GamePlayers
                .Include(gp => gp.Game)
                .Where(gp => gp.Game.IdentityUserId == userId && gp.GameId == gameId)
                .ToListAsync()));
        }

        /// <summary>
        /// Returns all user's game players by team player Ids
        /// </summary>
        /// <response code="200">Returns all game players</response>
        [HttpGet("ByTeamPlayer/{teamId}/{playerId}")]
        [ProducesResponseType(typeof(GamePlayerResponse), 200)]
        public async Task<IActionResult> GetGamePlayersByTeamPlayerId(long teamId, long playerId)
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<GamePlayerResponse>>(await _context.GamePlayers
                .Include(gp => gp.Game)
                .Where(gp => gp.Game.IdentityUserId == userId && gp.TeamId == teamId && gp.PlayerId == playerId)
                .ToListAsync()));
        }

        /// <summary>
        /// Returns user's game player by Id
        /// </summary>
        /// <response code="200">Returns game player by Id</response>
        /// <response code="404">Unable to find game player by given Ids</response>
        [HttpGet("{gamePlayerId}")]
        [ProducesResponseType(typeof(GamePlayerResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetGamePlayer(long gamePlayerId)
        {
            var userId = HttpContext.GetUserId();
            var gamePlayer = await _context.GamePlayers
                .Include(gp => gp.Game)
                .SingleOrDefaultAsync(gp => gp.Game.IdentityUserId == userId && gp.Id == gamePlayerId);

            if(gamePlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game player by given Id" } } });
            }

            return Ok(_mapper.Map<GamePlayerResponse>(gamePlayer));
        }

        /// <summary>
        /// Updates user's game player by Id
        /// </summary>
        /// <response code="204">Game player was successfully updated</response>
        /// <response code="400">Unable to update game player</response>
        /// <response code="404">Unable to find game player by given Id</response>
        [HttpPut("{gamePlayerId}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> PutGamePlayer(long gamePlayerId, UpdateGamePlayerRequest request)
        {
            var userId = HttpContext.GetUserId();
            var gamePlayer = await _context.GamePlayers
                .Include(gp => gp.Game)
                .AsNoTracking()
                .SingleOrDefaultAsync(gp => gp.Game.IdentityUserId == userId && gp.Id == gamePlayerId);

            if(gamePlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game player by given Id" } } });
            }

            if(request.StartTime > request.EndTime)
            {
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to update game player: start time must be less or equal end time" } } });
            }

            var updateGamePlayer = _mapper.Map<GamePlayer>(request);
            updateGamePlayer.Id = gamePlayerId;
            updateGamePlayer.TeamId = gamePlayer.TeamId;
            updateGamePlayer.PlayerId = gamePlayer.PlayerId;
            updateGamePlayer.GameId = gamePlayer.GameId;
            
            _context.GamePlayers.Update(updateGamePlayer);
            var updated = await _context.SaveChangesAsync();
            if (updated > 0)
                return NoContent();
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to update game player: database error" } } });
        }

        /// <summary>
        /// Creates user's game player
        /// </summary>
        /// <response code="201">Game player was successfully created</response>
        /// <response code="400">Unable to create game player</response>
        /// <response code="404">Unable to find</response>
        [HttpPost]
        [ProducesResponseType(typeof(GamePlayerResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> CreateGamePlayer(CreateGamePlayerRequest request)
        {
            var userId = HttpContext.GetUserId();

            var game = await _context.Games
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.IdentityUserId == userId && g.Id == request.GameId);
            if (game == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game by given Id" } } });
            }

            var teamPlayer = await _context.TeamPlayers
                .Include(tp => tp.Team)
                .AsNoTracking()
                .SingleOrDefaultAsync(tp => tp.Team.IdentityUserId == userId && tp.TeamId == request.TeamId && tp.PlayerId == request.PlayerId);
            if (teamPlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find team player by given Ids" } } });
            }

            if (request.StartTime > request.EndTime)
            {
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create game player: start time must be less or equal end time" } } });
            }

            var gamePlayer = _mapper.Map<GamePlayer>(request);
            _context.GamePlayers.Add(gamePlayer);
            var created = await _context.SaveChangesAsync();
            if (created > 0)
                return CreatedAtAction("GetGamePlayer", new { gamePlayerId = gamePlayer.Id }, _mapper.Map<GamePlayerResponse>(gamePlayer));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create game player: database error" } } });
        }

        /// <summary>
        /// Deletes user's game player by Id
        /// </summary>
        /// <response code="200">Game player was successfully deleted</response>
        /// <response code="400">Unable to delete game player</response>
        /// <response code="404">Unable to find game player by given Id</response>
        [HttpDelete("{gamePlayerId}")]
        [ProducesResponseType(typeof(GamePlayerResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> DeleteGamePlayer(long gamePlayerId)
        {
            var userId = HttpContext.GetUserId();
            var gamePlayer = await _context.GamePlayers
                .Include(gp => gp.Game)
                .AsNoTracking()
                .SingleOrDefaultAsync(gp => gp.Game.IdentityUserId == userId && gp.Id == gamePlayerId);

            if(gamePlayer == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game player by given Id" } } });
            }

            _context.GamePlayers.Remove(gamePlayer);
            var deleted = await _context.SaveChangesAsync();
            if (deleted > 0)
                return Ok(_mapper.Map<GamePlayerResponse>(gamePlayer));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to delete game player: database error" } } });
        }
    }
}