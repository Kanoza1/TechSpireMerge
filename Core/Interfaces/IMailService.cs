using Microsoft.AspNetCore.Http;


namespace Core.Interfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(string mailTo,string subject,string body);
        public string GenerateCode();
        public bool IsExpired();
    }
}
