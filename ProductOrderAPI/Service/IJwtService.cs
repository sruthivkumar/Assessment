using ProductOrderAPI.Model;

namespace ProductOrderAPI.Service.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
