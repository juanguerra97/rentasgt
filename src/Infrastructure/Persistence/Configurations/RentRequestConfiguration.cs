using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RentRequestConfiguration : IEntityTypeConfiguration<RentRequest>
    {
        public void Configure(EntityTypeBuilder<RentRequest> builder)
        {

            builder.Property(rq => rq.Id)
                .ValueGeneratedOnAdd();

            builder.Property(rq => rq.Status)
                .IsRequired();

            builder.Property(r => r.RequestDate)
                .IsRequired();

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.EndDate)
                .IsRequired();

            builder.Property(r => r.Place)
                .HasMaxLength(RentRequest.MAX_PLACE_LENGTH)
                .IsRequired(false);

            builder.HasOne(r => r.Product)
                .WithMany(p => p.RentRequests)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Requestor)
                .WithMany(u => u.RentRequests)
                .HasForeignKey(r => r.RequestorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Events)
                .WithOne(e => e.RentRequest)
                .HasForeignKey(e => e.RentRequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Rent)
                .WithOne(re => re.Request)
                .HasForeignKey((Rent r) => r.RequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);   

        }
    }
}
