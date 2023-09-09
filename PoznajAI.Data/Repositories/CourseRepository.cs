using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;

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
            var user = _dbContext.Users.Find(userId);
            return _dbContext.Courses.Include(c => c.Lessons).Where(c => c.Users.Contains(user));
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _dbContext.Courses.Include(c => c.Lessons).ToListAsync();
        }

        public async Task CreateCourse(Course course)
        {
            try
            {
                await _dbContext.Courses.AddAsync(course);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
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

        public async Task DeleteCourse(Guid courseId)
        {
            var Course = await _dbContext.Courses.FindAsync(courseId);
            if (Course != null)
            {
                _dbContext.Courses.Remove(Course);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Course> GetCourseById(Guid courseId)
        {
            return _dbContext.Courses.FirstOrDefault(c => c.Id == courseId);
        }
    }
}
