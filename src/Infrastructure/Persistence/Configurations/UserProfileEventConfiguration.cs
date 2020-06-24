using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class UserProfileEventConfiguration : IEntityTypeConfiguration<UserProfileEvent>
    {
        public void Configure(EntityTypeBuilder<UserProfileEvent> builder)
        {

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.EventType)
                .IsRequired();

            builder.Property(e => e.EventDate)
                .IsRequired();

            builder.Property(e => e.Message)
                .HasMaxLength(UserProfileEvent.MAX_MESSAGE_LENGTH)
                .IsRequired(false);

            builder.HasOne(e => e.UserProfile)
                .WithMany(u => u.ProfileEvents)
                .HasForeignKey(e => e.UserProfileId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.UserEvent)
                .WithMany()
                .HasForeignKey(e => e.UserEventId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
