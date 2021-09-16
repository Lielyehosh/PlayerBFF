using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Common.Utils
{
    public static class TestsCommon
    {
        public static async Task<T> TaskFromResult<T>(T result)
        {
            await Task.Delay(100);
            return result;
        } 
        public static AsyncUnaryCall<T> AsyncUnaryFromResult<T>(T result)
        {
            return new AsyncUnaryCall<T>(
                Task.Delay(10).ContinueWith(t => result),
                Task.FromResult(Metadata.Empty),
                () => Status.DefaultSuccess,
                () => Metadata.Empty, () => {}
            );
        }
        public static AsyncUnaryCall<T> AsyncUnaryFromResult<T>(Task<T> task)
        {
            return new AsyncUnaryCall<T>(
                task,
                Task.FromResult(Metadata.Empty),
                () => Status.DefaultSuccess,
                () => Metadata.Empty, () => {}
            );
        }
        public static AsyncUnaryCall<T> AsyncUnaryFromResult<T>()
            where T: new()
        {
            return AsyncUnaryFromResult(new T());
        }

        class ConsoleLogger<T> : ILogger<T>
        {
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Console.WriteLine(logLevel + " " + typeof(T).Name + " " + formatter(state, exception));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new Moq.Mock<IDisposable>().Object;
            }
        }
        public static ILogger<T> Logger<T>()
        {
            return new ConsoleLogger<T>();
        }
    }
}