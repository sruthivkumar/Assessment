using Microsoft.EntityFrameworkCore;
using ProductOrderAPI.Data;
using ProductOrderAPI.DTOs.Auth;
using ProductOrderAPI.Model;
using ProductOrderAPI.Service.Interface;

namespace ProductOrderAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwt;
        public AuthService(AppDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email ==
            request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password,
            user.PasswordHash))
                throw new ApplicationException("Invalid credentials");
            var token = _jwt.GenerateToken(user);
        
        return new AuthResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
        }
        public async Task RegisterAsync(RegisterRequest request, string
        currentUserRole)
        {
            if (currentUserRole != "Admin")
                throw new UnauthorizedAccessException("Only admin can register new  users");
               
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                throw new ApplicationException("Email already in use");
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = string.IsNullOrWhiteSpace(request.Role) ? "User" :
            request.Role
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
    }
}
