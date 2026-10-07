using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.CouponTests;

public class CouponCalculateDiscount
{
    [Fact]
    [Trait("Phase", "1")]
    public void PercentageDiscountRoundsToTwoDecimals()
    {
        var coupon = new CouponBuilder().Percentage(12.5m).Build();

        var discount = coupon.CalculateDiscount(99.99m);

        Assert.Equal(12.50m, discount);
    }

    [Fact]
    [Trait("Phase", "1")]
    public void FixedDiscountIsCappedAtSubtotal()
    {
        var coupon = new CouponBuilder().Fixed(10m).Build();

        var discount = coupon.CalculateDiscount(6m);

        Assert.Equal(6m, discount);
    }

    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsZeroForZeroSubtotal()
    {
        var coupon = new CouponBuilder().Fixed(5m).Build();

        var discount = coupon.CalculateDiscount(0m);

        Assert.Equal(0m, discount);
    }
}
