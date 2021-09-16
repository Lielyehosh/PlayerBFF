using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Models.DbModels;
using Common.Utils.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace AuthMS.Services
{
    public class AuthenticationGrpcService : Authentication.AuthenticationBase
    {
        private readonly ILogger<AuthenticationGrpcService> _logger;
        private readonly IUserService _userService;
        private readonly IMongoDal _dal;

        public AuthenticationGrpcService(
            ILogger<AuthenticationGrpcService> logger, 
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _logger.LogInformation("Auth GRPC service is up");
        }

        public override async Task<AuthUserResponse> AuthUser(AuthUserRequest request, ServerCallContext context)
        {
            _logger.LogDebug("Auth user request via GRPC");
            var user = await _userService.FindUserByEmailAsync(request.Email, context.CancellationToken);
            // TODO - implement a real validation
            if (request.Password != null)
            {
                _logger.LogInformation("User authentication successes");
                return new AuthUserResponse {Success = true};
            }
            return new AuthUserResponse {Success = false};
        }

        public override Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request,
            ServerCallContext context)
        {
            return _userService.RegisterNewUserAsync(new User()
            {
                Username = request?.Username,
                IdNumber = request?.IdNumber,
                EmailAddress = request?.Email
            }, context.CancellationToken);
        }
    }
}