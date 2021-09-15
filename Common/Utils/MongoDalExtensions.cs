using Common.Utils.Extensions;
using Common.Utils.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;

namespace Common.Utils
{
    public static class MongoDalExtensions
    {
        public static void AddMongoDal<TDatabase>(this IServiceCollection services) 
            where TDatabase : MongoDatabaseBase, new()
        {
            AddMongoDal<TDatabase, MongoDal>(services);
        }
        public static void AddMongoDal<TDatabase, TMongoDal>(this IServiceCollection services)
            where TDatabase: MongoDatabaseBase, new() 
            where TMongoDal: MongoDal
        {
            // mongodb ignore extra elements
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
            
            services.AddMongoDb<TDatabase>();
            if (typeof(TDatabase) != typeof(MongoDatabaseBase))
                services.AddSingleton<MongoDatabaseBase>(provider => provider.GetService<TDatabase>());
            services.AddSingleton<TMongoDal>();
            if (typeof(TMongoDal) != typeof(MongoDal))
                services.AddSingleton<MongoDal>(provider => provider.GetService<TMongoDal>());
            services.AddSingleton<IMongoDal>(provider => provider.GetService<MongoDal>());
        }
    }
}