using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleById(int roleId);
        Task<Role> GetRoleByName(UserRole roleName);
        Task CreateRole(Role role);
        Task DeleteRole(int roleId);
    }
}
