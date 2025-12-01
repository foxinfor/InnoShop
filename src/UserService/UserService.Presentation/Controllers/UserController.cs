using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        
        public UserController(IUserService userService, IEmailService emailService)
        {
            _emailService = emailService;
            _userService = userService;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Create(CreateUserDTO userDTO, CancellationToken cancellationToken = default)
        {
            var user = await _userService.CreateAsync(userDTO, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateUserDTO userDTO, CancellationToken cancellationToken = default)
        {
            await _userService.UpdateAsync(id, userDTO, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, CancellationToken cancellationToken)
        {
            var result = await _userService.ConfirmEmailAsync(token, cancellationToken);
            if (!result)
                return BadRequest("Неверный или просроченный токен.");
            return Ok("Email успешно подтверждён!");
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] string email, CancellationToken cancellationToken)
        {
            var result = await _userService.FindEmailAndTokenAsync(email, cancellationToken);
            if (result == null) return NotFound("Пользователь не найден.");

            await _emailService.SendConfirmationEmailAsync(result.Value.Email, result.Value.ConfirmationToken!, cancellationToken);

            return Ok("Письмо повторно отправлено.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("set-activation")]
        public async Task<IActionResult> SetActivation([FromQuery] string email, [FromQuery] bool isActive, CancellationToken cancellationToken)
        {
            var success = await _userService.SetActivationStatusAsync(email, isActive, cancellationToken);
            if (!success) return NotFound("Пользователь не найден.");

            var user = await _userService.GetByEmailAsync(email, cancellationToken);
            if (user == null) return NotFound("Пользователь не найден.");

            using var client = new HttpClient();
            var url = $"https://localhost:7231/api/products/set-availability/{user.Id}?isAvailable={isActive}";
            var response = await client.PutAsync(url, null, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Ошибка при обновлении продуктов пользователя.");
            }

            return Ok(isActive ? "Пользователь активирован." : "Пользователь деактивирован.");
        }
    }
}
