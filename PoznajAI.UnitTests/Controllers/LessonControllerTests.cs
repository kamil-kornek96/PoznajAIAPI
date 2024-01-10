using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PoznajAI.Controllers;
using PoznajAI.Models;
using PoznajAI.Models.Course;
using PoznajAI.Models.Lesson;
using PoznajAI.Services;

namespace PoznajAI.UnitTests.Controllers
{
    [TestFixture]
    public class LessonControllerTests
    {
        private LessonController _lessonController;
        private Mock<ILessonService> _lessonServiceMock;
        private Mock<ICourseService> _courseServiceMock;

        [SetUp]
        public void Setup()
        {
            _lessonServiceMock = new Mock<ILessonService>();
            _courseServiceMock = new Mock<ICourseService>();
            _lessonController = new LessonController(_lessonServiceMock.Object, _courseServiceMock.Object);
        }

        [Test]
        public async Task GetLessonById_ExistingId_ReturnsLesson()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var lessonDto = new LessonDetailsDto { Id = lessonId, Title = "Sample Lesson" };

            _lessonServiceMock.Setup(service => service.GetLessonById(lessonId))
                .ReturnsAsync(lessonDto);

            // Act
            var result = await _lessonController.GetLessonById(lessonId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<LessonDetailsDto>).Data, Is.EqualTo(lessonDto));
        }

        [Test]
        public async Task CreateLesson_ValidLesson_ReturnsCreated()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var lessonDto = new CreateLessonDto { CourseId = courseId, Title = "New Lesson" };
            var courseDto = new CourseDto { Id = courseId, Title = "Sample Course" };

            _courseServiceMock.Setup(service => service.GetCourseById(courseId))
                .ReturnsAsync(courseDto);
            _lessonServiceMock.Setup(service => service.CreateLesson(lessonDto))
                .ReturnsAsync(new LessonDto { Id = Guid.NewGuid(), Title = "New Lesson" });

            // Act
            var result = await _lessonController.CreateLesson(lessonDto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That((createdResult.Value as DefaultResponse<LessonDto>).Data.Title, Is.EqualTo("New Lesson"));
        }

        [Test]
        public async Task UpdateLesson_ExistingId_ReturnsUpdated()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var lessonDto = new UpdateLessonDto { Id = lessonId, Title = "Updated Lesson" };
            var existingLessonDto = new LessonDetailsDto { Id = lessonId, Title = "Existing Lesson" };

            _lessonServiceMock.Setup(service => service.GetLessonById(lessonId))
                .ReturnsAsync(existingLessonDto);

            // Act
            var result = await _lessonController.UpdateLesson(lessonId, lessonDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Lesson updated."));
        }

        [Test]
        public async Task DeleteLesson_ExistingId_ReturnsDeleted()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            _lessonServiceMock.Setup(service => service.DeleteLesson(lessonId))
                .ReturnsAsync(true);

            // Act
            var result = await _lessonController.DeleteLesson(lessonId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Lesson deleted."));
        }

        [Test]
        public async Task CreateLesson_CourseNotFound_ReturnsNotFound()
        {
            // Arrange
            var lessonDto = new CreateLessonDto { CourseId = Guid.NewGuid(), Title = "New Lesson" };

            _courseServiceMock.Setup(service => service.GetCourseById(lessonDto.CourseId))
                .ReturnsAsync((CourseDto)null);

            // Act
            var result = await _lessonController.CreateLesson(lessonDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That((notFoundResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Associated course not found for the lesson."));
        }

        [Test]
        public async Task UpdateLesson_LessonNotFound_ReturnsNotFound()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var lessonDto = new UpdateLessonDto { Id = lessonId, Title = "Updated Lesson" };

            _lessonServiceMock.Setup(service => service.GetLessonById(lessonId))
                .ReturnsAsync((LessonDetailsDto)null);

            // Act
            var result = await _lessonController.UpdateLesson(lessonId, lessonDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That((notFoundResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Lesson to update not found."));
        }

        [Test]
        public async Task DeleteLesson_LessonNotFound_ReturnsNotFound()
        {
            // Arrange
            var lessonId = Guid.NewGuid();

            _lessonServiceMock.Setup(service => service.DeleteLesson(lessonId))
                .ReturnsAsync(false);

            // Act
            var result = await _lessonController.DeleteLesson(lessonId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            Assert.That((notFoundResult.Value as DefaultResponse<object>).Message, Is.EqualTo("Lesson to delete not found."));
        }

    }
}
