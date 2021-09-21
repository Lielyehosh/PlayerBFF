using GameMs;

namespace GameService.Models
{
    public interface IGameMsClient
    {
        Game.GameClient GrpcClient { get; }
    }
    
    public class GameMsClient : IGameMsClient
    {
        public GameMsClient(Game.GameClient grpcClient)
        {
            GrpcClient = grpcClient;
        }

        public Game.GameClient GrpcClient { get; }
    }
}