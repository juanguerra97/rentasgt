using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class ConflictRecordConfiguration : IEntityTypeConfiguration<ConflictRecord>
    {
        public void Configure(EntityTypeBuilder<ConflictRecord> builder)
        {

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Description)
                .HasMaxLength(ConflictRecord.MAX_DESCRIPTION_LENGTH)
                .IsRequired();

            builder.Property(r => r.RecordDate)
                .IsRequired();

            builder.HasOne(r => r.Conflict)
                .WithMany(c => c.ConflictRecords)
                .HasForeignKey(r => r.ConflictId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
