using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ProductPictureConfiguration : IEntityTypeConfiguration<ProductPicture>
    {
        public void Configure(EntityTypeBuilder<ProductPicture> builder)
        {

            builder.HasKey(p => new { p.ProductId, p.PictureId });

            builder.HasOne(p => p.Picture)
                .WithMany()
                .HasForeignKey(p => p.PictureId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(prod => prod.Pictures)
                .HasForeignKey(p => p.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
