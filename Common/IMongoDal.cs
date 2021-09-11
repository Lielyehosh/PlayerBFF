using System.Threading;
using System.Threading.Tasks;
using Common.DbModels;

namespace Common
{
    public interface IMongoDal
    {
        public Task<User> CreateUserAsync(User user, CancellationToken ct);
        public Task<User> FindUserByIdAsync(string id, CancellationToken ct);
    }
}