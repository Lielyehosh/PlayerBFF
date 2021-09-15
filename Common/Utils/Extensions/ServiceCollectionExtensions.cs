using Common.Utils.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register MongoDB interfaces
        /// </summary>
        public static IServiceCollection AddMongoDb<TDatabase>(this IServiceCollection services)
            where TDatabase : class, IMongoDatabaseBase, new()
        {
            services.AddSingleton<TDatabase>(provider => new TDatabase());
            services.AddSingleton<IMongoDatabaseBase>(provider => provider.GetService<TDatabase>());
            return services;
        }
    }
}