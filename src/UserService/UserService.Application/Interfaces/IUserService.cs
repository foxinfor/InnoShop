using UserService.Application.DTO;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO);
        Task UpdateAsync(Guid id, UpdateUserDTO updateUserDTO);
        Task DeleteAsync(Guid id);
    }
}