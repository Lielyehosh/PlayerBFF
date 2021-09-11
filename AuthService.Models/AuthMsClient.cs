using AuthMS;

namespace AuthService.Models
{
    public interface IAuthMsClient
    {
        Authentication.AuthenticationClient GrpcClient { get; }
    }
    
    public class AuthMsClient : IAuthMsClient
    {
        public AuthMsClient(Authentication.AuthenticationClient grpcClient)
        {
            GrpcClient = grpcClient;
        }

        public Authentication.AuthenticationClient GrpcClient { get; }
    }
}