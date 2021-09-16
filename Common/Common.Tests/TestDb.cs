using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Utils.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Common.Utils
{
    public class TestDb<TDatabase>
        where TDatabase: MongoDatabaseBase, new()
    {
        private TDatabase instance;
        private static bool isFirst = true;
        private readonly ILogger<TestDb<MongoDatabaseBase>> _logger;

        public TestDb()
        {
            instance = new TDatabase();
            _logger = TestsCommon.Logger<TestDb<MongoDatabaseBase>>();
        }

        public TDatabase Database => instance;
        
        public async Task ConnectAsync(string connectionString, string dbName = "test")
        {
            var mongoUrl = MongoUrl.Create(connectionString);
            var settings = new DatabaseInitializeOptions
            {
                DatabaseName = dbName, 
                ConnectionSettings = MongoClientSettings.FromUrl(mongoUrl)
            };
            var client = new MongoClient(settings.ConnectionSettings);
            _logger.LogDebug("Clear {DbName}  db", dbName);
            var dropDb = isFirst;
            isFirst = false;
            if (dropDb)
            {
                await client.DropDatabaseAsync(settings.DatabaseName);
            }
            else
            {
                var db = client.GetDatabase(dbName);
                var collections = await (await db.ListCollectionsAsync()).ToListAsync();
                var BATCH_SIZE = 20;
                for (var i = 0; i < Math.Ceiling((double)collections.Count / BATCH_SIZE); i++)
                {
                    var batch = collections.Skip(i * BATCH_SIZE).Take(BATCH_SIZE);
                    await Task.WhenAll(batch.Select(async (coll) =>
                    {
                        var collName = coll["name"].ToString();
                        var collection = db.GetCollection<BsonDocument>(collName);
                        await collection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);
                    }));
                }
            }

            _logger.LogDebug("Done Clear  {DbName} db", dbName);
            var connected = await instance.InitializeAsync(new DatabaseInitializeOptions
            {
                DatabaseName = dbName, 
                ConnectionSettings = MongoClientSettings.FromUrl(mongoUrl)
            });
            if (!connected) 
                throw new Exception("Cant connect to db");
            _logger.LogDebug("{DbName} db connected", dbName);
        }
    }
}