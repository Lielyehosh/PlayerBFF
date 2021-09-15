using System;
using MongoDB.Driver;

namespace Common.Utils.Interfaces
{
    /// <summary>
    /// interface for collection details.
    /// </summary>
    public interface ICollectionDetails
    {
        Type ModelType { get; }
        string CollectionName { get; }
        CreateCollectionOptions Options { get; }
    }
}