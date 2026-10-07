using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.CouponTests;

public class CouponConstructor
{
    [Fact]
    [Trait("Phase", "1")]
    public void NormalizesCode()
    {
        var coupon = new Coupon(" welcome10 ", DiscountType.Percentage, 10m);

        Assert.Equal("WELCOME10", coupon.Code);
    }

    [Theory]
    [Trait("Phase", "1")]
    [InlineData("")]
    [InlineData(" ")]
    public void RejectsEmptyCode(string code)
    {
        Assert.ThrowsAny<System.Exception>(() => new Coupon(code, DiscountType.Percentage, 10m));
    }

    [Fact]
    [Trait("Phase", "1")]
    public void RejectsTooLongCode()
    {
        Assert.ThrowsAny<System.Exception>(() => new Coupon("123456789012345678901", DiscountType.Percentage, 10m));
    }

    [Theory]
    [Trait("Phase", "1")]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectsNonPositiveValue(decimal value)
    {
        Assert.ThrowsAny<System.Exception>(() => new Coupon("WELCOME10", DiscountType.Percentage, value));
    }

    [Fact]
    [Trait("Phase", "1")]
    public void RejectsPercentageOver100()
    {
        Assert.ThrowsAny<System.Exception>(() => new Coupon("OVER100", DiscountType.Percentage, 101m));
    }
}
