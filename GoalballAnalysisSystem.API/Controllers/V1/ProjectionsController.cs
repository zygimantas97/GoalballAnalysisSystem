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
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Extensions;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;

namespace GoalballAnalysisSystem.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectionsController : AbstractController
    {
        public ProjectionsController(DataContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        /// <summary>
        /// Returns all user's projections by game Id
        /// </summary>
        /// <response code="200">Returns all projections</response>
        [HttpGet("ByGame/{gameId}")]
        [ProducesResponseType(typeof(ProjectionResponse), 200)]
        public async Task<IActionResult> GetProjectionsByGameId(long gameId)
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<ProjectionResponse>>(await _context.Projections
                .Include(p => p.Game)
                .Where(p => p.Game.IdentityUserId == userId && p.GameId == gameId)
                .ToListAsync()));
        }

        /// <summary>
        /// Returns all user's projections by game player Id
        /// </summary>
        /// <response code="200">Returns all projections</response>
        [HttpGet("ByGamePlayer/{gamePlayerId}")]
        [ProducesResponseType(typeof(ProjectionResponse), 200)]
        public async Task<IActionResult> GetProjectionsByGamePlayerId(long gamePlayerId)
        {
            var userId = HttpContext.GetUserId();
            return Ok(_mapper.Map<List<ProjectionResponse>>(await _context.Projections
                .Include(p => p.Game)
                .Where(p => p.Game.IdentityUserId == userId && p.GamePlayerId == gamePlayerId)
                .ToListAsync()));
        }

        /// <summary>
        /// Returns user's projection by Id
        /// </summary>
        /// <response code="200">Returns projection by Id</response>
        /// <response code="404">Unable to find projection by given Id</response>
        [HttpGet("{projectionId}")]
        [ProducesResponseType(typeof(ProjectionResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetProjection(long projectionId)
        {
            var userId = HttpContext.GetUserId();
            var projection = await _context.Projections
                .Include(p => p.Game)
                .SingleOrDefaultAsync(p => p.Game.IdentityUserId == userId && p.Id == projectionId);

            if (projection == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find projection by given Id" } } });
            }

            return Ok(_mapper.Map<ProjectionResponse>(projection));
        }

        /// <summary>
        /// Updates user's projection by Id
        /// </summary>
        /// <response code="204">Projection was successfully updated</response>
        /// <response code="400">Unable to update projection</response>
        /// <response code="404">Unable to find projection by given Id</response>
        [HttpPut("{projectionId}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> UpdateProjectionw(long projectionId, ProjectionRequest request)
        {
            var userId = HttpContext.GetUserId();
            var projection = await _context.Projections
                .Include(p => p.Game)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Game.IdentityUserId == userId && p.Id == projectionId);

            if (projection == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find projection by given Id" } } });
            }

            var updateProjection = _mapper.Map<Projection>(request);
            updateProjection.Id = projectionId;
            updateProjection.GameId = projection.GameId;
            updateProjection.GamePlayerId = projection.GamePlayerId;

            _context.Projections.Update(updateProjection);
            var updated = await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates user's projection
        /// </summary>
        /// <response code="201">Projection was successfully created</response>
        /// <response code="400">Unable to create projection</response>
        /// <response code="404">Unable to find</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectionResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> CreateProjection(ProjectionRequest request)
        {
            var userId = HttpContext.GetUserId();

            var game = await _context.Games
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.IdentityUserId == userId && g.Id == request.GameId);
            if(game == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game by given Id" } } });
            }

            var userRole = HttpContext.GetUserRole();
            if(userRole == "RegularUser")
            {
                request.GamePlayerId = null;
            }
            else
            {
                if(request.GamePlayerId != null)
                {
                    var gamePlayer = await _context.GamePlayers
                        .Include(gp => gp.Game)
                        .AsNoTracking()
                        .SingleOrDefaultAsync(gp => gp.Game.IdentityUserId == userId && gp.Id == request.GamePlayerId);
                    if(gamePlayer == null)
                    {
                        return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find game player by given Id" } } });
                    }  
                }
            }

            var projection = _mapper.Map<Projection>(request);

            _context.Projections.Add(projection);
            var created = await _context.SaveChangesAsync();
            return CreatedAtAction("GetProjection", new { projectionId = projection.Id }, _mapper.Map<ProjectionResponse>(projection));
        }

        /// <summary>
        /// Deletes user's projection by Id
        /// </summary>
        /// <response code="200">Projection was successfully deleted</response>
        /// <response code="400">Unable to delete projection</response>
        /// <response code="404">Unable to find projection by given Id</response>
        [HttpDelete("{projectionId}")]
        [ProducesResponseType(typeof(ProjectionResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> DeleteProjection(long projectionId)
        {
            var userId = HttpContext.GetUserId();
            var projection = await _context.Projections
                .Include(p => p.Game)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Game.IdentityUserId == userId && p.Id == projectionId);

            if (projection == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to find projection by given Id" } } });
            }

            _context.Projections.Remove(projection);
            var deleted = await _context.SaveChangesAsync();
            return Ok(_mapper.Map<ProjectionResponse>(projection));
        }
    }
}
