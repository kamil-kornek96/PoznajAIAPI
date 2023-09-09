using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface ICourseRepository
    {
        Task<Guid> CreateCourse(Course course);
        Task<bool> DeleteCourse(Guid courseId);
        Task<IEnumerable<Course>> GetAllCourses();
        Task<IEnumerable<Course>> GetAllCoursesForUser(Guid userId);
        Task<Course> GetCourseById(Guid courseId);
        Task UpdateCourse(Course updatedCourse);
    }
}