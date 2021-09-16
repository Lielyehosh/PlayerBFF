using System;
using System.Threading;
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
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly IMongoDal _dal;

        private double JwtExpiredMinutes { get; set; } = 120;

        public AuthController(
            ILogger<AuthController> logger, 
            IConfiguration config,
            IAuthService authService,
            IMongoDal dal)
        {
            _logger = logger;
            _config = config;
            _authService = authService;
            _dal = dal;
            JwtExpiredMinutes = Convert.ToDouble(config["Jwt:ExpireTime"]);
        }
        
    
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult LoginAsync([FromBody] LoginRequest loginReq, CancellationToken ct)
        {
            if (loginReq == null)
                return BadRequest("invalid request body");

            var response = _authService.LoginAsync(loginReq, ct);
            if (response == null)
                return Unauthorized();
            
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public IActionResult RegisterAsync([FromBody] RegisterRequest registerReq, CancellationToken ct)
        {
            if (registerReq == null)
                return BadRequest("invalid request body");

            var response = _authService.RegisterAsync(registerReq, ct);
            if (response == null)
                return Unauthorized();
            
            return Ok(response);
        }
        
    }
}