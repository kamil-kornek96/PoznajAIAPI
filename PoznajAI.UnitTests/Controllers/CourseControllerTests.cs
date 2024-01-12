using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PoznajAI.Controllers;
using PoznajAI.Models;
using PoznajAI.Models.Course;
using PoznajAI.Services;

namespace PoznajAI.UnitTests.Controllers
{
    public class CourseControllerTests
    {
        private CourseController _courseController;
        private Mock<ICourseService> _courseServiceMock;

        [SetUp]
        public void Setup()
        {
            _courseServiceMock = new Mock<ICourseService>();
            _courseController = new CourseController(_courseServiceMock.Object);
        }


        [Test]
        public async Task GetCourseById_ExistingId_ReturnsCourse()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var courseDto = new CourseDto { Id = courseId, Title = "Test Course" };

            _courseServiceMock.Setup(service => service.GetCourseById(courseId))
                .ReturnsAsync(courseDto);

            // Act
            var result = await _courseController.GetCourseById(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<CourseDto>).Data, Is.EqualTo(courseDto));
        }

        [Test]
        public async Task GetCourseById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseServiceMock.Setup(service => service.GetCourseById(courseId))
                .ReturnsAsync((CourseDto)null);

            // Act
            var result = await _courseController.GetCourseById(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That((notFoundResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Course not found."));
        }

        [Test]
        public async Task GetCourseById_ServiceException_ReturnsInternalServerError()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            _courseServiceMock.Setup(service => service.GetCourseById(courseId))
                .ThrowsAsync(new Exception("Simulated service exception"));

            // Act
            var result = await _courseController.GetCourseById(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That((objectResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Problem with retrieving the course."));
        }

        [Test]
        public async Task CreateCourse_ValidCourse_ReturnsCreated()
        {
            // Arrange
            var courseDto = new CourseCreateDto { Title = "New Course" };
            var createdCourse = new CourseDto { Id = Guid.NewGuid(), Title = "New Course" };

            _courseServiceMock.Setup(service => service.CreateCourse(courseDto))
                .ReturnsAsync(createdCourse);

            // Act
            var result = await _courseController.CreateCourse(courseDto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.ActionName, Is.EqualTo("GetCourseById"));
            Assert.That((createdResult.RouteValues["id"] as Guid?), Is.EqualTo(createdCourse.Id));
            Assert.That((createdResult.Value as DefaultResponse<object>).Data, Is.EqualTo(createdCourse));
        }

        [Test]
        public async Task CreateCourse_ServiceException_ReturnsInternalServerError()
        {
            // Arrange
            var courseDto = new CourseCreateDto { Title = "New Course" };
            _courseServiceMock.Setup(service => service.CreateCourse(courseDto))
                .ThrowsAsync(new Exception("Simulated service exception"));

            // Act
            var result = await _courseController.CreateCourse(courseDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That((objectResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Problem with creating a course."));
        }

        [Test]
        public async Task UpdateCourse_ExistingId_ReturnsOk()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var courseDto = new CourseUpdateDto { Title = "Updated Course" };

            _courseServiceMock.Setup(service => service.UpdateCourse(courseId, courseDto))
                .ReturnsAsync(true);

            // Act
            var result = await _courseController.UpdateCourse(courseId, courseDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Course updated."));
        }

        [Test]
        public async Task UpdateCourse_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var courseDto = new CourseUpdateDto { Title = "Updated Course" };

            _courseServiceMock.Setup(service => service.UpdateCourse(courseId, courseDto))
                .ReturnsAsync(false);

            // Act
            var result = await _courseController.UpdateCourse(courseId, courseDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That((notFoundResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Course to update not found."));
        }

        [Test]
        public async Task UpdateCourse_ServiceException_ReturnsInternalServerError()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var courseDto = new CourseUpdateDto { Title = "Updated Course" };

            _courseServiceMock.Setup(service => service.UpdateCourse(courseId, courseDto))
                .ThrowsAsync(new Exception("Simulated service exception"));

            // Act
            var result = await _courseController.UpdateCourse(courseId, courseDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That((objectResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Problem with updating the course."));
        }

        [Test]
        public async Task DeleteCourse_ExistingId_ReturnsOk()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseServiceMock.Setup(service => service.DeleteCourse(courseId))
                .ReturnsAsync(true);

            // Act
            var result = await _courseController.DeleteCourse(courseId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Course deleted."));
        }

        [Test]
        public async Task DeleteCourse_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseServiceMock.Setup(service => service.DeleteCourse(courseId))
                .ReturnsAsync(false);

            // Act
            var result = await _courseController.DeleteCourse(courseId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That((notFoundResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Course to delete not found."));
        }

        [Test]
        public async Task DeleteCourse_ServiceException_ReturnsInternalServerError()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseServiceMock.Setup(service => service.DeleteCourse(courseId))
                .ThrowsAsync(new Exception("Simulated service exception"));

            // Act
            var result = await _courseController.DeleteCourse(courseId);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));
            Assert.That((objectResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Problem with deleting the course."));
        }


    }
}
