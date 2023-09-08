using PoznajAI.Controllers;

namespace PoznajAI.Services
{
    public interface ICourseService
    {
        Task CreateCourse(CourseCreateDto CourseDto);
        Task<UserCoursesResponseDto> GetAllCoursesForUser(Guid userId);
        Task<CourseDto> GetCourseById(Guid id);
        Task<bool> UpdateCourse(Guid id, CourseUpdateDto courseDto);
    }
}