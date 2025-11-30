namespace UserService.Application.DTOs
{
    public class RegistrationResponseDTO
    {
        public bool IsRegistered { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
