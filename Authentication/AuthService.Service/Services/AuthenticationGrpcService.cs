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

        public override Task<AuthUserResponse> AuthLoginUser(AuthLoginUserRequest request, ServerCallContext context)
        {
            return _userService.LoginUserAsync(request.Email, request.Password, context.CancellationToken);
        }

        public override Task<AuthUserResponse> AuthRegisterUser(AuthRegisterUserRequest request,
            ServerCallContext context)
        {
            return _userService.RegisterNewUserAsync(new User()
            {
                Username = request?.Username,
                HashedPassword = request?.Password,
                EmailAddress = request?.Email.ToLower()
            }, context.CancellationToken);
        }

    }
}