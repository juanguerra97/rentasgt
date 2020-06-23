using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Users)
                .WithOne(ur => ur.Room)
                .HasForeignKey(ur => ur.RoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
