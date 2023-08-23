using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Models;

namespace PetSpace.Data.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Feeding> Feedings { get; set; }
        public DbSet<FoodConsumption> FoodConsumptions { get; set; }
        public DbSet<FoodInventory> FoodInventories { get; set; }
        public DbSet<VetVisit> VetVisits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}



