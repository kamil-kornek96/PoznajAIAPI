using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface ICourseRepository
    {
        Task CreateCourse(Course Course);
        Task DeleteCourse(Guid courseId);
        Task<IEnumerable<Course>> GetAllCoursesForUser(Guid userId);
        Task<IEnumerable<Course>> GetAllCourses();
        Task UpdateCourse(Course Course);
        Task<Course> GetCourseById(Guid id);
    }
}