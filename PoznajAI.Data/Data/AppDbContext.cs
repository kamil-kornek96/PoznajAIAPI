using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Models;

namespace PoznajAI.Data.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CompletedLesson> CompletedLessons { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonAssignment> LessonAssignments { get; set; }
        public DbSet<LessonComment> LessonComments { get; set; }
        public DbSet<LessonRating> LessonRatings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompletedLesson>()
                .HasKey(cl => new { cl.CourseUserId, cl.LessonId });

            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.CourseUser)
                .WithMany(cu => cu.CompletedLessons)
                .HasForeignKey(cl => cl.CourseUserId);

            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.Lesson)
                .WithMany()
                .HasForeignKey(cl => cl.LessonId);

            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.CourseUser)
                .WithMany(cu => cu.CompletedLessons)
                .HasForeignKey(cl => cl.CourseUserId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict instead of Cascade

        }
    }

}



