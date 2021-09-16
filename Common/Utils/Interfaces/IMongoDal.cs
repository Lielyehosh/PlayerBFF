using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Common.Utils.Interfaces
{
    public interface IMongoDal
    {
        public IMongoCollection<TDbModel> GetCollection<TDbModel>() where TDbModel : IDbModel;

        public Task<TDbModel> InsertOneAsync<TDbModel>(TDbModel document, CancellationToken ct) where TDbModel : IDbModel;
    }
}