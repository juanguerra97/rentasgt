using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class UserChatRoomConfiguration : IEntityTypeConfiguration<UserChatRoom>
    {
        public void Configure(EntityTypeBuilder<UserChatRoom> builder)
        {

            builder.HasKey(ur => new { ur.UserId, ur.RoomId });

            builder.HasOne(ur => ur.Room)
                .WithMany(cr => cr.Users)
                .HasForeignKey(ur => ur.RoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
