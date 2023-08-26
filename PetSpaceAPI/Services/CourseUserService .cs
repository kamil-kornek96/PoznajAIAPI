using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using PoznajAI.Models.CourseUser;

public class CourseUserService : ICourseUserService
{
    private readonly ICourseUserRepository _userCourseRepository;
    private readonly IMapper _mapper;

    public CourseUserService(ICourseUserRepository userCourseRepository, IMapper mapper)
    {
        _userCourseRepository = userCourseRepository;
        _mapper = mapper;
    }

    public async Task<CourseUserDto> GetUserCourseById(int userCourseId)
    {
        var userCourse = await _userCourseRepository.GetUserCourseById(userCourseId);
        return _mapper.Map<CourseUserDto>(userCourse);
    }

    public async Task<List<CourseUserDto>> GetAllUserCourses()
    {
        var userCourses = await _userCourseRepository.GetAllUserCourses();
        return _mapper.Map<List<CourseUserDto>>(userCourses);
    }

    public async Task<List<CourseUserDto>> GetUserCoursesByUserId(int userId)
    {
        var userCourses = await _userCourseRepository.GetUserCoursesByUserId(userId);
        return _mapper.Map<List<CourseUserDto>>(userCourses);
    }

    public async Task<CourseUserDto> AddUserCourse(CourseUserCreateDto userCourse)
    {
        var userCourseToAdd = _mapper.Map<CourseUser>(userCourse);
        return _mapper.Map<CourseUserDto>(await _userCourseRepository.AddUserCourse(userCourseToAdd));
    }

    public async Task UpdateUserCourse(CourseUserUpdateDto userCourse)
    {
        var userCourseToUpdate = _mapper.Map<CourseUser>(userCourse);
        await _userCourseRepository.UpdateUserCourse(userCourseToUpdate);
    }

    public async Task DeleteUserCourse(int userCourseId)
    {
        await _userCourseRepository.DeleteUserCourse(userCourseId);
    }
}
