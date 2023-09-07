using PoznajAI.Data.Models;

namespace PoznajAI.Models.User
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public UserRole Name { get; set; }

        public Guid UserId { get; set; }
    }
}
