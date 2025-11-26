namespace UserService.Application.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActivate { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
