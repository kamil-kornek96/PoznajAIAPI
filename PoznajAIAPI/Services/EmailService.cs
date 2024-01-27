using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PoznajAI.Models;
using PoznajAI.Models.User;
using Serilog;
using System.Text;

namespace PoznajAI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUserService _userService;
        public EmailService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<string> SendEmailActivationMessage(Guid userId, string url)
        {
            try
            {
                var user = await _userService.GetUserById(userId);
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("Poznaj AI", "poznajai@outlook.com"));
                mailMessage.To.Add(new MailboxAddress(user.FirstName + " " + user.LastName, user.Email));
                mailMessage.Subject = "Poznaj AI - Aktywacja konta email";
                mailMessage.Body = new TextPart("html")
                {
                    Text = GetEmailTemplate(user, url)
                };
                string response = "";
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                    smtpClient.Authenticate("poznajai@outlook.com", "*V?mLmLcczN^9qE");
                    response = smtpClient.Send(mailMessage);
                    smtpClient.Disconnect(true);
                }

                return response;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while sending email");
                return null;
            }
        }

        private string GetEmailTemplate(UserDto user, string url)
        {
            string template = $"""
                <h2>Cześć {user.FirstName} {user.LastName}, potwierdź swój adres email</h2>

                <p>Przed skorzystaniem z serwisu musisz potwierdzić twój adres email klikając w poniższy link:</p>
                <p><a href="{url}/activation?token={user.EmailConfirmationToken}">{url}/activation?token={user.EmailConfirmationToken}</a></p>
                """;
            return template;
        }
    }
}
