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

            builder.HasIndex(cr => new { cr.ProductId, cr.UserId })
                .IsUnique(true);

            builder.HasOne(c => c.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
