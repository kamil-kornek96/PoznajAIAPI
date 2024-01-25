using PoznajAI.Models.Auth;
using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IJwtService
    {
        TokenResponseDto GenerateToken(UserDto userDto);
        Task<UserDto> ValidateToken(string token);
        UserDto FastValidateToken(string token);
    }
}
