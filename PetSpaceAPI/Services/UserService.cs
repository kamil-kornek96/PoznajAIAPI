using PetSpaceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetSpaceAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        bool IsUsernameTaken(string username);
        void CreateUser(User user);
    }

    public class UserService : IUserService
    {
        private List<User> _users = new List<User>();

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public bool IsUsernameTaken(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        public void CreateUser(User user)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.Id = _users.Max(u => u.Id) + 1;
            _users.Add(user);
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
