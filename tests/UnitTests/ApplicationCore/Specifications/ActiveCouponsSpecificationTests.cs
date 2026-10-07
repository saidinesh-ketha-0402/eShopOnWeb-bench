using System.Collections.Generic;
using System.Linq;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Specifications;

public class ActiveCouponsSpecificationTests
{
    [Fact]
    [Trait("Phase", "1")]
    public void ReturnsOnlyActiveCouponsOrderedByCode()
    {
        var spec = new ActiveCouponsSpecification();

        var result = spec.Evaluate(GetCoupons()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("SAVE5", result[0].Code);
        Assert.Equal("WELCOME10", result[1].Code);
    }

    private static List<Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate.Coupon> GetCoupons()
    {
        return new List<Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate.Coupon>
        {
            new CouponBuilder().WithCode("WELCOME10").Build(),
            new CouponBuilder().WithCode("EXPIRED20").Inactive().Build(),
            new CouponBuilder().WithCode("SAVE5").Build()
        };
    }
}
