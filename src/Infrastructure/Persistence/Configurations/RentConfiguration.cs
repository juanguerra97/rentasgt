using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RentConfiguration : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.HasKey(r => r.RequestId);

            builder.Property(r => r.StartDate)
                .IsRequired(false);

            builder.Property(r => r.EndDate)
                .IsRequired(false);

            builder.Property(r => r.TotalCost)
                .IsRequired(false);

            builder.HasOne(r => r.Request)
                .WithOne(rq => rq.Rent)
                .HasForeignKey((Rent r) => r.RequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.ChatRoom)
                .WithMany()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
