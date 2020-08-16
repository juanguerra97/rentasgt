using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rentasgt.Domain.Entities;

namespace rentasgt.Infrastructure.Persistence.Configurations
{
    public class RentCostConfiguration : IEntityTypeConfiguration<RentCost>
    {
        public void Configure(EntityTypeBuilder<RentCost> builder)
        {

            builder.HasKey(c => new { c.ProductId, c.Duration });

            builder.Property(c => c.Duration)
                .IsRequired();

            builder.Property(c => c.Cost)
                .IsRequired();

            //builder.HasOne(c => c.Product)
            //    .WithMany(p => p.Costs)
            //    .HasForeignKey(c => c.ProductId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
