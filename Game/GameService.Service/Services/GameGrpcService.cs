using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GameMs.Services
{
    public class GameGrpcService: Game.GameBase
    {
        private readonly ILogger<GameGrpcService> _logger;
        private readonly IDictionary<string, PlayerConnection> _players;

        public GameGrpcService(ILogger<GameGrpcService> logger)
        {
            _logger = logger;
            _players = new Dictionary<string, PlayerConnection>();
            _logger.LogInformation("Game GRPC service cons");
        }

        public override async Task JoinGame(JoinGameRequest request, IServerStreamWriter<GameUpdate> responseStream, ServerCallContext context)
        {
            await CreateNewPlayerConnectionAsync(request, responseStream);
        }

        private async Task CreateNewPlayerConnectionAsync(JoinGameRequest request, IServerStreamWriter<GameUpdate> responseStream)
        {
            _logger.LogDebug("Creating new player connection");
            try
            {
                var player = PlayerConnection.InitializeNewPlayerConnection(request, responseStream);
                if (!_players.TryAdd(request.UserId, player))
                {
                    await player.DisposeAsync();
                }
                else
                {
                    _logger.LogDebug("New player connection added");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to add new player connection");
                throw;
            }
        }

        public override async Task<PlayerMadeMoveResponse> PlayerMadeMove(PlayerMadeMoveRequest request, ServerCallContext context)
        {
            if (_players.TryGetValue(request.UserId, out var playerConnection))
            {
                playerConnection.SendUpdateAsync(new GameUpdate()
                {
                    Update = $"{request.UserId} made move {request.Move} successfully"
                });
                return new PlayerMadeMoveResponse() {Success = true};
            }
            return new PlayerMadeMoveResponse() {Success = false};
        }


        // public override async Task Play(IAsyncStreamReader<PlayMoveRequest> requestStream, IServerStreamWriter<PlayMoveResponse> responseStream, ServerCallContext context)
        // {
        //     ListenForPlayMovesAsync(requestStream, responseStream, context).Start();
        // }
        //
        // private async Task ListenForPlayMovesAsync(IAsyncStreamReader<PlayMoveRequest> requestStream,
        //     IServerStreamWriter<PlayMoveResponse> serverStreamWriter, ServerCallContext context)
        // {
        //     while (await requestStream.MoveNext(context.CancellationToken))
        //     {
        //         await HandlePlayMoveMessageAsync(requestStream.Current, serverStreamWriter);
        //     }
        // }
        //
        // private async Task HandlePlayMoveMessageAsync(PlayMoveRequest playMove,
        //     IServerStreamWriter<PlayMoveResponse> serverStreamWriter)
        // {
        //     _logger.LogDebug("Handling move from {User}", playMove.Player);
        //     if (playMove.Move == "A")
        //     {
        //         await serverStreamWriter.WriteAsync(new PlayMoveResponse()
        //         {
        //             Result = "Confirm A"
        //         });
        //     }
        // }
    }
}