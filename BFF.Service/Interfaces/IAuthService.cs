using System.Threading;
using BFF.Service.Models;

namespace BFF.Service.Interfaces
{
    public interface IAuthService
    {
        public LoginResponse LoginAsync(LoginRequest request, CancellationToken ct);
    }
}