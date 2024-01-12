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
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Guid> CreateUser(User user, UserRole role)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var newRole = new Role { Name = role, UserId = user.Id };
                _dbContext.Roles.Add(newRole);
                await _dbContext.SaveChangesAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public async Task UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> AddCourseToUser(Guid userId, Guid courseId)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.Courses)
                    .FirstOrDefaultAsync(u => u.Id == userId);

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
                throw new Exception("An error occurred while adding a course to the user.", ex);
            }
        }

        public async Task<User> AddUserRoleAsync(Guid userId, UserRole role)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            if (user.Roles.Any(r => r.Name == role))
            {
                return null;
            }

            var roleDto = new Role
            {
                Name = role,
                UserId = userId
            };

            user.Roles.Add(roleDto);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
