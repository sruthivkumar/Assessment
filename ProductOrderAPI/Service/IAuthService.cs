using ProductOrderAPI.DTOs.Auth;

namespace ProductOrderAPI.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request, string currentUserRole);
    }
}
