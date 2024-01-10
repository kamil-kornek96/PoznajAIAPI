using Serilog;

namespace PoznajAI.Helpers
{
    public class FileHandler : IFileHandler
    {
        public async Task<bool> SaveFile(string filePath, IFormFile file)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during saving file");
                return false;
            }
        }
    }
}
