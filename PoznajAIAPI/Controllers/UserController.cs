using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<UserDto>> Login(LoginRequestDto model)
        {
            var userDto = await _userService.Authenticate(model.Username, model.Password);

            if (userDto == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var token = _jwtService.GenerateToken(userDto);

            return Ok(token);
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
        public async Task<ActionResult> Register(RegisterRequestDto model)
        {
            if (await _userService.IsUsernameTaken(model.Username))
            {
                return BadRequest(new { message = "Username is already taken" });
            }

            var userDto = new UserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };

            await _userService.CreateUser(userDto);

            var addeUserDto = await _userService.Authenticate(model.Username, model.Password);

            if (addeUserDto == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var token = _jwtService.GenerateToken(addeUserDto);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("check-auth")]
        public async Task<ActionResult> CheckAuthentication()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token is missing" });
            }

            var userDto = _jwtService.ValidateToken(token);

            if (userDto == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            return Ok(new
            {
                message = "User is authenticated",
                user = userDto
            });
        }

        [Authorize]
        [HttpPost("add-course")]
        public async Task<ActionResult> AddCourseToUser([FromBody] CourseAssignmentDto assignment)
        {
            // Check if the assignment model is valid
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

    }
}
