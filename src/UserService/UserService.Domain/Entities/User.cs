using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public string PasswordHash { get; private set; }
        public string? PasswordResetToken { get; private set; }
        public bool IsActivate { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string? ConfirmationToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        private static readonly PasswordHasher<User> _hasher = new();

        public User() { }

        public User(string firstName, string lastName, string email, string password)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = "User";
            PasswordHash = _hasher.HashPassword(this,password);
            IsActivate = false;
            EmailConfirmed = false;
            ConfirmationToken = Guid.NewGuid().ToString();
            PasswordResetToken = Guid.NewGuid().ToString();
        }

        public User(Guid id, string firstName, string lastName, string email, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = "User";
            PasswordHash = password;
            IsActivate = false;
            EmailConfirmed = false;
        }


        public void Activate() => IsActivate = true;
        public void Deactivate() => IsActivate = false;


        public void SetConfirmationToken(string token, DateTime expiry)
        {
            ConfirmationToken = token;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            ConfirmationToken = null;
        }

        public void SetPassword(string passwordHash)
        {
            PasswordHash = _hasher.HashPassword(this, passwordHash);
        }

        public void SetRefreshToken(string token, DateTime expiry)
        {
            RefreshToken = token;
            RefreshTokenExpiryTime = expiry;
        }

        public void ClearRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
        }

        public void UpdateDetails(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public IList<string> GetRoles()
        {
            return new List<string> { Role };
        }

        public bool CheckPassword(string password)
        {
            var result = _hasher.VerifyHashedPassword(this, PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
        }
    }
}
