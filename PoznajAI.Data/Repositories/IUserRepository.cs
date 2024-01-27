using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddCourseToUser(Guid userId, Guid courseId);
        Task<User> AddUserRoleAsync(Guid userId, UserRole role);
        Task<Guid> CreateUser(User user, UserRole role);
        Task<bool> DeleteUser(Guid userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByEmail(string username);
        Task UpdateUser(User user);
        Task<bool> EmailExists(string username);
        Task<User> GetUserByActivationToken(string token);
    }
}