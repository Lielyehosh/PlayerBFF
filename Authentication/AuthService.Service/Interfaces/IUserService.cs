using System.Threading;
using System.Threading.Tasks;
using Common.Models.DbModels;

namespace AuthMS.Services
{
    public interface IUserService
    {
        public Task<RegisterUserResponse> RegisterNewUserAsync(User user, CancellationToken ct);
        public Task<User> FindUserByIdNumberAsync(string idNumber, CancellationToken ct);
        bool TestM();
    }
}