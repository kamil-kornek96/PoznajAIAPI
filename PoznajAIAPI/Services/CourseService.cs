using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Controllers;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PoznajAI.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _CourseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository CourseRepository, IMapper mapper, ILogger<CourseService> logger)
        {
            _CourseRepository = CourseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserCoursesResponseDto> GetAllCoursesForUser(Guid userId)
        {
            try
            {
                var ownedCourses = await _CourseRepository.GetAllCoursesForUser(userId);
                var allCourses = await _CourseRepository.GetAllCourses();


                var availableCourses = allCourses.Where(course => !ownedCourses.Any(ownedCourse => ownedCourse.Id == course.Id)).ToList();

                var responseDto = new UserCoursesResponseDto
                {
                    AllCourses = _mapper.Map<List<CourseDto>>(availableCourses),
                    OwnedCourses = _mapper.Map<List<OwnedCourseDto>>(ownedCourses)
                };

                return responseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas pobierania kursów dla użytkownika.");
                throw;
            }
        }




        public async Task CreateCourse(CourseCreateDto CourseDto)
        {
            var Course = _mapper.Map<Course>(CourseDto);
            await _CourseRepository.CreateCourse(Course);
            _logger.LogInformation("Course created: {@Course}", Course);
        }

        public async Task<CourseDto> GetCourseById(Guid id)
        {
            try
            {
                var course = await _CourseRepository.GetCourseById(id);

                if (course == null)
                {
                    return null;
                }

                var courseDto = _mapper.Map<CourseDto>(course);
                return courseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the course by ID.");
                return null;
            }
        }

        public async Task<bool> UpdateCourse(Guid id, CourseUpdateDto courseDto)
        {
            try
            {
                var existingCourse = await _CourseRepository.GetCourseById(id);

                if (existingCourse == null)
                {
                    return false;
                }

                await _CourseRepository.UpdateCourse(_mapper.Map<Course>(courseDto));

                _logger.LogInformation("Course updated: {@Course}", existingCourse);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the course.");
                return false;
            }
        }
    }

}
