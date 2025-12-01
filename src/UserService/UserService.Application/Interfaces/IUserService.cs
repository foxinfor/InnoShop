using UserService.Application.DTOs;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, CreateUserDTO updateUserDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ConfirmEmailAsync(string token, CancellationToken cancellationToken);
        Task<(string Email, string? ConfirmationToken)?> FindEmailAndTokenAsync(string email, CancellationToken cancellationToken);
        Task<(string Email, string? PasswordResetToken)?> GetEmailAndResetTokenAsync(string email, CancellationToken cancellationToken);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken);

        Task<bool> SetActivationStatusAsync(string email, bool isActive, CancellationToken cancellationToken);
        Task<UserDTO?> GetByEmailAsync(string email, CancellationToken ct);
    }
}