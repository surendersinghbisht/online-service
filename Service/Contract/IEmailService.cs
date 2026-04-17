namespace onilne_service.Service.Contract
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otp);
    }
}
