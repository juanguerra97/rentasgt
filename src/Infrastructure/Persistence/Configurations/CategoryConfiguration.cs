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
                .HasMaxLength(Category.MAX_NAME_LENGTH)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(Category.MAX_DESCRIPTION_LENGTH)
                .IsRequired();

            builder.HasMany(c => c.Products)
                .WithOne(pc => pc.Category)
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
