using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(string username, string password);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task<UserDto> GetUserByName(string username);
        Task<bool> IsUsernameTaken(string username);
        Task CreateUser(UserDto user);
        Task<bool> AddCourseToUser(Guid userId, Guid courseId);
    }
}
