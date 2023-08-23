using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Models
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
