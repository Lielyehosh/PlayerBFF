using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Utils.Extensions;
using Common.Utils.Interfaces;
using Common.Utils.Settings;
using MongoDB.Driver;
using Serilog;

namespace Common.Utils
{
    /// <summary>
    /// Represent the DB, contains all collection definitions
    /// </summary>
    public class MongoDatabaseBase : IMongoDatabaseBase
    {
        /// <summary>
        /// DB reference
        /// </summary>
        protected IMongoDatabase MongoDb { get; private set; }
        
        /// <summary>
        /// Connection string.
        /// </summary>
        public MongoUrl MongoUrl { get; private set; }
        
        /// <summary>
        /// Mapping between the collection type and the collection details
        /// </summary>
        protected readonly Dictionary<Type, ICollectionDetails> _collectionDetailsTypeMap = new Dictionary<Type, ICollectionDetails>();

        public MongoDatabaseBase()
        {
        }
        
        public IMongoCollection<T> GetCollection<T>() where T : IDbModel
        {
            var desc = GetCollectionDetails<T>();
            return MongoDb.GetCollection<T>(desc.CollectionName);
        }

        private ICollectionDetails GetCollectionDetails<T>()
        {
            var dbType = typeof(T);
            if (!_collectionDetailsTypeMap.TryGetValue(dbType, out var collDetails))
                throw new InvalidOperationException($"Not support type {dbType.Name}");
            return collDetails;
        }

        /// <summary>
        /// Add a collection details.
        /// </summary>
        /// <param name="collDetails"></param>
        protected void AddCollectionDetails(ICollectionDetails collDetails)
        {
            if (_collectionDetailsTypeMap.ContainsKey(collDetails.ModelType))
                throw new InvalidOperationException($"Type {collDetails.ModelType} was already registered");

            _collectionDetailsTypeMap[collDetails.ModelType] = collDetails;
        }
        
        /// <summary>
        /// Initialize db connection
        /// </summary>
        /// <param name="initOptions"></param>
        /// <returns></returns>
        public async Task<bool> InitializeAsync(DatabaseInitializeOptions initOptions)
        {
            if (IsInitialized)
                throw new InvalidOperationException("Already initialized");

            var settings = initOptions.ConnectionSettings;
            var builder = new MongoUrlBuilder
            {
                DatabaseName = initOptions.DatabaseName,
                Servers = settings.Servers,
                UseTls = settings.UseTls
            };
            MongoUrl = builder.ToMongoUrl(); 
            Log.Information("Connecting to database {ConnectionString}", MongoUrl.ToPublicString());
            var client = new MongoClient(settings);
            MongoDb = client.GetDatabase(MongoUrl.DatabaseName);
            
            
            // Create collections with specific options if required
            foreach (var collection in _collectionDetailsTypeMap.Values.Where(v => v.Options != null))
            {
                try
                {
                    await MongoDb.CreateCollectionAsync(
                        collection.CollectionName, collection.Options);
                }
                catch (MongoCommandException e)
                {
                    // Catcher for an already existing collection.
                    if (!e.ErrorMessage.Contains("collection already exists"))
                        throw;
                }
            }

            // TODO - Create indexes.
            
            IsInitialized = true;
            return true;
        }

        public bool IsInitialized { get; private set; }
    }
}