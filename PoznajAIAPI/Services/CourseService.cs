using AutoMapper;
using Microsoft.Extensions.Logging;
using PoznajAI.Controllers;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoznajAI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository, IMapper mapper, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserCoursesResponseDto> GetAllCoursesForUser(Guid userId)
        {
            try
            {
                var ownedCourses = await _courseRepository.GetAllCoursesForUser(userId);
                var allCourses = await _courseRepository.GetAllCourses();

                var availableCourses = allCourses.Except(ownedCourses).ToList();

                var responseDto = new UserCoursesResponseDto
                {
                    AllCourses = _mapper.Map<List<CourseDto>>(availableCourses),
                    OwnedCourses = _mapper.Map<List<OwnedCourseDto>>(ownedCourses)
                };

                return responseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching courses for the user.");
                throw;
            }
        }

        public async Task<Guid> CreateCourse(CourseCreateDto courseDto)
        {
            try
            {
                var course = _mapper.Map<Course>(courseDto);
                var createdCourseId = await _courseRepository.CreateCourse(course);

                _logger.LogInformation("Course created: {@Course}", course);

                return createdCourseId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the course.");
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
                _logger.LogError(ex, "An error occurred while fetching the course by ID.");
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

                _logger.LogInformation("Course updated: {@Course}", existingCourse);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the course.");
                throw;
            }
        }
    }
}
