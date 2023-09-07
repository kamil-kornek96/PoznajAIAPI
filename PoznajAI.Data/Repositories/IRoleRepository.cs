using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface IRoleRepository
    {
        Task CreateRole(Role role);
        Task DeleteRole(Guid roleId);
        Task<Role> GetRoleById(Guid roleId);
        Task<Role> GetRoleByName(UserRole roleName);
    }
}