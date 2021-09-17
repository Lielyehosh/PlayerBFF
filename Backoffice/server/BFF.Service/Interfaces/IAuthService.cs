using System.Threading;
using System.Threading.Tasks;
using BFF.Service.Controllers;
using BFF.Service.Models;

namespace BFF.Service.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);
        public Task<AuthResponse> RegisterAsync(RegisterRequest registerReq, CancellationToken ct);
        
        
        // TODO - consider to create another service for user detail updates
        /// <summary>
        /// Reset password request 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<ResetPasswordResponse> ResetPasswordAsync(string userId, string pw, CancellationToken ct);
    }
}