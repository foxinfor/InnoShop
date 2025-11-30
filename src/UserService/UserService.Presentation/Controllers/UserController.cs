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
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
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

    }
}
