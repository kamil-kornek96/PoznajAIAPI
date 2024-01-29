namespace PoznajAI.Services
{
    public interface IEmailService
    {
        Task<string> SendEmailActivationMessage(Guid userId, string url);
    }
}