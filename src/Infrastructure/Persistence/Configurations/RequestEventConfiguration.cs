using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RequestEventConfiguration : IEntityTypeConfiguration<RequestEvent>
    {
        public void Configure(EntityTypeBuilder<RequestEvent> builder)
        {

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.EventType)
                .IsRequired();

            builder.Property(e => e.EventDate)
                .IsRequired();

            builder.Property(e => e.Message)
                .HasMaxLength(128)
                .IsRequired(false);

            builder.HasOne(e => e.RentRequest)
                .WithMany(r => r.Events)
                .HasForeignKey(e => e.RentRequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
