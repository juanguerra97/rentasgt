using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ConflictConfiguration : IEntityTypeConfiguration<Conflict>
    {
        public void Configure(EntityTypeBuilder<Conflict> builder)
        {

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(c => c.RentId)
                .IsUnique();

            builder.Property(c => c.Status)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(Conflict.MAX_DESCRIPTION_LENGTH)
                .IsRequired();

            builder.Property(c => c.ConflictDate)
                .IsRequired();

            builder.HasOne(c => c.Rent)
                .WithOne()
                .HasForeignKey((Conflict c) => c.RentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Moderator)
                .WithMany()
                .HasForeignKey(c => c.ModeratorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Complainant)
                .WithMany()
                .HasForeignKey(c => c.ComplainantId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ConflictRecords)
                .WithOne(cr => cr.Conflict)
                .HasForeignKey(cr => cr.ConflictId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
