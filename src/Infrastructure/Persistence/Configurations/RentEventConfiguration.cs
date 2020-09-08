using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RentEventConfiguration : IEntityTypeConfiguration<RentEvent>
    {
        public void Configure(EntityTypeBuilder<RentEvent> builder)
        {
            builder.Property(re => re.Id)
                .ValueGeneratedOnAdd();

            builder.Property(re => re.EventType)
                .IsRequired();

            builder.Property(re => re.EventDate)
                .IsRequired();

            builder.Property(re => re.Message)
                .HasMaxLength(RentEvent.MAX_MESSAGE_LENGTH)
                .IsRequired(false);

            builder.HasOne(re => re.Rent)
                .WithMany(r => r.Events)
                .HasForeignKey(re => re.RentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
