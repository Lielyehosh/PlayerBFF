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

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            _logger.LogDebug("Login request via GRPC");
            await Task.Delay(1000);
            return new LoginResponse
            {
                Message = "Success from liel"
            };
        }
    }
}