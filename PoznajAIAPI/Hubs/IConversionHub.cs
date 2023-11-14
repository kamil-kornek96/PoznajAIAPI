namespace PoznajAI.Hubs
{
    public interface IConversionHub
    {
        Task SendConversionStatus(string fileName, string status);
    }
}