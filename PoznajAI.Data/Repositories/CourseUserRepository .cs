using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CourseUserRepository : ICourseUserRepository
{
    private readonly AppDbContext _context;

    public CourseUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CourseUser> GetUserCourseById(int userCourseId)
    {
        return await _context.CourseUsers.FirstOrDefaultAsync(cu => cu.Id == userCourseId);
    }

    public async Task<List<CourseUser>> GetAllUserCourses()
    {
        return await _context.CourseUsers.ToListAsync();
    }

    public async Task<List<CourseUser>> GetUserCoursesByUserId(int userId)
    {
        return await _context.CourseUsers
            .Where(cu => cu.UserId == userId)
            .ToListAsync();
    }

    public async Task<CourseUser> AddUserCourse(CourseUser userCourse)
    {
        _context.CourseUsers.Add(userCourse);
        await _context.SaveChangesAsync();
        return userCourse;
    }

    public async Task<bool> UpdateUserCourse(CourseUser userCourse)
    {
        _context.CourseUsers.Update(userCourse);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task DeleteUserCourse(int userCourseId)
    {
        var userCourse = await _context.CourseUsers.FirstOrDefaultAsync(cu => cu.Id == userCourseId);
        if (userCourse != null)
        {
            _context.CourseUsers.Remove(userCourse);
            await _context.SaveChangesAsync();
        }
    }
}
