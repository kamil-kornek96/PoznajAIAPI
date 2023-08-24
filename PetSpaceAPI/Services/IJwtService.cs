using PetSpaceAPI.Models.User;

namespace PetSpaceAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserDto userDto);
    }
}
