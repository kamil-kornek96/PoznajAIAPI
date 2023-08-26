using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserDto userDto);
    }
}
