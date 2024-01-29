using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PoznajAI.Data.Models;
using PoznajAI.Models;
using PoznajAI.Models.Auth;
using PoznajAI.Models.Course;
using PoznajAI.Models.User;
using PoznajAI.Services;
using Serilog;


namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("DefaultPolicy")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IJwtService jwtService, ICourseService courseService, IEmailService emailService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _courseService = courseService;
            _emailService = emailService;
        }

        /// <summary>
        /// Logs a user into the system and return JWT token after successful login.
        /// </summary>
        /// <param name="model">Login information.</param>
        /// <returns>A newly generated JWT token.</returns>
        /// <response code="200">Returns the newly generated JWT token.</response>
        /// <response code="400">If the login information is incorrect.</response>
        /// <response code="500">If there was an error during login.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(DefaultResponse<TokenResponseDto>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 400)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<string>> Login(LoginRequestDto model)
        {
            try
            {
                var userDto = await _userService.Authenticate(model.Email, model.Password);

                if (userDto == null)
                {
                    Log.Warning("Login failed for user with email: {Email}", model.Email);
                    return BadRequest(new DefaultResponse<object>(400, "Username or password is incorrect.", false));
                }

                if (!userDto.IsEmailConfirmed)
                {
                    Log.Warning("Login failed, because of not activated account, for user with email: {Email}", model.Email);
                    return BadRequest(new DefaultResponse<object>(400, "Account not activated", false));
                }

                var token = _jwtService.GenerateToken(userDto);

                Log.Information("User logged in: {Email}", model.Email);
                return Ok(new DefaultResponse<TokenResponseDto>(200, "Successfully logged in!", true, token));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during login for user with email: {Email}", model.Email);
                return StatusCode(500, new DefaultResponse<object>(500, "Error occurred during login.", false));
            }
        }

        /// <summary>
        /// Registers a new user in the system and return JWT token after successful registration.
        /// </summary>
        /// <param name="model">User information for registration.</param>
        /// <returns>A newly generated JWT token.</returns>
        /// <response code="200">Returns the newly generated JWT token.</response>
        /// <response code="400">If the user information is invalid or the email is already taken.</response>
        /// <response code="500">If there was an error during registration.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(DefaultResponse<string>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 400)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<string>> Register(RegisterRequestDto model)
        {
            try
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

                var userId = await _userService.CreateUser(userDto);

                if (userId == Guid.Empty)
                {
                    return BadRequest(new DefaultResponse<object>(400, "Error occurred while registering user.", false));
                }

                var addedUserDto = await _userService.Authenticate(model.Email, model.Password);

                if (addedUserDto == null)
                {
                    return BadRequest(new DefaultResponse<object>(400, "Username or password is incorrect.", false));
                }

                var requestUrlWithoutPath = new Uri(HttpContext.Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority);


                await _emailService.SendEmailActivationMessage(userId, requestUrlWithoutPath);

                Log.Information("User registered: {Email}", model.Email);
                return Ok(new DefaultResponse<object>(200, "Successfully registered!", true, null));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during user registration for email: {Email}", model.Email);
                return StatusCode(500, new DefaultResponse<object>(500, "Error occurred during user registration.", false));
            }
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>Returns a list of all users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="500">If there was an issue while retrieving users.</response>
        [HttpGet]
        [ProducesResponseType(typeof(DefaultResponse<IEnumerable<UserDto>>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(new DefaultResponse<object>(200, "Users succesfully retrieved.", true, users));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all users");
                return StatusCode(500, new DefaultResponse<object>(500, "Error retrieving all users", false));
            }
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Returns a user with the specified identifier or an appropriate error code on failure.</returns>
        /// <response code="200">Returns the user with the specified identifier.</response>
        /// <response code="404">If the user with the specified identifier is not found.</response>
        /// <response code="500">If there was an issue while retrieving the user.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<UserDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new DefaultResponse<object>(404, "User not found.", false));
                }
                return Ok(new DefaultResponse<object>(200, "User succesfully retrieved.", true, user));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving user by ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Error retrieving user by ID.", false));
            }
        }

        /// <summary>
        /// Retrieves all courses for the authenticated user.
        /// </summary>
        /// <returns>Returns a list of courses for the authenticated user.</returns>
        /// <response code="200">Returns the list of courses for the user.</response>
        /// <response code="401">If the user is unauthorized.</response>
        /// <response code="500">If there was an issue while retrieving user courses.</response>
        [Authorize]
        [HttpGet("courses")]
        [ProducesResponseType(typeof(DefaultResponse<UserCoursesResponseDto>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 401)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<UserCoursesResponseDto>> GetAllCoursesForUser()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new DefaultResponse<object>(401, "Unauthorized user", false));
                }

                var userDto = await _jwtService.ValidateToken(token);
                var courses = await _courseService.GetAllCoursesForUser(userDto);

                return Ok(new DefaultResponse<object>(200, "User courses successfuly retrieved", true, courses));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching user courses");
                return StatusCode(500, new DefaultResponse<object>(500, "Error retrieving user courses", false));
            }
        }

        /// <summary>
        /// Checks if the user is authenticated.
        /// </summary>
        /// <returns>Returns a message indicating user authentication status and user details if authenticated.</returns>
        /// <response code="200">Returns a message indicating user authentication status along with user details if authenticated.</response>
        /// <response code="401">If the token is not found or invalid.</response>
        [Authorize]
        [HttpGet("auth")]
        [ProducesResponseType(typeof(DefaultResponse<UserDto>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 401)]
        public async Task<ActionResult> CheckAuthentication()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new DefaultResponse<object>(401, "Token not found", false));
            }

            var userDto = await _jwtService.ValidateToken(token);

            if (userDto == null)
            {
                return Unauthorized(new DefaultResponse<object>(401, "Invalid token", false));
            }

            return Ok(new DefaultResponse<object>(200, "User authenticated", false, userDto));
        }

        /// <summary>
        /// Adds a course to the authenticated user.
        /// </summary>
        /// <param name="courseId">Course identifier to add.</param>
        /// <returns>Returns a message indicating the result of the operation.</returns>
        /// <response code="200">Returns a message indicating that the course was added successfully.</response>
        /// <response code="400">If the ModelState is invalid.</response>
        /// <response code="401">If the user is unauthorized or the token is not found.</response>
        [Authorize]
        [HttpPost("courses/{courseId}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 400)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 401)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> AddCourseToUser(Guid courseId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new DefaultResponse<object>(401, "Token not found", false));
                }

                var userDto = await _jwtService.ValidateToken(token);

                var isAdded = await _userService.AddCourseToUser(userDto.Id, courseId);

                if (isAdded)
                {
                    Log.Information("Course added to user successfully. UserId: {UserId}, CourseId: {CourseId}", userDto.Id, courseId);
                    return Ok(new DefaultResponse<object>(200, "Course added to user successfully", true));
                }
                else
                {
                    Log.Warning("Course could not be added to user. UserId: {UserId}, CourseId: {CourseId}", userDto.Id, courseId);
                    return BadRequest(new DefaultResponse<object>(400, "Course could not be added to user", false));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding course to user");
                return StatusCode(500, new DefaultResponse<object>(500, "Error adding course to user", false));
            }
        }

        /// <summary>
        /// Adds a role to the specified user.
        /// </summary>
        /// <param name="userId">User identifier to add the role to.</param>
        /// <param name="role">User role to add.</param>
        /// <returns>Returns a message indicating the result of the operation.</returns>
        /// <response code="200">Returns a message indicating that the role was added successfully.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpPost("{userId}/roles")]
        [ProducesResponseType(typeof(DefaultResponse<UserDto>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<IActionResult> AddUserRole(Guid userId, [FromBody] UserRole role)
        {
            try
            {
                var user = await _userService.AddUserRoleAsync(userId, role);
                if (user == null)
                {
                    Log.Warning("User not found. UserId: {UserId}", userId);
                    return NotFound(new DefaultResponse<object>(404, "User not found", false));
                }

                Log.Information("Role added to user successfully. UserId: {UserId}, Role: {Role}", userId, role);
                return Ok(new DefaultResponse<object>(200, "Role added to user successfully", true, user));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding role to user");
                return StatusCode(500, new DefaultResponse<object>(500, "Error adding role to user", false));
            }
        }


        /// <summary>
        /// Activate user account with help of activation token
        /// </summary>
        /// <param name="token">Activation token (sended to user email)</param>
        /// <returns>Information whether activation was successful</returns>
        /// <response code="200">If activation was successful</response>
        /// <response code="400">If the token was invalid or the account has already been activated.</response>
        /// <response code="500">If an unexpected error occurred during activation</response>
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateEmail([FromBody] TokenResponseDto token)
        {
            try
            {
                var activation = await _userService.ActivateUserEmail(token.Token);
                if (activation.Success)
                {
                    return Ok(new DefaultResponse<object>(200, "Email activated", true, null));
                }
                return BadRequest(new DefaultResponse<object>(400, activation.Message, true, null));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while email activation");
                return StatusCode(500, new DefaultResponse<object>(500, "Error while email activation", false));
            }
        }

    }
}
