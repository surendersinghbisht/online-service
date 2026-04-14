using Microsoft.Extensions.Options;
using onilne_service.Contract;
using onilne_service.Model;
using System.Net;
using System.Net.Mail;

public class EmailService: IEmailService
{
    private readonly EmailSettings _settings;
    private readonly IConfiguration _config;

    public EmailService(IOptions<EmailSettings> settings, IConfiguration config)
    {
        _settings = settings.Value;
        _config = config;
    }

    public async Task SendOtpAsync(string toEmail, string otp)
    {
        var fromEmail = _config["EmailSettings:Email"];

        if (string.IsNullOrEmpty(toEmail))
            throw new Exception("Recipient email is NULL");

        if (string.IsNullOrEmpty(fromEmail))
            throw new Exception("Sender email is NULL (check appsettings)");

        var client = new SmtpClient(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]))
        {
            Credentials = new NetworkCredential(fromEmail, _config["EmailSettings:Password"]),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = "OTP Code",
            Body = $"Your OTP is {otp}"
        };

        mail.To.Add(toEmail);

        await client.SendMailAsync(mail);
    }
}