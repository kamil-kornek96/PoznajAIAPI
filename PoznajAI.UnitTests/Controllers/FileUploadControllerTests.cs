using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PoznajAI.Controllers;
using PoznajAI.Helpers;
using PoznajAI.Models;
using PoznajAI.Services.Video;

namespace PoznajAI.UnitTests.Controllers
{
    [TestFixture]
    public class FileUploadControllerTests
    {
        private FileUploadController _fileUploadController;
        private Mock<IVideoConversionService> _conversionServiceMock;
        private Mock<IHangfireJobEnqueuer> _hangfireJobEnqueuerMock;
        private Mock<IFileHandler> _fileServiceMock;

        [SetUp]
        public void Setup()
        {
            _conversionServiceMock = new Mock<IVideoConversionService>();
            _fileServiceMock = new Mock<IFileHandler>();
            _hangfireJobEnqueuerMock = new Mock<IHangfireJobEnqueuer>();
            _fileUploadController = new FileUploadController(_conversionServiceMock.Object, _fileServiceMock.Object, _hangfireJobEnqueuerMock.Object);
        }

        [Test]
        public async Task UploadVideoAsync_Success()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.FileName).Returns("example.mp4");
            formFileMock.Setup(x => x.Length).Returns(1);
            formFileMock.Setup(x => x.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _conversionServiceMock.Setup(x => x.ConvertVideo(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var formCollection = new FormCollection(new System.Collections.Generic.Dictionary<string, 
                Microsoft.Extensions.Primitives.StringValues>(), new FormFileCollection { formFileMock.Object });

            _fileUploadController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Request = { Form = formCollection } }
            };

            _fileServiceMock.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync(true);

            // Act
            var result = await _fileUploadController.UploadVideoAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("File uploaded."));
        }

        [Test]
        public async Task UploadVideoAsync_EmptyFile()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.Length).Returns(0); // simulate an empty file

            var formCollection = new FormCollection(new System.Collections.Generic.Dictionary<string, 
                Microsoft.Extensions.Primitives.StringValues>(), new FormFileCollection { formFileMock.Object });

            _fileUploadController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Request = { Form = formCollection } }
            };

            // Act
            var result = await _fileUploadController.UploadVideoAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var okResult = result as ObjectResult;
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("File is empty."));
        }

        [Test]
        public async Task UploadVideoAsync_SaveFileFailure()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(x => x.FileName).Returns("example.mp4");
            formFileMock.Setup(x => x.Length).Returns(1); // simulate a non-empty file
            formFileMock.Setup(x => x.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var formCollection = new FormCollection(new System.Collections.Generic.Dictionary<string, 
                Microsoft.Extensions.Primitives.StringValues>(), new FormFileCollection { formFileMock.Object });

            _fileUploadController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { Request = { Form = formCollection } }
            };

            _fileServiceMock.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync(false);

            // Act
            var result = await _fileUploadController.UploadVideoAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var okResult = result as ObjectResult;
            Assert.That((okResult.Value as DefaultResponse<object>).Message, Is.EqualTo("File upload failed. Unable to save the file."));
        }

    }
}
