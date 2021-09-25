using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace GameMs.Services
{
    public class PlayerConnection : IAsyncDisposable
    {
        private readonly IServerStreamWriter<GameUpdate> _writeStream;
        public static PlayerConnection InitializeNewPlayerConnection(JoinGameRequest request, IServerStreamWriter<GameUpdate> responseStream)
        {
            return new PlayerConnection(request.UserId, responseStream);
        }
     
        public string UserId { get; }
        
        private PlayerConnection(string userId, IServerStreamWriter<GameUpdate> writeStream)
        {
            UserId = userId;
            _writeStream = writeStream;
        }

        public async void SendUpdateAsync(GameUpdate update)
        {
            await this._writeStream.WriteAsync(update);
        }
        
        
        // private void Start()
        // {
        //     // Start the running tasks.
        //     Task.Run(async () =>
        //     {
        //         var neverCompleteTask = new TaskCompletionSource<bool>();
        //
        //         // Mark that the task is stopped.
        //         _mStoppedTcs.SetResult(true);
        //     }, _mStopCts.Token);
        // }
        //
        // private readonly TaskCompletionSource<bool> _mStoppedTcs = new TaskCompletionSource<bool>();
        // private readonly CancellationTokenSource _mStopCts = new CancellationTokenSource();
        //
        public async ValueTask DisposeAsync()
        {
            //_mStopCts.Cancel();

            // Wait for stop.
            //await _mStoppedTcs.Task.ConfigureAwait(false);
        }
    }
}