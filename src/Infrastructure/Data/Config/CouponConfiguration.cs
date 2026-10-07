using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;

namespace Microsoft.eShopWeb.Infrastructure.Data.Config;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.Property(c => c.Value)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.MinimumBasketTotal)
            .HasColumnType("decimal(18,2)");
    }
}
