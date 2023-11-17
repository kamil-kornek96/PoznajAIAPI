using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PoznajAI.Services.Video;

namespace PoznajAI.Controllers
{
    [Route("api/uploads")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private const string TempFolder = "uploads\\temp";
        private const string VideosFolder = "uploads\\videos";
        private readonly IVideoConversionService _conversionService;

        public FileUploadController(IVideoConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost("video")]
        public async Task<IActionResult> UploadVideoAsync()
        {
            try
            {
                var file = Request.Form.Files[0]; // Pobierz przesłany plik

                if (file.Length > 0)
                {
                    var fileName = Path.Combine(Path.GetRandomFileName().Split('.').First() + '.' + file.FileName.Split('.').Last()); // Generuj unikalną nazwę pliku
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), TempFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); // Zapisz przesłany plik na dysku
                    }

                    BackgroundJob.Enqueue(() => _conversionService.ConvertVideo(filePath, CancellationToken.None));

                    // Tutaj możesz dodać dodatkową logikę, np. zapis do bazy danych, przetwarzanie itp.

                    return Ok(new { fileName }); // Zwróć nazwę zapisanego pliku
                }

                return BadRequest("Plik jest pusty.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }

        [HttpGet("video/{fileName}")]
        public IActionResult GetVideo(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), VideosFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    // Określ MIME Type na podstawie rozszerzenia pliku
                    var mimeType = "application/octet-stream"; // Domyślny MIME Type

                    var provider = new FileExtensionContentTypeProvider();
                    if (provider.TryGetContentType(fileName, out var outMimeType))
                    {
                        mimeType = outMimeType;
                    }

                    return File(fileStream, mimeType, fileName);
                }

                filePath = Path.Combine(Directory.GetCurrentDirectory(), TempFolder, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    // Określ MIME Type na podstawie rozszerzenia pliku
                    var mimeType = "application/octet-stream"; // Domyślny MIME Type

                    var provider = new FileExtensionContentTypeProvider();
                    if (provider.TryGetContentType(fileName, out var outMimeType))
                    {
                        mimeType = outMimeType;
                    }

                    return File(fileStream, mimeType, fileName);
                }

                return NotFound("Plik nie istnieje.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }
    }
}
