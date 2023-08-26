using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoznajAI.Data.Models;
using PoznajAI.Services;
using PoznajAI.Models.CourseUser;

namespace PoznajAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseUsersController : ControllerBase
    {
        private readonly ICourseUserService _userCourseService;

        public CourseUsersController(ICourseUserService userCourseService)
        {
            _userCourseService = userCourseService;
        }

        [HttpGet("{userCourseId}")]
        public async Task<ActionResult<CourseUserDto>> GetUserCourseById(int userCourseId)
        {
            var userCourse = await _userCourseService.GetUserCourseById(userCourseId);

            if (userCourse == null)
            {
                return NotFound();
            }

            return Ok(userCourse);
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseUserDto>>> GetAllUserCourses()
        {
            var userCourses = await _userCourseService.GetAllUserCourses();
            return Ok(userCourses);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<CourseUserDto>>> GetUserCoursesByUserId(int userId)
        {
            var userCourses = await _userCourseService.GetUserCoursesByUserId(userId);
            return Ok(userCourses);
        }

        [HttpPost]
        public async Task<ActionResult> AddUserCourse([FromBody] CourseUserCreateDto userCourse)
        {
            if (userCourse == null)
            {
                return BadRequest();
            }

            var userCourseDto = await _userCourseService.AddUserCourse(userCourse);
            return CreatedAtAction(nameof(GetUserCourseById), new { userCourseId = userCourseDto.Id }, userCourseDto);
        }

        [HttpPut("{userCourseId}")]
        public async Task<ActionResult> UpdateUserCourse(int userCourseId, [FromBody] CourseUserUpdateDto userCourse)
        {
            if (userCourse == null || userCourse.Id != userCourseId)
            {
                return BadRequest();
            }

            await _userCourseService.UpdateUserCourse(userCourse);
            return NoContent();
        }

        [HttpDelete("{userCourseId}")]
        public async Task<ActionResult> DeleteUserCourse(int userCourseId)
        {
            await _userCourseService.DeleteUserCourse(userCourseId);
            return NoContent();
        }
    }
}
