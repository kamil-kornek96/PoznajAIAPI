using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Services;
using System;
using System.Linq;
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
                    return Unauthorized(new { message = "Użytkownik niezautoryzowany" });
                }

                var userDto = await _jwtService.ValidateToken(token);
                var courses = await _courseService.GetAllCoursesForUser(userDto.Id);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z pobraniem kursów." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseCreateDto courseDto)
        {
            try
            {
                var courseId = await _courseService.CreateCourse(courseDto);
                return CreatedAtAction(nameof(GetCourseById), new { id = courseId }, new { message = "Dodano kurs." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z dodaniem kursu." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseById(id);

                if (course == null)
                {
                    return NotFound(new { message = "Nie znaleziono kursu." });
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z pobraniem kursu." });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, CourseUpdateDto courseDto)
        {
            try
            {
                var success = await _courseService.UpdateCourse(id, courseDto);

                if (!success)
                {
                    return NotFound(new { message = "Nie znaleziono kursu do aktualizacji." });
                }

                return Ok(new { message = "Zaktualizowano kurs." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z aktualizacją kursu." });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            try
            {
                var success = await _courseService.DeleteCourse(id);

                if (!success)
                {
                    return NotFound(new { message = "Nie znaleziono kursu do usunięcia." });
                }

                return Ok(new { message = "Usunięto kurs." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z usunięciem kursu." });
            }
        }

    }
}
