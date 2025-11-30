using System.Security.Claims;
using UserService.Application.DTOs;

namespace UserService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> AuthAsync(LoginDTO loginDto, CancellationToken cancellationToken = default);
        Task<RegistrationResponseDTO> RegisterAsync(LoginDTO registerDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
        Task LogoutAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    }
}
