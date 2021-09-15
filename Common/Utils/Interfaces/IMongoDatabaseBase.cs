using System.Threading.Tasks;
using Common.Utils.Settings;
using MongoDB.Driver;

namespace Common.Utils.Interfaces
{
    public interface IMongoDatabaseBase
    {
        /// <summary>
        /// Initialize connection to the database
        /// </summary>
        /// <param name="initOptions">Options for database initialization</param>
        Task<bool> InitializeAsync(DatabaseInitializeOptions initOptions);
        

        /// <summary>
        /// Provide access to the collection.
        /// </summary>
        IMongoCollection<T> GetCollection<T>() where T : IDbModel;
    }
}