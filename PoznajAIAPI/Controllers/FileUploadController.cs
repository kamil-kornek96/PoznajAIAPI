using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Threading.Tasks;

namespace PoznajAI.Controllers
{
    [Route("api")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private const string UploadFolder = "uploads"; // Katalog, w którym będą przechowywane przesłane pliki

        [HttpPost("video-upload")]
        public async Task<IActionResult> UploadVideoAsync()
        {
            try
            {
                var file = Request.Form.Files[0]; // Pobierz przesłany plik

                if (file.Length > 0)
                {
                    var fileName = Path.Combine(UploadFolder, Path.GetRandomFileName().Split('.').First()+'.'+file.FileName.Split('.').Last()); // Generuj unikalną nazwę pliku
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); // Zapisz przesłany plik na dysku
                    }

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

        [HttpGet("uploads/{fileName}")]
        public IActionResult GetVideo(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),UploadFolder, fileName);

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
