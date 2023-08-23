using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories
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
