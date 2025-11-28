using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedNever();

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(500);

            builder.Property(u => u.IsActivate)
                   .IsRequired();

            builder.Property(u => u.EmailConfirmed)
                   .IsRequired();

            builder.Property(u => u.RefreshToken)
                   .HasMaxLength(500);

            builder.Property(u => u.RefreshTokenExpiryTime);

            builder.HasData(
                new User(Guid.Parse("11111111-1111-1111-1111-111111111111"), "User1", "User1", "testuser1@example.com","12345"),
                new User(Guid.Parse("22222222-2222-2222-2222-222222222222"), "User2", "User2", "testuser2@example.com","12345")
            );
        }
    }
}
