using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoalballAnalysisSystem.API.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <response code="200">A new user was successfully registered in the system</response>
        /// <response code="400">Unable to register a new user</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Register(UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => new ErrorModel { Message = e.ErrorMessage })).ToList()
                });
            }

            var _authResponse = await _identityService.RegisterAsync(request.Email, request.Password, request.UserName);
            if (!_authResponse.Success)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = _authResponse.Errors.Select(e => new ErrorModel { Message = e }).ToList()
                });
            }

            return Ok(new AuthenticationResponse
            {
                Token = _authResponse.Token,
                RefreshToken = _authResponse.RefreshToken
            });
        }

        /// <summary>
        /// Logs an user in the system
        /// </summary>
        /// <response code="200">>An user was successfully logged in the system</response>
        /// <response code="400">Unable to log an user in the system</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Login(UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => new ErrorModel { Message = e.ErrorMessage })).ToList()
                });
            }

            var _authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!_authResponse.Success)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = _authResponse.Errors.Select(e => new ErrorModel { Message = e }).ToList()
                });
            }

            return Ok(new AuthenticationResponse
            {
                Token = _authResponse.Token,
                RefreshToken = _authResponse.RefreshToken
            });
        }

        /// <summary>
        /// Refreshes an authentication token (JWT)
        /// </summary>
        /// <response code="200">An authentication token was successfully refreshed</response>
        /// <response code="400">Unable to refresh an authentication token</response>
        [HttpPost("RefreshToken")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var _authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
            if (!_authResponse.Success)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = _authResponse.Errors.Select(e => new ErrorModel { Message = e }).ToList()
                });
            }

            return Ok(new AuthenticationResponse
            {
                Token = _authResponse.Token,
                RefreshToken = _authResponse.RefreshToken
            });
        }
    }
}
