using System;
using System.Threading;
using System.Threading.Tasks;
using BFF.Service.Extensions;
using BFF.Service.Interfaces;
using BFF.Service.Models;
using Common.Models.DbModels;
using Common.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BFF.Service.Controllers
{
    [DisableCors]
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        private double JwtExpiredMinutes { get; set; } = 120;

        public AuthController(
            ILogger<AuthController> logger, 
            IConfiguration config,
            IAuthService authService)
        {
            _logger = logger;
            _config = config;
            _authService = authService;
            JwtExpiredMinutes = Convert.ToDouble(config["Jwt:ExpireTime"]);
        }
        
    
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginReq, CancellationToken ct)
        {
            _logger.LogDebug("Receive new login request");
            if (loginReq == null)
                return BadRequest("invalid request body");

            var response = await _authService.LoginAsync(loginReq, ct);
            if (response.Success) return Ok(response);
            
            _logger.LogDebug("Unauthorized user login request - {Error}", response.Error);
            return Unauthorized(response.Error);

        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest registerReq, CancellationToken ct)
        {
            _logger.LogDebug("Receive new register request");
            if (registerReq == null)
                return BadRequest("invalid request body");

            var response = await _authService.RegisterAsync(registerReq, ct);
            if (response.Success) return Ok(response);
            
            _logger.LogDebug("Unauthorized user register request - {Error}", response.Error);
            return Unauthorized(response.Error);

        }
        
        [HttpPost]
        [Route("reset-pass")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest body, CancellationToken ct)
        {
            _logger.LogDebug("Receive new reset password request");
            if (body == null)
                return BadRequest("invalid request body");

            var response = await _authService.ResetPasswordAsync(HttpContext.GetUserId(),body.Password, ct);
            if (response.Success) return Ok(response);
            
            _logger.LogDebug("Reset password request failed - {Error}", response.Error);
            return Problem(response.Error);
        }
        
    }
}