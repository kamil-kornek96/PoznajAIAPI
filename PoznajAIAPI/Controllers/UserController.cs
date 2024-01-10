using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Data.Models;
using PoznajAI.Models.Auth;
using PoznajAI.Models.Course;
using PoznajAI.Models.User;
using PoznajAI.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService, ICourseService courseService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _courseService = courseService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequestDto model)
        {
            var userDto = await _userService.Authenticate(model.Email, model.Password);

            if (userDto == null)
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }

            var token = _jwtService.GenerateToken(userDto);

            return Ok(new { message = "Successfully logged in!", token });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterRequestDto model)
        {

            if (await _userService.IsEmailTaken(model.Email))
            {
                return BadRequest(new { message = "Username is taken." });
            }

            var userDto = new UserCreateDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            };

            try
            {
                var userId = await _userService.CreateUser(userDto);

                if (userId == Guid.Empty)
                {
                    return BadRequest(new { message = "Error occurred while registering user." });
                }

                var addedUserDto = await _userService.Authenticate(model.Email, model.Password);

                if (addedUserDto == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect." });
                }

                var token = _jwtService.GenerateToken(addedUserDto);

                return Ok(new
                {
                    message = "",
                    token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while registering user." });
            }
        }

        [Authorize]
        [HttpGet("courses")]
        public async Task<ActionResult<UserCoursesResponseDto>> GetAllCoursesForUser()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Unauthorized user" });
                }

                var userDto = await _jwtService.ValidateToken(token);
                var courses = await _courseService.GetAllCoursesForUser(userDto);

                return Ok(courses);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching user courses");
                return StatusCode(500, new { message = "Error retrieving user courses" });
            }
        }

        [Authorize]
        [HttpGet("auth")]
        public async Task<ActionResult> CheckAuthentication()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token not found" });
            }

            var userDto = await _jwtService.ValidateToken(token);

            if (userDto == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            return Ok(new
            {
                message = "User authenticated",
                user = userDto
            });
        }

        [Authorize]
        [HttpPost("courses/{courseId}")]
        public async Task<ActionResult> AddCourseToUser(Guid courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token not found" });
            }

            var userDto = await _jwtService.ValidateToken(token);

            var isAdded = await _userService.AddCourseToUser(userDto.Id, courseId);

            if (isAdded)
            {
                return Ok(new { message = "Course added to user successfully" });
            }
            else
            {
                return BadRequest(new { message = "Course could not be added to user" });
            }
        }

        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AddUserRole(Guid userId, [FromBody] UserRole role)
        {
            var user = await _userService.AddUserRoleAsync(userId, role);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "", user });
        }
    }
}
