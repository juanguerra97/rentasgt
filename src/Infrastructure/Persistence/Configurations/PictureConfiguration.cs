using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.StorageType)
                .IsRequired();

            builder.Property(p => p.PictureContent)
                .IsRequired();

        }
    }
}
