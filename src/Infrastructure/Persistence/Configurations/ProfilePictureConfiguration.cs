using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ProfilePictureConfiguration : IEntityTypeConfiguration<ProfilePicture>
    {
        public void Configure(EntityTypeBuilder<ProfilePicture> builder)
        {

            builder.HasKey(p => p.UserId);

            builder.HasOne(p => p.User)
                .WithOne(u => u.ProfilePicture)
                .HasForeignKey((ProfilePicture p) => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Picture)
                .WithMany()
                .HasForeignKey(p => p.PictureId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
