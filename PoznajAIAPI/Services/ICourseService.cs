using PoznajAI.Controllers;

namespace PoznajAI.Services
{
    public interface ICourseService
    {
        Task<Guid> CreateCourse(CourseCreateDto courseDto);
        Task<UserCoursesResponseDto> GetAllCoursesForUser(Guid userId);
        Task<CourseDto> GetCourseById(Guid id);
        Task<bool> UpdateCourse(Guid id, CourseUpdateDto courseDto);
        Task<bool> DeleteCourse(Guid id);
    }
}