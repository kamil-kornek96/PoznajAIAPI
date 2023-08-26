using PoznajAI.Models.CourseUser;

public interface ICourseUserService
{
    Task<CourseUserDto> AddUserCourse(CourseUserCreateDto userCourse);
    Task DeleteUserCourse(int userCourseId);
    Task<List<CourseUserDto>> GetAllUserCourses();
    Task<CourseUserDto> GetUserCourseById(int userCourseId);
    Task<List<CourseUserDto>> GetUserCoursesByUserId(int userId);
    Task UpdateUserCourse(CourseUserUpdateDto userCourse);
}