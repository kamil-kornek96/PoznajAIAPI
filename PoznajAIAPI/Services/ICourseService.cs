namespace PoznajAI.Services
{
    public interface ICourseService
    {
        Task CreateCourse(CourseCreateDto CourseDto);
        Task<UserCoursesResponseDto> GetAllCoursesForUser(Guid userId);
    }
}