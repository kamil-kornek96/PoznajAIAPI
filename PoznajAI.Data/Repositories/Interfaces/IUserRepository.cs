using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int userId);
        Task<bool> UsernameExists(string username);
    }
}
