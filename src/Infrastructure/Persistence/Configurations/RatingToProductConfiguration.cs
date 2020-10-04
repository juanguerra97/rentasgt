using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RatingToProductConfiguration : IEntityTypeConfiguration<RatingToProduct>
    {
        public void Configure(EntityTypeBuilder<RatingToProduct> builder)
        {
            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Status)
                .IsRequired();

            builder.Property(r => r.ProductRatingValue)
                .IsRequired(false);

            builder.Property(r => r.OwnerRatingValue)
                .IsRequired(false);

            builder.Property(r => r.Comment)
                .HasMaxLength(RatingToProduct.MAX_COMMENT_LENGTH)
                .IsRequired(false);

            builder.Property(r => r.RatingDate)
                .IsRequired(false);

            builder.HasOne(r => r.FromUser)
                .WithMany()
                .HasForeignKey(r => r.FromUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
