using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task CreateUser(User user, UserRole role)
        {
            try
            {
                // Utwórz użytkownika bez przypisanej roli
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                // Utwórz nową rolę i przypisz ją do użytkownika
                var newRole = new Role { Name = role, UserId = user.Id };
                _dbContext.Roles.Add(newRole);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }
        }

        public async Task UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> AddCourseToUser(Guid userId, Guid courseId)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.Courses) // Include the Courses collection
                    .FirstOrDefaultAsync(u => u.Id == userId); // Assuming 'Id' is the user's primary key

                var course = await _dbContext.Courses.FindAsync(courseId);

                if (user != null && course != null && !user.Courses.Contains(course))
                {
                    user.Courses.Add(course);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return false;
            }
        }


    }
}
