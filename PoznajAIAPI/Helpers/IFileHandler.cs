namespace PoznajAI.Helpers
{
    public interface IFileHandler
    {
        Task<bool> SaveFile(string filePath, IFormFile file);
    }
}