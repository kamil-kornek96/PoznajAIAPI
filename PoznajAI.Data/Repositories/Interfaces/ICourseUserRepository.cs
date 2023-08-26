using PoznajAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface ICourseUserRepository
    {
        Task<CourseUser> GetUserCourseById(int userCourseId);
        Task<List<CourseUser>> GetAllUserCourses();
        Task<List<CourseUser>> GetUserCoursesByUserId(int userId);
        Task<CourseUser> AddUserCourse(CourseUser userCourse);
        Task<bool> UpdateUserCourse(CourseUser userCourse);
        Task DeleteUserCourse(int userCourseId);
    }
}
