﻿namespace PoznajAI.Models.User
{
    public class UserCreateDto
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
