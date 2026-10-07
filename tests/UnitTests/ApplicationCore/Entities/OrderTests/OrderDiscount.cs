using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.OrderTests;

public class OrderDiscount
{
    [Fact]
    [Trait("Phase", "2")]
    public void SubtotalMatchesUndiscountedItems()
    {
        var order = new OrderBuilder().WithDefaultValues();

        Assert.Equal(order.Total(), order.Subtotal());
    }

    [Fact]
    [Trait("Phase", "2")]
    public void ApplyDiscountSetsCouponAndAmount()
    {
        var order = new OrderBuilder().WithDefaultValues();

        order.ApplyDiscount("WELCOME10", 2m);

        Assert.Equal("WELCOME10", order.CouponCode);
        Assert.Equal(2m, order.DiscountAmount);
    }

    [Fact]
    [Trait("Phase", "2")]
    public void TotalUsesSubtotalMinusDiscount()
    {
        var order = new OrderBuilder().WithDefaultValues();
        order.ApplyDiscount("WELCOME10", 1m);

        Assert.Equal(order.Subtotal() - order.DiscountAmount, order.Total());
    }

    [Fact]
    [Trait("Phase", "2")]
    public void DiscountCannotExceedSubtotal()
    {
        var order = new OrderBuilder().WithDefaultValues();
        order.ApplyDiscount("SAVE5", decimal.MaxValue);

        Assert.True(order.DiscountAmount <= order.Subtotal());
    }
}
