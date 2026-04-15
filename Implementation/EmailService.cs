using Microsoft.Extensions.Options;
using onilne_service.Contract;
using onilne_service.Model;
using System.Net;
using System.Net.Mail;
using Resend;

public class EmailService: IEmailService
{
    private readonly EmailSettings _settings;
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IOptions<EmailSettings> settings, IConfiguration config, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _config = config;
        _logger = logger;
    }

    public async Task SendOtpAsync(string toEmail, string otp)
    {
        try
        {
            var apiKey = _config["Resend:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("Resend API key is missing");

            var resend = ResendClient.Create(apiKey);

            var response = await resend.EmailSendAsync(new EmailMessage()
            {
                From = "noreply@voltmart.uk",
                To = toEmail,
                Subject = "Your OTP Code",
                HtmlBody = $"<h2>Your OTP is: {otp}</h2>"
            });

            if (response == null)
            {
                _logger.LogError("Resend returned null response for {Email}", toEmail);
                throw new Exception("Failed to send email");
            }

            _logger.LogInformation("OTP email sent successfully to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending OTP to {Email}", toEmail);
            throw;
        }
    }
}