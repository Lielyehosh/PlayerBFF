using System.Threading;
using System.Threading.Tasks;
using Common.Models.DbModels;

namespace AuthMS.Services
{
    public interface IUserService
    {
        public Task<RegisterUserResponse> RegisterNewUserAsync(User user, CancellationToken ct = default);
        public Task<User> FindUserByIdNumberAsync(string idNumber, CancellationToken ct = default);
        public Task<User> FindUserByEmailAsync(string email, CancellationToken ct = default);
        public Task<User> FindUserByUsernameAsync(string username, CancellationToken ct = default);
    }
}