using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Utils.Interfaces;
using Common.Utils.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Common.Utils.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Initialize mongodb instance with the given connection string
        /// </summary>
        public static IApplicationBuilder UseMongoDb(this IApplicationBuilder app, string connectionString)
        {
            // Validate settings
            var mongoUrl = MongoUrl.Create(connectionString);
            // TODO - pass the db name in connection string
            // if (mongoUrl.DatabaseName == null)
            //     throw new ArgumentException("No database name specified in connection string", nameof(connectionString));

            var opts = new DatabaseInitializeOptions()
            {
                ConnectionSettings = MongoClientSettings.FromUrl(mongoUrl),
                DatabaseName = "game"
            };
            return app.UseMongoDb(opts);
        }

        /// <summary>
        /// Connect to mongodb instance explicitly from the service provider
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static Task UseMongoDbConnectAsync(this IServiceProvider provider, string connectionString)
        {
            var mongoUrl = MongoUrl.Create(connectionString);
            if (mongoUrl.DatabaseName == null)
                return Task.FromException(new ArgumentException("No database name specified in connection string", nameof(connectionString)));

            var opts = new DatabaseInitializeOptions()
            {
                ConnectionSettings = MongoClientSettings.FromUrl(mongoUrl),
                DatabaseName = mongoUrl.DatabaseName
            };
            var db = provider.GetService<IMongoDatabaseBase>();
            return db?.InitializeAsync(opts);
        }

        public class UseMongoDbData
        {
            public bool IsInit { get; set; }
            
            public DatabaseInitializeOptions Options { get; set; } 
            
            public SemaphoreSlim Semaphore { get; set; } = new SemaphoreSlim(1);
        }

        public static async Task ConnectAsync(this IApplicationBuilder app)
        {
            if(!app.Properties.TryGetValue("UsingMongoDB", out var dataObj))
                throw new InvalidOperationException("UseMongoDB not called");
            
            var data =  (UseMongoDbData) dataObj;
            if (data.IsInit) return;
            await data.Semaphore.WaitAsync();
            try
            {
                if (data.IsInit) return;
                var db = app.ApplicationServices.GetService<IMongoDatabaseBase>();
                await db.InitializeAsync(data.Options);
                data.IsInit = true;
            }
            finally
            {
                data.Semaphore.Release();
            }
        }
        /// <summary>
        /// Initialize mongodb instance with the given connection string
        /// </summary>
        public static IApplicationBuilder UseMongoDb(this IApplicationBuilder app, DatabaseInitializeOptions options)
        {
            if (app.Properties.TryGetValue("UsingMongoDB", out var wasUsing))
                throw new InvalidOperationException("Already called 'UseMongoDB'");
            var data = new UseMongoDbData
            {
                Options = options,
            };

            app.Properties["UsingMongoDB"] = data;

            //
            // Register the database asynchronously upon first request
            //
            
            // bool isInit = false;
            // var sem = new SemaphoreSlim(1);
            app.Use(async (ctx, next) =>
            {
                if (!data.IsInit)
                    await app.ConnectAsync();
                await next();
            });

            return app;
        }
    }
}