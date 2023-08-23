﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PetSpaceAPI.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Repositories;
using PetSpace.Data.Models;

namespace PetSpaceAPI.Services
{


    public interface IUserService
    {
        Task<UserDto> Authenticate(string username, string password);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<bool> IsUsernameTaken(string username);
        Task CreateUser(UserDto user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Authenticate(string username, string password)
        {
            var user = await GetUserById(username);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserById(string userName)
        {
            var user = await _userRepository.GetUserByUsername(userName);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return await _userRepository.UsernameExists(username);
        }

        public async Task CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash.ToString();
            user.PasswordSalt = passwordSalt.ToString();

            await _userRepository.CreateUser(user);
            _logger.LogInformation("User created: {@user}", user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

}