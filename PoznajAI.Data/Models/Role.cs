namespace PoznajAI.Data.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public UserRole Name { get; set; }

        public User User { get; set; }

        public Guid UserId { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin,
    }


}
