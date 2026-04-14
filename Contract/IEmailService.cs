namespace onilne_service.Contract
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otp);
    }
}
