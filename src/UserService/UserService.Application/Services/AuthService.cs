using AutoMapper;
using System.Security.Claims;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> AuthAsync(LoginDTO loginDto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByEmailAsync(loginDto.Email, cancellationToken);

            if (user == null)
            {
                return new AuthResponseDTO
                {
                    IsAuthenticated = false,
                    ErrorMessage = "Пользователь не найден."
                };
            }

            if (!user.CheckPassword(loginDto.Password))
            {
                return new AuthResponseDTO
                {
                    IsAuthenticated = false,
                    ErrorMessage = "Неверный пароль."
                };
            }

            var roles = user.GetRoles();
            var token = _tokenService.GenerateJwtToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
            
            await _userRepository.UpdateAsync(user, cancellationToken);

            var response = _mapper.Map<AuthResponseDTO>(user);
            response.Token = token;
            response.RefreshToken = refreshToken;
            response.IsAuthenticated = true;
            return response;
        }

        public Task LogoutAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RegistrationResponseDTO> RegisterAsync(LoginDTO registerDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
