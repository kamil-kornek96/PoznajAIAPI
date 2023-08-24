using Microsoft.AspNetCore.Mvc;
using PetSpaceAPI.Models.Auth;
using PetSpaceAPI.Models.User;
using PetSpaceAPI.Services;

namespace PetSpaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public UserController(IUserService userService, JwtService jwtService)
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
        public async Task<ActionResult<UserDto>> GetUserById(int id)
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
            return Ok();
        }
    }
}
