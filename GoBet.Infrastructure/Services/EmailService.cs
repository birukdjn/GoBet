using GoBet.Application.Interfaces;
using GoBet.Infrastructure.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;


namespace GoBet.Infrastructure.Services
{
    public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
    {
        private readonly EmailSettings _settings = emailSettings.Value;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetLink)
        {
            string subject = "Reset Your GoBet Password";
            string body = $@"
                <h1>Password Reset Request</h1>
                <p>You requested to reset your password for your GoBet Transport account.</p>
                <p>Please click the link below to set a new password. This link expires in 1 hour.</p>
                <a href='{resetLink}' style='padding: 10px; background-color: #007bff; color: white; text-decoration: none;'>Reset Password</a>
                <p>If you did not request this, please ignore this email.</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
}