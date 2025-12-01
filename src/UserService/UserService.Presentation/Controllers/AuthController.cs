using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, IEmailService emailService, IUserService userService)
        {
            _authService = authService;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDTO loginDto, CancellationToken cancellationToken = default)
        {
            var result = await _authService.AuthAsync(loginDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RefreshTokenAsync(request, cancellationToken);
            return Ok(result);
        }



        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] string email, CancellationToken cancellationToken)
        {
            var result = await _userService.GetEmailAndResetTokenAsync(email, cancellationToken);
            if (result == null) return NotFound("Пользователь не найден.");

            await _emailService.SendPasswordResetEmailAsync(result.Value.Email, result.Value.PasswordResetToken!, cancellationToken);
            return Ok("Письмо с токеном восстановления отправлено.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, string newPassword, CancellationToken cancellationToken)
        {
            var success = await _userService.ResetPasswordAsync(email, token, newPassword, cancellationToken);
            if (!success) return BadRequest("Неверный токен или email.");
            return Ok("Пароль успешно изменён.");
        }

        [HttpPost("swagger-auth")]
        [AllowAnonymous]
        public async Task<IActionResult> SwaggerAuth([FromForm] string username, [FromForm] string password, CancellationToken cancellationToken = default)
        {
            var loginDto = new LoginDTO { Email = username, Password = password };
            var result = await _authService.AuthAsync(loginDto, cancellationToken);

            if (!result.IsAuthenticated)
                return Unauthorized(new { error = result.ErrorMessage });

            return Ok(new
            {
                access_token = result.Token,
                token_type = "bearer",
                expires_in = 3600,
                refresh_token = result.RefreshToken
            });
        }
    }
}
