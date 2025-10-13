using Store.Services.Helper.Email;

namespace Store.Services.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(TempEmail email);
    }
}
