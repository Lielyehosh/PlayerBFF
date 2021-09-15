using MongoDB.Driver;

namespace Common.Utils.Interfaces
{
    public interface IMongoDal
    {
        public IMongoCollection<TDbModel> GetCollection<TDbModel>() where TDbModel : IDbModel;
    }
}