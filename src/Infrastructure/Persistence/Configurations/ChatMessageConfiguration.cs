using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {

            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.MessageType)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired();

            builder.Property(m => m.SentDate)
                .IsRequired(false);

            builder.Property(m => m.Content)
                .HasMaxLength(ChatMessage.MAX_MESSAGE_LENGTH)
                .IsRequired();

            builder.HasOne(m => m.Room)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.RoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Sender)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
