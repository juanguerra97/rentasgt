using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class TextMessageConfiguration : IEntityTypeConfiguration<TextMessage>
    {
        public void Configure(EntityTypeBuilder<TextMessage> builder)
        {

            builder.Property(m => m.TextContent)
                .HasMaxLength(512)
                .IsRequired();

        }
    }
}
