using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO createUserDTO, CancellationToken cancellationToken)
        {
            var user = new User(createUserDTO.FirstName, createUserDTO.LastName, createUserDTO.Email, createUserDTO.Password);
            var created = await _repository.AddAsync(user, cancellationToken);
            return new UserDTO(created.Id, created.FirstName, created.LastName, created.Email, created.PasswordHash,created.Role, false, false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllAsync(cancellationToken);
            return users.Select(u => new UserDTO(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PasswordHash,
                u.Role,
                u.IsActivate,
                u.EmailConfirmed
            ));
        }

        public async Task<UserDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);

            return new UserDTO(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PasswordHash,
                user.Role,
                user.IsActivate,
                user.EmailConfirmed
            );
        }
        public async Task UpdateAsync(Guid id, CreateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);
            user.UpdateDetails(updateUserDTO.FirstName, updateUserDTO.LastName, updateUserDTO.Email);
            await _repository.UpdateAsync(user);
        }
        public async Task<bool> ConfirmEmailAsync(string token, CancellationToken cancellationToken = default)
        {
            var user = await _repository.GetByConfirmationTokenAsync(token, cancellationToken);
            if (user == null)
                return false;

            user.ConfirmEmail();

            await _repository.UpdateAsync(user, cancellationToken);
            return true;
        }
        public async Task<(string Email, string? ConfirmationToken)?> FindEmailAndTokenAsync(string email, CancellationToken cancellationToken)
        {
            return await _repository.FindEmailAndTokenAsync(email, cancellationToken);
        }





        public async Task<(string Email, string? PasswordResetToken)?> GetEmailAndResetTokenAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _repository.FindByEmailAsync(email, cancellationToken);
            if (user == null) return null;
            return (user.Email, user.PasswordResetToken);
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken)
        {
            var user = await _repository.FindByEmailAsync(email, cancellationToken);
            if (user == null || user.PasswordResetToken != token) return false;

            user.SetPassword(newPassword);
            user.ClearPasswordResetToken();

            await _repository.UpdateAsync(user, cancellationToken);
            return true;
        }


        public async Task<bool> SetActivationStatusAsync(string email, bool isActive, CancellationToken cancellationToken)
        {
            var user = await _repository.FindByEmailAsync(email, cancellationToken);
            if (user == null) return false;

            if (isActive)
                user.Activate();
            else
                user.Deactivate();

            await _repository.UpdateAsync(user, cancellationToken);
            return true;
        }

        public async Task<UserDTO?> GetByEmailAsync(string email, CancellationToken ct)
        {
            var user = await _repository.FindByEmailAsync(email, ct);
            if (user == null) return null;

            return new UserDTO(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PasswordHash,
                user.Role,
                user.IsActivate,
                user.EmailConfirmed
            );
        }
    }
}
