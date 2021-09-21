using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GameMs.Services
{
    public class GameGrpcService: Game.GameBase
    {
        private readonly ILogger<GameGrpcService> _logger;

        public GameGrpcService(ILogger<GameGrpcService> logger)
        {
            _logger = logger;
            _logger.LogInformation("Game GRPC service cons");
        }

        public override async Task JoinGame(JoinGameRequest request, IServerStreamWriter<JoinGameResponse> responseStream, ServerCallContext context)
        {
            for (var i = 0; i < 30; i++)
            {
                await responseStream.WriteAsync(new JoinGameResponse()
                {
                    Message = $" Hello {i}"
                });
                await Task.Delay(1000);
            }
        }
    }
}