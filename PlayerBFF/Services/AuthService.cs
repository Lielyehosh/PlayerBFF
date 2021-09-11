using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlayerBFF.Interfaces;
using PlayerBFF.Models;

namespace PlayerBFF.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        public AuthService(
            IConfiguration config, 
            ILogger<AuthService> logger)
        {
            _config = config;
            _logger = logger;
            _logger.LogDebug("AuthService Created");
        }

        private double JwtExpiredMinutes { get; set; } = 120;
        
        public LoginResponse LoginAsync(LoginRequest request, CancellationToken ct)
        {
            var success = AuthenticateUser(request);
            if (!success) return null;
            try
            {
                return new LoginResponse()
                {
                    Token = GenerateJsonWebToken(request)
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to generate JWT token");
                return null;
            }
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


        private bool AuthenticateUser(LoginRequest login)
        {
            // TODO - implement a real validation
            if (login.Username == "Liel")
            {
                _logger.LogInformation("User authentication successes");
                return true;
            }    
            return false;
        }

    }
}