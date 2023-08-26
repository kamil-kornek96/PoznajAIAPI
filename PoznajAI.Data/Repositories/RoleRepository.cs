using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;

namespace PoznajAI.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _dbContext;

        public RoleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _dbContext.Roles.FindAsync(roleId);
        }

        public async Task<Role> GetRoleByName(UserRole roleName)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task CreateRole(Role role)
        {
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRole(int roleId)
        {
            var role = await _dbContext.Roles.FindAsync(roleId);
            if (role != null)
            {
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
