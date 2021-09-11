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

namespace PlayerBFF.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;
        
        // TODO - move out to external configuration
        private double JwtExpiredMinutes { get; set; } = 120;

        // LOGIN
        public AuthController(ILogger<AuthController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        
    
        [HttpPost]
        [Route("login")]
        public IActionResult LoginAsync([FromBody] LoginRequest loginReq, CancellationToken ct)
        {
            if (loginReq == null)
                return BadRequest("invalid request body");
            
            var user = AuthenticateUser(loginReq);

            if (user == null) return Unauthorized();
            
            var tokenString = GenerateJsonWebToken(loginReq);    
            return Ok(new LoginResponse()
            {
                Token = tokenString
            });

        }    
    
        private string GenerateJsonWebToken(LoginRequest loginReq)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Issuer"],
                    claims: new List<Claim>(),
                    expires: DateTime.UtcNow.AddMinutes(JwtExpiredMinutes),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);    
        }


        private UserModel AuthenticateUser(LoginRequest login)
        {    
    
            // TODO - move to different service and implement the logic
            if (login.Username == "Liel")    
            {
                _logger.LogInformation("User authentication successes");
                return new UserModel
                {
                    Username = "Liel Test", 
                    EmailAddress = "test.liel@gmail.com"
                };    
            }    
            return null;    
        }
        
        
    }
}