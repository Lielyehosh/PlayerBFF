using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AuthMS.Services
{
    public class AuthenticationGrpcService : Authentication.AuthenticationBase
    {
        private readonly ILogger<AuthenticationGrpcService> _logger;

        public AuthenticationGrpcService(ILogger<AuthenticationGrpcService> logger)
        {
            _logger = logger;
            _logger.LogInformation("Auth GRPC service is up");
        }

        public override async Task<AuthUserResponse> AuthUser(AuthUserRequest request, ServerCallContext context)
        {
            _logger.LogDebug("Auth user request via GRPC");
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