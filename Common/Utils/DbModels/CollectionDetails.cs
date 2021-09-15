using System;
using Common.Utils.Interfaces;
using MongoDB.Driver;

namespace Common.Utils.DbModels
{
    public class CollectionDetails<T> : ICollectionDetails
        where T : IDbModel
    {
        public CollectionDetails(string collectionName, CreateCollectionOptions options = null)
        {
            CollectionName = collectionName;
            Options = options;
            ModelType = typeof(T);
        }

        public Type ModelType { get; }
        public string CollectionName { get; }
        public CreateCollectionOptions Options { get; }
    }
}