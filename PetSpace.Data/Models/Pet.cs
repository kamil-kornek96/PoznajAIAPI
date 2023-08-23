using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public DateTime BirthDate { get; set; }
        public string OwnerName { get; set; }

        public ICollection<Feeding> Feedings { get; set; }
        public ICollection<VetVisit> VetVisits { get; set; }


        public ICollection<User> User { get; set; }
    }
}
