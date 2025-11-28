namespace UserService.Application.DTOs
{
    public record CreateUserDTO(
        string FirstName,
        string LastName,
        string Email,
        string Password
    );
}
