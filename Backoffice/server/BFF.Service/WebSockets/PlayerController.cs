using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AuthMS;
using AuthService.Models;
using BFF.Service.Extensions;
using GameMs;
using GameService.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BFF.Service.WebSockets
{
    [Route("ws/[controller]")]
    [Controller]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IGameMsClient _gameMsClient;

        public PlayerController(ILogger<PlayerController> logger,
            IGameMsClient gameMsClient)
        {
            _logger = logger;
            _gameMsClient = gameMsClient;
        }
        
        [HttpGet("join")]
        public async Task Join()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                _logger.LogDebug("Accept WebSocket request");
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                try
                {
                    await PlayTest(HttpContext, webSocket);
                }
                finally
                {
                    if (webSocket.CloseStatus != null)
                        await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription,
                            CancellationToken.None);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private async Task PlayTest(HttpContext httpContext, WebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var response = _gameMsClient.GrpcClient.JoinGame(new JoinGameRequest()
                {
                    Name = "Liel"
                }).ResponseStream;
                
                while (await response.MoveNext())
                {
                    _logger.LogDebug("response from grpc server-{Res}", response.Current);
                    await WriteToWebSocketBuffer(webSocket, response.Current);
                }

            }
        }


        // [HttpGet("join")]
        // public async Task Join()
        // {
        //     if (HttpContext.WebSockets.IsWebSocketRequest)
        //     {
        //         _logger.LogDebug("Accept WebSocket request");
        //         using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        //         try
        //         {
        //             await Play(HttpContext, webSocket);
        //         }
        //         finally
        //         {
        //             if (webSocket.CloseStatus != null)
        //                 await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription,
        //                     CancellationToken.None);
        //         }
        //     }
        //     else
        //     {
        //         HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //     }
        // }

        private async Task Play(HttpContext httpContext, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var username = httpContext.GetUserId();
            await WriteToWebSocketBuffer(webSocket, new WebSocketMessageBase()
            {
                Message = $"Hello {username}",
                Sender = "System"
            });
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.CloseStatus.HasValue)
                    break;
                var msgObj = ReadFromWebSocketBuffer<WebSocketMessageBase>(buffer, 0, result.Count);
                // handle the message
                _logger.LogDebug("Receive message from client-{Message}", msgObj.Message);
                await WriteToWebSocketBuffer(webSocket, new WebSocketMessageBase()
                {
                    Message = "Hello " + msgObj.Message,
                    Sender = "System"
                });
            }
            // await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        
        private static async Task WriteToWebSocketBuffer(WebSocket webSocket, object message)
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                var value = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
                await webSocket.SendAsync(value, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static TMessage ReadFromWebSocketBuffer<TMessage>(byte[] buffer, int index, int count)
        {
            var msgStr = Encoding.ASCII.GetString(buffer, 0, count);
            var msgObj = JsonSerializer.Deserialize<TMessage>(msgStr);
            return msgObj;
        }

        public class WebSocketMessageBase
        {
            public string Sender { get; set; }
            public string Message { get; set; }
        }
        
    }
}