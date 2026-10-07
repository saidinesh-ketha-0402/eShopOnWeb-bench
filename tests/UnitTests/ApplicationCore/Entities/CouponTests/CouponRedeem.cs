using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.CouponTests;

public class CouponRedeem
{
    [Fact]
    [Trait("Phase", "1")]
    public void IncrementsTimesRedeemed()
    {
        var coupon = new CouponBuilder().Build();

        coupon.Redeem();

        Assert.Equal(1, coupon.TimesRedeemed);
    }

    [Fact]
    [Trait("Phase", "1")]
    public void ThrowsWhenRedemptionLimitReached()
    {
        var coupon = new CouponBuilder().WithMaxRedemptions(1, 1).Build();

        Assert.Throws<CouponRedemptionLimitReachedException>(() => coupon.Redeem());
    }
}
