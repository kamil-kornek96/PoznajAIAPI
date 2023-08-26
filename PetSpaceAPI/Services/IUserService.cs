using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(string username, string password);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<bool> IsUsernameTaken(string username);
        Task CreateUser(UserDto user);
    }
}
