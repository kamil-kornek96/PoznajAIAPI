using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using PoznajAI.Models.User;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace PoznajAI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);

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

        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await _userRepository.EmailExists(email);
        }

        public async Task<Guid> CreateUser(UserCreateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            try
            {
                var userId = await _userRepository.CreateUser(user, UserRole.User);
                Log.Information("User created: {@user}", user);
                return userId;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the user.");
                throw;
            }
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

        public async Task<bool> AddCourseToUser(Guid userId, Guid courseId)
        {
            try
            {
                Log.Information($"Add course: {courseId.ToString()} to user: {userId.ToString()}");
                return await _userRepository.AddCourseToUser(userId, courseId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a course to the user.");
                throw;
            }
        }

        public async Task<UserDto> AddUserRoleAsync(Guid userId, UserRole role)
        {
            try
            {
                var user = await _userRepository.AddUserRoleAsync(userId, role);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a role to the user.");
                throw;
            }
        }
    }
}
