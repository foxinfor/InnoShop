using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(p => p.Id);

            modelBuilder.Entity<User>().HasData(
                new User(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Test", "User1", "testuser1@example.com"),
                new User(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Demo", "User2", "testuser2@example.com")
            );
        }
    }
}
