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
        private TDatabase _instance;
        private static bool _isFirst = true;
        private readonly ILogger<TestDb<MongoDatabaseBase>> _logger;

        public TestDb()
        {
            _instance = new TDatabase();
            _logger = TestsCommon.Logger<TestDb<MongoDatabaseBase>>();
        }

        public TDatabase Database => _instance;
        
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
            var dropDb = _isFirst;
            _isFirst = false;
            if (dropDb)
            {
                await client.DropDatabaseAsync(settings.DatabaseName);
            }
            else
            {
                var db = client.GetDatabase(dbName);
                var collections = await (await db.ListCollectionsAsync()).ToListAsync();
                var batchSize = 20;
                for (var i = 0; i < Math.Ceiling((double)collections.Count / batchSize); i++)
                {
                    var batch = collections.Skip(i * batchSize).Take(batchSize);
                    await Task.WhenAll(batch.Select(async (coll) =>
                    {
                        var collName = coll["name"].ToString();
                        var collection = db.GetCollection<BsonDocument>(collName);
                        await collection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);
                    }));
                }
            }

            _logger.LogDebug("Done Clear  {DbName} db", dbName);
            var connected = await _instance.InitializeAsync(new DatabaseInitializeOptions
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