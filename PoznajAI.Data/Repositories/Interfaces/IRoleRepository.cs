﻿using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleById(int roleId);
        Task<Role> GetRoleByName(UserRole roleName);
        Task CreateRole(Role role);
        Task DeleteRole(int roleId);
    }
}