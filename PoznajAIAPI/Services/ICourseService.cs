using PoznajAI.Controllers;
using PoznajAI.Models.User;

namespace PoznajAI.Services
{
    public interface ICourseService
    {
        Task<Guid> CreateCourse(CourseCreateDto courseDto);
        Task<UserCoursesResponseDto> GetAllCoursesForUser(UserDto userId);
        Task<CourseDto> GetCourseById(Guid id);
        Task<bool> UpdateCourse(Guid id, CourseUpdateDto courseDto);
        Task<bool> DeleteCourse(Guid id);
    }
}