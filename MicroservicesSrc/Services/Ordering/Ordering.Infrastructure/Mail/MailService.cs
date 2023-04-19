using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Ordering.Infrastructure.Mail
{
    public class MailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public MailService(IOptions<EmailSettings> emailSettings) => _emailSettings = emailSettings.Value;

        public async Task<bool> SendEmailAsync(Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName),
                                                   new EmailAddress(email.To),
                                                   email.Subject,
                                                   email.Body,
                                                   email.Body);

            var response = await client.SendEmailAsync(msg);

            return response.StatusCode switch
            {
                var x when x == HttpStatusCode.Accepted || x == HttpStatusCode.OK => true,
                _ => false
            };
        }
    }
}
