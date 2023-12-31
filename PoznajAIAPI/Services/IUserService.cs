﻿using PoznajAI.Data.Models;
using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface IUserService
    {
        Task<bool> AddCourseToUser(Guid userId, Guid courseId);
        Task<UserDto> AddUserRoleAsync(Guid userId, UserRole role);
        Task<UserDto> Authenticate(string username, string password);
        Task<Guid> CreateUser(UserCreateDto userDto);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task<User> GetUserByName(string username);
        Task<bool> IsUsernameTaken(string username);
    }
}