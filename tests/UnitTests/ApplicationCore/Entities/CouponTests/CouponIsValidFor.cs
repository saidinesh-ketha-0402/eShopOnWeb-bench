using System;
using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.CouponTests;

public class CouponIsValidFor
{
    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsFalseWhenInactive()
    {
        var coupon = new CouponBuilder().Inactive().Build();

        Assert.False(coupon.IsValidFor(100m, DateTime.UtcNow));
    }

    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsFalseWhenExpired()
    {
        var coupon = new CouponBuilder().ExpiringAt(DateTime.UtcNow.AddMinutes(-1)).Build();

        Assert.False(coupon.IsValidFor(100m, DateTime.UtcNow));
    }

    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsFalseWhenMaxRedemptionsReached()
    {
        var coupon = new CouponBuilder().WithMaxRedemptions(1, 1).Build();

        Assert.False(coupon.IsValidFor(100m, DateTime.UtcNow));
    }

    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsFalseWhenSubtotalBelowMinimum()
    {
        var coupon = new CouponBuilder().WithMinimum(200m).Build();

        Assert.False(coupon.IsValidFor(100m, DateTime.UtcNow));
    }

    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsTrueWhenAllRulesPass()
    {
        var coupon = new CouponBuilder().WithMinimum(20m).Build();

        Assert.True(coupon.IsValidFor(25m, DateTime.UtcNow));
    }
}
