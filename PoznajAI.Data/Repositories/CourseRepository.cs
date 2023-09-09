using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoznajAI.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _dbContext;

        public CourseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesForUser(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return _dbContext.Courses.Include(c => c.Lessons).Where(c => c.Users.Contains(user)).ToList();
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _dbContext.Courses.Include(c => c.Lessons).ToListAsync();
        }

        public async Task<Guid> CreateCourse(Course course)
        {
            try
            {
                await _dbContext.Courses.AddAsync(course);
                await _dbContext.SaveChangesAsync();
                return course.Id;
            }
            catch (Exception ex)
            {
                // Obsługa błędów, np. zapis do logów
                throw new Exception("An error occurred while creating the course.", ex);
            }
        }

        public async Task UpdateCourse(Course updatedCourse)
        {
            var existingCourse = await _dbContext.Courses.FindAsync(updatedCourse.Id);

            if (existingCourse == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            _dbContext.Entry(existingCourse).CurrentValues.SetValues(updatedCourse);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCourse(Guid courseId)
        {
            var course = await _dbContext.Courses.FindAsync(courseId);
            if (course != null)
            {
                _dbContext.Courses.Remove(course);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Course> GetCourseById(Guid courseId)
        {
            return await _dbContext.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Id == courseId);
        }
    }
}
