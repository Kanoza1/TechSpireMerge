using Core.Entities;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace Infrastructure.Services
{
    public class MailService : Core.Interfaces.IMailService
    {
        private readonly EmailSettings emailSettings;
        public MailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public string GenerateCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public bool IsExpired()
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body)
        {
           var email= new MimeMessage()
            {
             Sender=MailboxAddress.Parse(emailSettings.Email),
             Subject=subject
            };
            email.To.Add(MailboxAddress.Parse(mailTo));
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.From.Add(new MailboxAddress(emailSettings.DisplayName,emailSettings.Email));
            email.Body= builder.ToMessageBody();
            using var smtp=new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);          
        }
    }
}
