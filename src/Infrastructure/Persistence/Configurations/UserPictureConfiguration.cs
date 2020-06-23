using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    class UserPictureConfiguration : IEntityTypeConfiguration<UserPicture>
    {
        public void Configure(EntityTypeBuilder<UserPicture> builder)
        {

            builder.HasKey(p => p.UserId);

            builder.HasOne(p => p.User)
                .WithOne(u => u.UserPicture)
                .HasForeignKey((UserPicture p) => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Picture)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
