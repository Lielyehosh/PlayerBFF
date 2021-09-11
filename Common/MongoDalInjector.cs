using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Common
{
    public static class MongoDalInjector
    {
        public static void AddMongoDal(this IServiceCollection serviceCollection,
            string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                var settings = MongoClientSettings.FromConnectionString(connectionString);
                var client = new MongoClient(settings);
                var database = client.GetDatabase("game");
                serviceCollection.AddSingleton<IMongoDal>(services =>
                    new MongoDal(database));
            }

        }
    }
}