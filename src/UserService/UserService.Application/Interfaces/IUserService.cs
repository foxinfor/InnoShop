using UserService.Application.DTOs;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, UpdateUserDTO updateUserDTO, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}