using Microsoft.AspNetCore.Identity.UI.Services;

namespace FinancialAssistent.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Реалізуйте тут код для відправки email
            // Наприклад, використовуючи сторонні API або SMTP

            return Task.CompletedTask;
        }
    }

}
