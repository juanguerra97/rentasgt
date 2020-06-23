using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(p => p.OtherNames)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(512)
                .IsRequired();

            builder.HasOne(p => p.Owner)
                .WithMany(u => u.Products)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Pictures)
                .WithOne(pp => pp.Product)
                .HasForeignKey(pp => pp.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Costs)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Categories)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.RentRequests)
                .WithOne(rq => rq.Product)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(p => p.Location);

        }
    }
}
