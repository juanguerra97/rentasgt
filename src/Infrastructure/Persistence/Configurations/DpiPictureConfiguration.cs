using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;
namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class DpiPictureConfiguration : IEntityTypeConfiguration<DpiPicture>
    {
        public void Configure(EntityTypeBuilder<DpiPicture> builder)
        {

            builder.HasKey(p => p.UserId);

            builder.HasOne(p => p.User)
                .WithOne(u => u.DpiPicture)
                .HasForeignKey((DpiPicture p) => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Picture)
                .WithMany()
                .HasForeignKey(p => p.PictureId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
