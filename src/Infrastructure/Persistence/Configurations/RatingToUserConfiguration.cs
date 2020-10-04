using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RatingToUserConfiguration : IEntityTypeConfiguration<RatingToUser>
    {
        public void Configure(EntityTypeBuilder<RatingToUser> builder)
        {
            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Status)
                .IsRequired();

            builder.Property(r => r.RatingValue)
                .IsRequired(false);

            builder.Property(r => r.Comment)
                .HasMaxLength(RatingToUser.MAX_COMMENT_LENGTH)
                .IsRequired(false);

            builder.Property(r => r.RatingDate)
                .IsRequired(false);

            builder.HasOne(r => r.FromUser)
                .WithMany()
                .HasForeignKey(r => r.FromUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(r => r.ToUser)
                .WithMany()
                .HasForeignKey(r => r.ToUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
