namespace PoznajAI.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public UserRole Name { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin,
    }


}
