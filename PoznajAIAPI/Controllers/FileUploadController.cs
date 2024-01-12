using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PoznajAI.Helpers;
using PoznajAI.Models;
using PoznajAI.Services.Video;
using Serilog;

namespace PoznajAI.Controllers
{
    [Route("api/uploads")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private const string TempFolder = "uploads\\temp";
        private const string VideosFolder = "uploads\\videos";
        private readonly IVideoConversionService _conversionService;
        private readonly IFileHandler _fileService;
        private readonly IHangfireJobEnqueuer _hangfireJobEnqueuer;

        public FileUploadController(IVideoConversionService conversionService, IFileHandler fileService, IHangfireJobEnqueuer hangfireJobEnqueuer)
        {
            _conversionService = conversionService;
            _fileService = fileService;
            _hangfireJobEnqueuer = hangfireJobEnqueuer;
        }

        /// <summary>
        /// Uploads a video file and return it's name.
        /// </summary>
        /// <returns>Returns the name of the saved file or an appropriate error message.</returns>
        /// <response code="200">Returns if the file was uploaded correctly.</response>
        /// <response code="400">If file is empty.</response>
        /// <response code="500">If there was an issue while uploading the video.</response>
        [RequestSizeLimit(100_000_000)]
        [HttpPost("video")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 400)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<IActionResult> UploadVideoAsync()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var fileName = Path.Combine(Path.GetRandomFileName().Split('.').First() + '.' + file.FileName.Split('.').Last());
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), TempFolder, fileName);

                    var success = await _fileService.SaveFile(filePath, file);

                    if (success)
                    {
                        _hangfireJobEnqueuer.Enqueue(() => _conversionService.ConvertVideo(filePath, CancellationToken.None));


                        Log.Information("Uploaded file: {FileName}", fileName);

                        return Ok(new DefaultResponse<object>(200, "File uploaded.", true, new { fileName }));
                    }

                    Log.Error("File upload failed. Unable to save the file.");
                    return StatusCode(500, new DefaultResponse<object>(500, "File upload failed. Unable to save the file.", false));
                }

                return BadRequest(new DefaultResponse<object>(400, "File is empty.", false));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while uploading a file.");
                return StatusCode(500, new DefaultResponse<object>(500, "An error occurred while uploading a file.", false));
            }
        }

        /// <summary>
        /// Gets the specified video file by name.
        /// </summary>
        /// <param name="fileName">The name of the video file to retrieve.</param>
        /// <returns>Returns the requested video file or an appropriate error message.</returns>
        /// <response code="200">Returns the file.</response>
        /// <response code="404">If file was not found.</response>
        /// <response code="500">If there was an issue while retrieving the file.</response>
        [HttpGet("video/{fileName}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public IActionResult GetVideo(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), VideosFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    var mimeType = "application/octet-stream";

                    var provider = new FileExtensionContentTypeProvider();
                    if (provider.TryGetContentType(fileName, out var outMimeType))
                    {
                        mimeType = outMimeType;
                    }

                    Log.Information("Retrieved file: {FileName}", fileName);

                    return File(fileStream, mimeType, fileName);
                }

                filePath = Path.Combine(Directory.GetCurrentDirectory(), TempFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    var mimeType = "application/octet-stream";

                    var provider = new FileExtensionContentTypeProvider();
                    if (provider.TryGetContentType(fileName, out var outMimeType))
                    {
                        mimeType = outMimeType;
                    }

                    Log.Information("Retrieved file: {FileName}", fileName);

                    return File(fileStream, mimeType, fileName);
                }

                return NotFound(new DefaultResponse<object>(404, "File not found.", false));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving a file.");
                return StatusCode(500, new DefaultResponse<object>(500, "An error occurred while retrieving a file.", false));
            }
        }
    }
}
