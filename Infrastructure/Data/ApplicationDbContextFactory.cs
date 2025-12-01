using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=product_db;Port=5432;Database=innoshop_products;Username=postgres;Password=postgres",
                b => b.MigrationsAssembly("Infrastructure")
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
