using System.Threading;
using System.Threading.Tasks;
using Common.DbModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Common
{
    public class MongoDal : IMongoDal
    {
        private readonly IMongoDatabase _db;

        public MongoDal(IMongoDatabase db)
        {
            _db = db;
        }
        
        public async Task<User> CreateUserAsync(User user, CancellationToken ct)
        {
            var userColl = _db.GetCollection<User>("user");
            await userColl.InsertOneAsync(user, ct);
            return user;
        }

        public async Task<User> FindUserByIdAsync(string id, CancellationToken ct)
        {
            var userColl = _db.GetCollection<User>("user");
            // TODO - continue from here, consider to add tests
            return null;
        }
    }
}