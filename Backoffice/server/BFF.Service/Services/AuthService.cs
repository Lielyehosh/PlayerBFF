using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public const string IdClaim = "id";
        public const string EmailClaim = "email";
        public const string UsernameClaim = "username";
        
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
        
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            var grpcRes = await _authMsClient.GrpcClient.AuthLoginUserAsync(new AuthLoginUserRequest()
            {
                Email = request.Email,
                Password = request.Password
            }, cancellationToken: ct);
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

        public async Task<AuthResponse> RegisterAsync(RegisterRequest registerReq, CancellationToken ct)
        {
            var grpcRes = await _authMsClient.GrpcClient.AuthRegisterUserAsync(new AuthRegisterUserRequest()
            {
                Email = registerReq.Email,
                Username = registerReq.FullName,
                Password = registerReq.Password
            }, cancellationToken: ct);
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

        public async Task<ResetPasswordResponse> ResetPasswordAsync(string userId, string pw, CancellationToken ct)
        {
            var grpcRes = await _authMsClient.GrpcClient.ResetPwAsync(new ResetPwRequest()
            {
                Password = pw,
                UserId = userId
            });
            if (!grpcRes.Success)
            {
                return new ResetPasswordResponse()
                {
                    Success = grpcRes.Success,
                    Error = grpcRes.Error
                };
            }

            return new ResetPasswordResponse()
            {
                Error = "",
                Success = grpcRes.Success
            };
        }

        private string GenerateJsonWebToken(UserData user)    
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: new [] {
                    new Claim(EmailClaim, user.Email),
                    new Claim(UsernameClaim, user.Username),
                    new Claim(IdClaim, user.Id),
                },
                expires: DateTime.UtcNow.AddMinutes(JwtExpiredMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);    
        }

    }
}