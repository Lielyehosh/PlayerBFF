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
        
        public AuthResponse Login(LoginRequest request, CancellationToken ct)
        {
            var grpcRes = _authMsClient.GrpcClient.AuthUserAsync(new AuthUserRequest()
            {
                Email = request.Email,
                Password = request.Password
            }, cancellationToken: ct).ResponseAsync.Result;
            if (!grpcRes.Success) 
                return new AuthResponse()
                {
                    Error = grpcRes.Error,
                    Success = false,
                    Token = null
                };
            try
            {
                return new AuthResponse()
                {
                    Token = GenerateJsonWebToken(grpcRes.User),
                    Error = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token");
                return new AuthResponse()
                {
                    Token = null,
                    Error = "Failed to generate token",
                    Success = false
                };
            }
        }

        public AuthResponse Register(RegisterRequest registerReq, CancellationToken ct)
        {
            var grpcRes = _authMsClient.GrpcClient.RegisterUserAsync(new RegisterUserRequest()
            {
                Email = registerReq.Email,
                Username = registerReq.FullName,
                Password = registerReq.Password
            }, cancellationToken: ct).ResponseAsync.Result;
            if (!grpcRes.Success) 
                return new AuthResponse()
                {
                    Error = grpcRes.Error,
                    Success = false,
                    Token = null
                };
            try
            {
                return new AuthResponse()
                {
                    Token = GenerateJsonWebToken(grpcRes.User),
                    Error = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token");
                return new AuthResponse()
                {
                    Token = null,
                    Error = "Failed to generate token",
                    Success = false
                };
            }
        }

        private string GenerateJsonWebToken(UserData user)    
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: new []{
                    new Claim("email", user.Email),
                    new Claim("username", user.Username),},
                expires: DateTime.UtcNow.AddMinutes(JwtExpiredMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);    
        }

    }
}