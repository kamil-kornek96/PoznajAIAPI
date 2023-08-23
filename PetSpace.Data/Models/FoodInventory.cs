using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Models
{
    public class FoodInventory
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public double QuantityInGrams { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
