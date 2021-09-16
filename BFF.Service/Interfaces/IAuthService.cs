using System.Threading;
using BFF.Service.Controllers;
using BFF.Service.Models;

namespace BFF.Service.Interfaces
{
    public interface IAuthService
    {
        public AuthResponse Login(LoginRequest request, CancellationToken ct);
        public AuthResponse Register(RegisterRequest registerReq, CancellationToken ct);
    }
}