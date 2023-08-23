using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Models
{
    public class Feeding
    {
        public int Id { get; set; }
        public DateTime FeedingTime { get; set; }
        public double AmountInGrams { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
