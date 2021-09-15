using Common.Utils.Interfaces;
using MongoDB.Driver;

namespace Common.Utils
{
    public class MongoDal : IMongoDal
    {
        private readonly MongoDatabaseBase _db;
        

        public MongoDal(MongoDatabaseBase db)
        {
            _db = db;
        }

        public IMongoCollection<TDbModel> GetCollection<TDbModel>() where TDbModel : IDbModel
        {
            return _db.GetCollection<TDbModel>();
        }
    }
}