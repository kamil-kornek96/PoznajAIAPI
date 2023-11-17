namespace PoznajAI.Services.Video
{
    public interface IVideoConversionService
    {
        Task ConvertVideo(string inputFilePath, CancellationToken cancellationToken = default);
    }
}