using System.Threading;
using PlayerBFF.Controllers;
using PlayerBFF.Models;

namespace PlayerBFF.Interfaces
{
    public interface IAuthService
    {
        public LoginResponse LoginAsync(LoginRequest request, CancellationToken ct);
    }
}