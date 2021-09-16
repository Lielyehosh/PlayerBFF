using System;
using System.Threading;
using BFF.Service.Interfaces;
using BFF.Service.Models;
using Common.Models.DbModels;
using Common.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BFF.Service.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly IMongoDal _dal;

        // TODO - move out to external configuration
        private double JwtExpiredMinutes { get; set; } = 120;

        // LOGIN
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
        }

        [HttpGet]
        public IActionResult Test(CancellationToken ct)
        {
            var coll = _dal.GetCollection<User>();
            coll.InsertOne(new User()
            {
                Username = "John",
                EmailAddress = "John@aa.com",
                ModifyAt = DateTime.UtcNow,
                CreateAt = DateTime.UtcNow,
                IdNumber = "333222111"
            }, cancellationToken: ct);
            return Ok();
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