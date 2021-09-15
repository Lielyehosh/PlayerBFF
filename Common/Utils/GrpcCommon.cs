using System;
using System.Net.Http;
using Common.Utils.Settings;
using Grpc.Net.Client;

namespace Common.Utils
{
    public static class GrpcCommon
    {
        public static GrpcChannel GetGrpcChannel(GrpcClientSettings settings)
        {
            HttpClientHandler httpClientHandler;
            if (settings.IgnoreSsl)
            {
                Console.WriteLine("WARNING: Disable SSL for microservice communication");
                AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }
            else
            {
                httpClientHandler = new HttpClientHandler();
            }

            return GrpcChannel.ForAddress(settings.Address, new GrpcChannelOptions
            {
                HttpHandler = httpClientHandler,
                MaxReceiveMessageSize = 20_000_000,
                MaxSendMessageSize = 20_000_000,
            });
        }
    }
}