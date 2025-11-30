namespace UserService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string token, CancellationToken cancellationToken = default);
    }
}
