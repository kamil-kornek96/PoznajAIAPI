using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IJwtService _jwtService;

        public CourseController(ICourseService courseService, IJwtService jwtService)
        {
            _courseService = courseService;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpGet("my-courses")]
        public async Task<ActionResult<UserCoursesResponseDto>> GetAllCoursesForUser()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing" });
                }

                var userDto = await _jwtService.ValidateToken(token);
                var courses = await _courseService.GetAllCoursesForUser(userDto.Id);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error).
                return StatusCode(500, new { message = "An error occurred while fetching courses." });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateCourse(CourseCreateDto courseDto)
        {
            try
            {
                await _courseService.CreateCourse(courseDto);
                return Ok(new { message = "Course created successfully." });
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error).
                return StatusCode(500, new { message = "An error occurred while creating the course." });
            }
        }

        // Get a course by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseById(id);

                if (course == null)
                {
                    return NotFound(new { message = "Course not found." });
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error).
                return StatusCode(500, new { message = "An error occurred while fetching the course." });
            }
        }

        // Update an existing course
        [Authorize] // Add authorization if required
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, CourseUpdateDto courseDto)
        {
            try
            {
                var existingCourse = await _courseService.GetCourseById(id);

                if (existingCourse == null)
                {
                    return NotFound(new { message = "Course not found." });
                }

                await _courseService.UpdateCourse(id, courseDto);
                return Ok(new { message = "Course updated successfully." });
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error).
                return StatusCode(500, new { message = "An error occurred while updating the course." });
            }
        }
    }
}
