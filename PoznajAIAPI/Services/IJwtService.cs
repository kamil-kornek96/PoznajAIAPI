using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IJwtService
    {
        TokenDto GenerateToken(UserDto userDto);
        Task<UserDto> ValidateToken(string token);
    }
}
