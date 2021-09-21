using GameMs;
using Common.Utils;
using Common.Utils.Settings;
using GameService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Models
{
    public static class GameMsClientInjector
    {
        public static void AddGameMsClient(this IServiceCollection serviceCollection,
            GrpcClientSettings grpcSettings)
        {
            var hasGrpcClient = false;

            if (grpcSettings != null)
            {
                serviceCollection.AddSingleton(services =>
                    GetGameGrpcClient(grpcSettings));
                hasGrpcClient = true;
            }

            serviceCollection.AddSingleton<IGameMsClient>(services =>
                new GameMsClient(hasGrpcClient ? services.GetService<Game.GameClient>() : null));
        }

        /// <summary>
        /// Get Game Grpc Client for console apps
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static Game.GameClient GetGameGrpcClient(GrpcClientSettings settings)
        {
            return new Game.GameClient(GrpcCommon.GetGrpcChannel(settings));
        }
    }
}