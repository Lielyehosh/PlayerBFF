using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using AuthMS;
using AuthService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlayerBFF.Interfaces;
using LoginRequest = PlayerBFF.Models.LoginRequest;
using LoginResponse = PlayerBFF.Models.LoginResponse;

namespace PlayerBFF.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;
        private readonly IAuthMsClient _authMsClient;

        public AuthService(
            IConfiguration config, 
            ILogger<AuthService> logger, 
            IAuthMsClient authMsClient)
        {
            _config = config;
            _logger = logger;
            _authMsClient = authMsClient;
            _logger.LogDebug("AuthService.Service Created");
        }

        private double JwtExpiredMinutes { get; set; } = 120;
        
        public LoginResponse LoginAsync(LoginRequest request, CancellationToken ct)
        {
            var res = _authMsClient.GrpcClient.LoginAsync(new AuthMS.LoginRequest()
            {
                Name = "Test"
            }).ResponseAsync.Result;
            var success = AuthenticateUser(request);
            if (!success) return null;
            try
            {
                return new LoginResponse()
                {
                    Token = GenerateJsonWebToken(request)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token");
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