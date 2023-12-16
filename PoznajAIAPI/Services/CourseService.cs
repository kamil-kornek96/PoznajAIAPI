using AutoMapper;
using PoznajAI.Controllers;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using PoznajAI.Models.User;
using Serilog;

namespace PoznajAI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<UserCoursesResponseDto> GetAllCoursesForUser(UserDto user)
        {
            try
            {
                var ownedCourses = await _courseRepository.GetAllCoursesForUser(user.Id);
                var allCourses = await _courseRepository.GetAllCourses();

                var availableCourses = allCourses.Except(ownedCourses).ToList();

                var allCoursesDto = _mapper.Map<List<CourseDto>>(availableCourses);
                if (!user.IsAdmin)
                {
                    allCoursesDto.ForEach(c =>
                    {
                        c.Lessons.ForEach(l =>
                        {
                            l.Duration = null;
                            l.Id = Guid.Empty;
                        });
                    });
                }

                var responseDto = new UserCoursesResponseDto
                {
                    AllCourses = allCoursesDto,
                    OwnedCourses = _mapper.Map<List<OwnedCourseDto>>(ownedCourses)
                };

                return responseDto;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching courses for the user.");
                throw;
            }
        }

        public async Task<Guid> CreateCourse(CourseCreateDto courseDto)
        {
            try
            {
                var course = _mapper.Map<Course>(courseDto);
                var createdCourseId = await _courseRepository.CreateCourse(course);

                Log.Information("Course created: {@Course}", course);

                return createdCourseId;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the course.");
                throw;
            }
        }

        public async Task<CourseDto> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseRepository.GetCourseById(id);

                if (course == null)
                {
                    return null;
                }

                var courseDto = _mapper.Map<CourseDto>(course);
                return courseDto;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching the course by ID.");
                throw;
            }
        }

        public async Task<bool> UpdateCourse(Guid id, CourseUpdateDto courseDto)
        {
            try
            {
                var existingCourse = await _courseRepository.GetCourseById(id);

                if (existingCourse == null)
                {
                    return false;
                }

                courseDto.Id = id;
                await _courseRepository.UpdateCourse(_mapper.Map<Course>(courseDto));

                Log.Information("Course updated: {@Course}", existingCourse);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating the course.");
                throw;
            }
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            try
            {
                var existingCourse = await _courseRepository.GetCourseById(id);

                if (existingCourse == null)
                {
                    return false;
                }

                await _courseRepository.DeleteCourse(id);

                Log.Information("Course deleted: {@Course}", existingCourse);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the course.");
                throw;
            }
        }

    }
}
