using System.Threading.Tasks;
using Common;
using Common.DbModels;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AuthMS.Services
{
    public class AuthenticationGrpcService : Authentication.AuthenticationBase
    {
        private readonly ILogger<AuthenticationGrpcService> _logger;
        private readonly IMongoDal _dal;

        public AuthenticationGrpcService(ILogger<AuthenticationGrpcService> logger,
            IMongoDal dal)
        {
            _logger = logger;
            _dal = dal;
            _logger.LogInformation("Auth GRPC service is up");
        }

        public override async Task<AuthUserResponse> AuthUser(AuthUserRequest request, ServerCallContext context)
        {
            _logger.LogDebug("Auth user request via GRPC");
            // var user = await _dal.FindUserByIdAsync("123123123", context.CancellationToken);
            // TODO - implement a real validation
            await Task.Delay(1000);
            if (request.Username == "Liel")
            {
                _logger.LogInformation("User authentication successes");
                return new AuthUserResponse()
                {
                    Success = true
                };
            }
            return new AuthUserResponse()
            {
                Success = false
            };
        }
    }
}