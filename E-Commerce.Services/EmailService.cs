
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSetting _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSetting> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            using var smtpClient = new SmtpClient(_emailSettings.Host)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation($"✅ Email sent successfully to {MaskEmail(toEmail)}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to send email to {Email}", MaskEmail(toEmail));
            throw; // إعادة رمي الاستثناء حتى يتم التعامل معه في أماكن أخرى
        }
    }

    private string MaskEmail(string email)
            {
                var atIndex = email.IndexOf('@');
                return atIndex > 2 ? email.Substring(0, 2) + "****" + email.Substring(atIndex) : email;
            }
}