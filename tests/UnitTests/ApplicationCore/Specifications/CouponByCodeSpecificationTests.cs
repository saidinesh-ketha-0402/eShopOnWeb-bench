using System.Collections.Generic;
using System.Linq;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Specifications;

public class CouponByCodeSpecificationTests
{
    [Fact]
    [Trait("Phase", "1")]
    public void MatchesCouponByCodeCaseInsensitively()
    {
        var spec = new CouponByCodeSpecification("welcome10");

        var result = spec.Evaluate(GetCoupons()).ToList();

        Assert.Single(result);
        Assert.Equal("WELCOME10", result[0].Code);
    }

    private static List<Coupon> GetCoupons()
    {
        return new List<Coupon>
        {
            new("WELCOME10", DiscountType.Percentage, 10m),
            new("SAVE5", DiscountType.FixedAmount, 5m)
        };
    }
}
