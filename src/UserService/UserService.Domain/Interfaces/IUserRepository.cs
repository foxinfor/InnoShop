using System.Security.Claims;
using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User> FindByEmailAsync(string? email, CancellationToken cancellationToken = default);
        Task<User?> GetUserAsync(ClaimsPrincipal principal);

        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        Task<User?> GetByConfirmationTokenAsync(string token, CancellationToken cancellationToken = default);

        Task<(string Email, string? ConfirmationToken)?> FindEmailAndTokenAsync(string email, CancellationToken cancellationToken);
    }
}
