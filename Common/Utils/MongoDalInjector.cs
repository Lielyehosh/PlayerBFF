namespace Common.Utils
{
    public static class MongoDalInjector
    {
        // public static void AddMongoDal(this IServiceCollection serviceCollection,
        //     string connectionString)
        // {
        //     if (string.IsNullOrEmpty(connectionString)) 
        //         return;
        //     
        //     // create mongo client 
        //     var settings = MongoClientSettings.FromConnectionString(connectionString);
        //     var client = new MongoClient(settings);
        //     var database = client.GetDatabase("game");
        //     
        //     // Create collections with specific options if required - do this only after database version was validated..
        //     foreach (var collection in m_collections.Values.SelectMany(v => v.Values).Where(v => v.Options != null))
        //     {
        //         if (!collection.OnlySubCollections)
        //         {
        //             try
        //             {
        //                 await MongoDb.CreateCollectionAsync(
        //                     collection.CollectionName, collection.Options);
        //             }
        //             catch (MongoCommandException e)
        //             {
        //                 // Cater for an already existing collection.
        //                 if (!e.ErrorMessage.Contains("collection already exists"))
        //                     throw;
        //             }
        //         }
        //
        //         foreach (var sub in collection.SubCollections)
        //         {
        //             try
        //             {
        //                 await MongoDb.CreateCollectionAsync(
        //                     collection.GetSubCollectionName(sub), collection.Options);
        //             }
        //             catch (MongoCommandException e)
        //             {
        //                 // Cater for an already existing collection.
        //                 if (!e.ErrorMessage.Contains("sub collection already exists"))
        //                     throw;
        //             }
        //         }
        //     }
        //
        //     // Create indexes - do this only after database version was validated.
        //     if (!initOptions.DeferIndexCreation)
        //         await EnsureIndexesAsync();
        //
        //     // Perform user specific tasks.
        //     await OnInitializedAsync();
        //
        //     IsInitialized = true;
        //
        //     return true;
        //     
        //     serviceCollection.AddSingleton<IMongoDal>(services =>
        //         new MongoDal(database));
        // }
    }
}