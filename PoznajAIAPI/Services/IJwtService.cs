using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserDto userDto);
        Task<UserDto> ValidateToken(string token);
        UserDto FastValidateToken(string token);
    }
}
