using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlayerBFF.Interfaces;
using PlayerBFF.Models;

namespace PlayerBFF.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        // TODO - move out to external configuration
        private double JwtExpiredMinutes { get; set; } = 120;

        // LOGIN
        public AuthController(
            ILogger<AuthController> logger, 
            IConfiguration config,
            IAuthService authService)
        {
            _logger = logger;
            _config = config;
            _authService = authService;
        }
        
    
        [HttpPost]
        [Route("login")]
        public IActionResult LoginAsync([FromBody] LoginRequest loginReq, CancellationToken ct)
        {
            if (loginReq == null)
                return BadRequest("invalid request body");

            var response = _authService.LoginAsync(loginReq, ct);
            if (response == null)
                return Unauthorized();
            
            return Ok(response);

        }

    }
}