using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Models;

namespace PoznajAI.Data.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }

}



