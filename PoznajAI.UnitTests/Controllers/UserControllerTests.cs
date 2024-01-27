using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PoznajAI.Controllers;
using PoznajAI.Data.Models;
using PoznajAI.Models;
using PoznajAI.Models.Auth;
using PoznajAI.Models.Course;
using PoznajAI.Models.User;
using PoznajAI.Services;

namespace PoznajAI.UnitTests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private Mock<IUserService> _userServiceMock;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<ICourseService> _courseServiceMock;
        private Mock<IEmailService> _emailServiceMock;
        private TokenResponseDto _token;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _jwtServiceMock = new Mock<IJwtService>();
            _courseServiceMock = new Mock<ICourseService>();
            _emailServiceMock = new Mock<IEmailService>();


            _userController = new UserController(_userServiceMock.Object, _jwtServiceMock.Object, _courseServiceMock.Object, _emailServiceMock.Object);

            _token = new TokenResponseDto { Token = "example_token" };
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var headers = new HeaderDictionary { { "Authorization", _token.Token } };
            request.SetupGet(r => r.Headers).Returns(headers);
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.Setup(c => c.Request.Scheme).Returns("http");
            httpContext.Setup(c => c.Request.Host).Returns(new HostString("example.com"));
            _userController.ControllerContext = new ControllerContext { HttpContext = httpContext.Object };
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequestDto = new LoginRequestDto
            {
                Email = "test@example.com",
                Password = "password"
            };
            TokenResponseDto expectedToken = new TokenResponseDto { Token = "mocked.jwt.token" };
            var userDto = new UserDto { Id = Guid.NewGuid(), Email = "test@example.com", IsEmailConfirmed = true };

            _userServiceMock.Setup(service => service.Authenticate(loginRequestDto.Email, loginRequestDto.Password))
                .ReturnsAsync(userDto);
            _jwtServiceMock.Setup(service => service.GenerateToken(userDto))
                .Returns(expectedToken);

            // Act
            var result = await _userController.Login(loginRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<TokenResponseDto>).Data, Is.EqualTo(expectedToken));
        }

        [Test]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var registerRequestDto = new RegisterRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Password = "password"
            };

            var userDto = new UserDto { Id = guid, Email = "johndoe@example.com" };

            _userServiceMock.Setup(service => service.IsEmailTaken(It.Is<string>(e => e == registerRequestDto.Email)))
                .ReturnsAsync(false);
            _userServiceMock.Setup(service => service.CreateUser(It.IsAny<UserCreateDto>()))
                .ReturnsAsync(userDto.Id);
            _userServiceMock.Setup(service => service.Authenticate(It.Is<string>(e => e == registerRequestDto.Email), It.Is<string>(p => p == registerRequestDto.Password)))
                .ReturnsAsync(userDto);
            _jwtServiceMock.Setup(service => service.GenerateToken(It.Is<UserDto>(u => u == userDto)))
                .Returns(_token);

            // Act
            var result = await _userController.Register(registerRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetAllUsers_ReturnsListOfUsers()
        {
            // Arrange
            var userDtoList = new List<UserDto>
            {
            new UserDto { Id = Guid.NewGuid(), Email = "user1@example.com" },
            new UserDto { Id = Guid.NewGuid(), Email = "user2@example.com" }

            };

            _userServiceMock.Setup(service => service.GetAllUsers())
                .ReturnsAsync(userDtoList);

            // Act
            var result = await _userController.GetAllUsers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Data, Is.EquivalentTo(userDtoList));
        }

        [Test]
        public async Task GetUserById_ExistingId_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { Id = userId, Email = "existinguser@example.com" };

            _userServiceMock.Setup(service => service.GetUserById(userId))
                .ReturnsAsync(userDto);

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Data, Is.EqualTo(userDto));
        }

        [Test]
        public async Task GetAllCoursesForUser_ValidToken_ReturnsListOfCourses()
        {
            // Arrange
            var userDto = new UserDto { Id = Guid.NewGuid(), Email = "test@example.com" };
            var userCoursesResponseDto = new UserCoursesResponseDto
            {
                AllCourses = new List<CourseDto>
                {
                    new CourseDto { Id = Guid.NewGuid(), Title = "All Course 1" },
                    new CourseDto { Id = Guid.NewGuid(), Title = "All Course 2" }
                },
                OwnedCourses = new List<OwnedCourseDto>
                {
                    new OwnedCourseDto { Id = Guid.NewGuid(), Title = "Owned Course 1" },
                    new OwnedCourseDto { Id = Guid.NewGuid(), Title = "OwnedCourse 2" }
                }
            };

            _jwtServiceMock.Setup(service => service.ValidateToken(It.Is<string>(t => t == _token.Token)))
                .ReturnsAsync(userDto);
            _courseServiceMock.Setup(service => service.GetAllCoursesForUser(It.IsAny<UserDto>()))
                .ReturnsAsync(userCoursesResponseDto);


            // Act
            var result = await _userController.GetAllCoursesForUser();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            var data = ((okResult.Value as DefaultResponse<object>).Data as UserCoursesResponseDto);
            Assert.That(data.OwnedCourses, Is.EquivalentTo(userCoursesResponseDto.OwnedCourses));
            Assert.That(data.AllCourses, Is.EquivalentTo(userCoursesResponseDto.AllCourses));
        }

        [Test]
        public async Task CheckAuthentication_ValidToken_ReturnsAuthenticatedUser()
        {
            // Arrange
            var userDto = new UserDto { Id = Guid.NewGuid(), Email = "test@example.com" };

            _jwtServiceMock.Setup(service => service.ValidateToken(It.Is<string>(t => t ==
            _token.Token)))
                .ReturnsAsync(userDto);

            // Act
            var result = await _userController.CheckAuthentication();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Data, Is.EqualTo(userDto));
        }

        [Test]
        public async Task AddCourseToUser_ValidCourseId_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var userDto = new UserDto { Id = userId, Email = "test@example.com" };

            _jwtServiceMock.Setup(service => service.ValidateToken(It.Is<string>(t => t == _token.Token)))
                .ReturnsAsync(userDto);
            _userServiceMock.Setup(service => service.AddCourseToUser(
                It.Is<Guid>(id => id == userId),
                It.Is<Guid>(id => id == courseId)
            ))
            .ReturnsAsync(true);


            // Act
            var result = await _userController.AddCourseToUser(courseId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Success, Is.EqualTo(true));
        }

        [Test]
        public async Task AddUserRole_ValidUserId_ReturnsUserWithRole()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto { Id = userId, Email = "test@example.com" };
            var role = UserRole.Admin; // or any role

            _userServiceMock.Setup(service => service.AddUserRoleAsync(userId, role))
                .ReturnsAsync(userDto);

            // Act
            var result = await _userController.AddUserRole(userId, role);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Data, Is.EqualTo(userDto));
        }


    }
}
