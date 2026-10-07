using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.BasketTests;

public class BasketApplyCoupon
{
    [Fact]
    [Trait("Phase", "1")]
    public void AppliesAndNormalizesCouponCode()
    {
        var basket = new Basket("buyer-1");

        basket.ApplyCoupon(" welcome10 ");

        Assert.Equal("WELCOME10", basket.CouponCode);
    }

    [Fact]
    [Trait("Phase", "1")]
    public void RemoveCouponClearsCouponCode()
    {
        var basket = new Basket("buyer-1");
        basket.ApplyCoupon("SAVE5");

        basket.RemoveCoupon();

        Assert.Null(basket.CouponCode);
    }
}
