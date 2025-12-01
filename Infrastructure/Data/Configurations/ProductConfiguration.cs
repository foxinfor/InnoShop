using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(p => p.Price)
                   .IsRequired();

            builder.Property(p => p.IsAvailable)
                   .IsRequired();

            builder.Property(p => p.OwnerUserId)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.Property(p => p.UpdatedAt);

            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.Price);
            builder.HasIndex(p => p.OwnerUserId);
            builder.HasIndex(p => p.CreatedAt);
        }
    }
}
