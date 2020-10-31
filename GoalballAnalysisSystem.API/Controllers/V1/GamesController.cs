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
    public class GamesController : AbstractController
    {
        public GamesController(DataContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
        
        /// <summary>
        /// Returns all user's games
        /// </summary>
        /// <response code="200">Returns all games</response>
        [HttpGet]
        [ProducesResponseType(typeof(GameResponse), 200)]
        public async Task<IActionResult> GetGames()
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<GameResponse>>(await _context.Games.Where(g => g.IdentityUserId == userId).ToListAsync()));
        }

        /// <summary>
        /// Returns user's game by Id
        /// </summary>
        /// <response code="200">Returns game by Id</response>
        /// <response code="404">Unable to find game by given Id</response>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(GameResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetGame(int gameId)
        {
            var userId = HttpContext.GetUserId();
            var game = await _context.Games
                .SingleOrDefaultAsync(g => g.Id == gameId && g.IdentityUserId == userId);

            if (game == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find user's game by given Id" } } });
            }

            return Ok(_mapper.Map<GameResponse>(game));
        }

        /// <summary>
        /// Updates user's game by Id
        /// </summary>
        /// <response code="204">Game was successfully updated</response>
        /// <response code="400">Unable to update game</response>
        /// <response code="404">Unable to find</response>
        [HttpPut("{gameId}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> UpdateGame(int gameId, GameRequest request)
        {
            var userId = HttpContext.GetUserId();
            var game = await _context.Games
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == gameId && g.IdentityUserId == userId);

            if(game == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find user's game by given Id" } } });
            }

            var updateGame = _mapper.Map<Game>(request);
            updateGame.Id = gameId;
            updateGame.IdentityUserId = userId;
            updateGame.Date = game.Date;
            updateGame.HomeTeamId = game.HomeTeamId;
            updateGame.GuestTeamId = game.GuestTeamId;

            _context.Games.Update(updateGame);
            var updated = await _context.SaveChangesAsync();
            if (updated > 0)
                return NoContent();
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to update user's game: database error" } } });
        }

        /// <summary>
        /// Creates user's game
        /// </summary>
        /// <response code="201">Game was successfully created</response>
        /// <response code="400">Unable to create game</response>
        /// <response code="404">Unable to find team by given Id</response>
        [HttpPost]
        [ProducesResponseType(typeof(GameResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> CreateGame(GameRequest request)
        {
            var userId = HttpContext.GetUserId();
            
            var userRole = HttpContext.GetUserRole();
            if (userRole == "RegularUser")
            {
                request.HomeTeamId = null;
                request.GuestTeamId = null;
            }
            else
            {
                if (request.HomeTeamId != null)
                {
                    var homeTeam = await _context.Teams
                        .AsNoTracking()
                        .SingleOrDefaultAsync(t => t.Id == request.HomeTeamId && t.IdentityUserId == userId);
                    if (homeTeam == null)
                    {
                        return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find team by given Id" } } });
                    }
                }
                if (request.GuestTeamId != null)
                {
                    var guestTeam = await _context.Teams
                        .AsNoTracking()
                        .SingleOrDefaultAsync(t => t.Id == request.GuestTeamId && t.IdentityUserId == userId);
                    if (guestTeam == null)
                    {
                        return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find team by given Id" } } });
                    }
                }
            }

            var game = _mapper.Map<Game>(request);
            game.IdentityUserId = userId;
            game.Date = DateTime.Now;

            _context.Games.Add(game);
            var created = await _context.SaveChangesAsync();
            if (created > 0)
                return CreatedAtAction("GetGame", new { gameId = game.Id }, _mapper.Map<GameResponse>(game));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create game: database error" } } });
        }

        /// <summary>
        /// Deletes user's game by Id
        /// </summary>
        /// <response code="200">Game was successfully deleted</response>
        /// <response code="400">Unable to delete game</response>
        /// <response code="404">Unable to find game by given Id</response>
        [HttpDelete("{gameId}")]
        [ProducesResponseType(typeof(GameResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> DeleteGame(int gameId)
        {
            var userId = HttpContext.GetUserId();
            var game = await _context.Games
                .Include(g => g.GamePlayers)
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == gameId && g.IdentityUserId == userId);
            if (game == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game by given Id" } } });
            }

            _context.Games.Remove(game);
            var deleted = await _context.SaveChangesAsync();
            if (deleted > 0)
                return Ok(_mapper.Map<GameResponse>(game));
            return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to delete game: database error" } } });
        }
    }
}
