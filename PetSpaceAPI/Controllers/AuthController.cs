using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetSpaceAPI.Services;
using PetSpaceAPI.Models.DTO;

namespace PetSpaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserService _userService; // Przyjmujemy, że masz serwis do obsługi użytkowników

        public AuthController(JwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user.Id);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto model)
        {
            // Sprawdź, czy użytkownik o podanej nazwie nie istnieje już w bazie danych
            if (_userService.IsUsernameTaken(model.Username))
            {
                return BadRequest("Username already taken.");
            }

            var user = new User
            {
                Username = model.Username,
                // Możesz dodatkowo przekazać i zahaszować hasło itp.
            };

            _userService.CreateUser(user);

            // Tutaj możesz dodać logikę, która w razie potrzeby generuje token JWT po rejestracji

            return Ok("Registration successful.");
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Możesz tutaj dodać dodatkową logikę, jeśli wymaga to wylogowanie

            return Ok("Logged out successfully.");
        }
    }
}
