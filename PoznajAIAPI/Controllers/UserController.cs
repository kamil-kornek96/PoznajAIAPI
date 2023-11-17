using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Data.Models;
using PoznajAI.Models.Auth;
using PoznajAI.Models.User;
using PoznajAI.Services;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequestDto model)
        {
            var userDto = await _userService.Authenticate(model.Username, model.Password);

            if (userDto == null)
            {
                return BadRequest(new { message = "Nazwa użytkownika lub hasło są nieprawidłowe." });
            }

            var token = _jwtService.GenerateToken(userDto);

            return Ok(new { message = "Pomyślnie zalogowano!", token });
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
            if (await _userService.IsUsernameTaken(model.Username))
            {
                return BadRequest(new { message = "Nazwa użytkownika jest zajęta." });
            }

            var userDto = new UserCreateDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };

            try
            {
                var userId = await _userService.CreateUser(userDto);

                if (userId == Guid.Empty)
                {
                    return BadRequest(new { message = "Podczas rejestracji użytkownika, wystąpił błąd." });
                }

                var addedUserDto = await _userService.Authenticate(model.Username, model.Password);

                if (addedUserDto == null)
                {
                    return BadRequest(new { message = "Nazwa użytkownika lub hasło są nieprawidłowe." });
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
                return StatusCode(500, new { message = "Podczas rejestracji użytkownika, wystąpił błąd." });
            }
        }

        [Authorize]
        [HttpGet("check-auth")]
        public async Task<ActionResult> CheckAuthentication()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Brak tokena" });
            }

            var userDto = await _jwtService.ValidateToken(token);

            if (userDto == null)
            {
                return Unauthorized(new { message = "Niepoprawny token" });
            }

            return Ok(new
            {
                message = "Użytkownik zautoryzowany",
                user = userDto
            });
        }

        [Authorize]
        [HttpPost("add-course")]
        public async Task<ActionResult> AddCourseToUser([FromBody] CourseAssignmentDto assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isAdded = await _userService.AddCourseToUser(new Guid(assignment.UserId), new Guid(assignment.CourseId));

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
