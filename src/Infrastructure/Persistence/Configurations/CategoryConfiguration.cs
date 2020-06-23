using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(512)
                .IsRequired();

            builder.HasMany(c => c.Products)
                .WithOne(pc => pc.Category)
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
