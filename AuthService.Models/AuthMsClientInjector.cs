using AuthMS;
using Common;
using Common.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Models
{
    public static class AuthMsClientInjector
    {
        public static void AddAuthMsClient(this IServiceCollection serviceCollection,
            GrpcClientSettings grpcSettings)
        {
            var hasGrpcClient = false;

            if (grpcSettings != null)
            {
                serviceCollection.AddSingleton(services =>
                    GetAuthGrpcClient(grpcSettings));
                hasGrpcClient = true;
            }

            serviceCollection.AddSingleton<IAuthMsClient>(services =>
                new AuthMsClient(hasGrpcClient ? services.GetService<Authentication.AuthenticationClient>() : null));
        }

        /// <summary>
        /// Get Authentication Grpc Client for console apps
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static Authentication.AuthenticationClient GetAuthGrpcClient(GrpcClientSettings settings)
        {
            return new Authentication.AuthenticationClient(GrpcCommon.GetGrpcChannel(settings));
        }
    }
}