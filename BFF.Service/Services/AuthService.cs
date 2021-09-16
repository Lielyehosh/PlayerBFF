using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using AuthMS;
using AuthService.Models;
using BFF.Service.Controllers;
using BFF.Service.Interfaces;
using BFF.Service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = BFF.Service.Models.LoginRequest;
using LoginResponse = BFF.Service.Models.LoginResponse;

namespace BFF.Service.Services
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
            var grpcRes = _authMsClient.GrpcClient.AuthUserAsync(new AuthMS.AuthUserRequest()
            {
                Email = request.Email,
                Password = request.Password
            }, cancellationToken: ct).ResponseAsync.Result;
            if (!grpcRes.Success) return null;
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

        public RegisterResponse RegisterAsync(RegisterRequest registerReq, CancellationToken ct)
        {
            var grpcRes = _authMsClient.GrpcClient.RegisterUserAsync(new RegisterUserRequest()
            {
                Email = registerReq.Email,
                Username = registerReq.Username,
                IdNumber = registerReq.IdNumber
            }, cancellationToken: ct).ResponseAsync.Result;
            if (!grpcRes.Success) return null;
            try
            {
                return new RegisterResponse()
                {
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

    }
}