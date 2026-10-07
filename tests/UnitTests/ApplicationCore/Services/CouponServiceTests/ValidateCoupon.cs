using System;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.CouponServiceTests;

public class ValidateCoupon
{
    private readonly IRepository<Coupon> _couponRepository = Substitute.For<IRepository<Coupon>>();
    private readonly IRepository<Basket> _basketRepository = Substitute.For<IRepository<Basket>>();
    private readonly FakeClock _clock = new() { UtcNow = new DateTime(2026, 01, 01, 0, 0, 0, DateTimeKind.Utc) };

    [Fact]
    [Trait("Phase", "1")]
    public async Task ReturnsCouponWhenValid()
    {
        var coupon = new Coupon("WELCOME10", DiscountType.Percentage, 10m);
        _couponRepository.FirstOrDefaultAsync(Arg.Any<CouponByCodeSpecification>(), default).Returns(coupon);
        var service = new CouponService(_couponRepository, _basketRepository, _clock);

        var result = await service.ValidateCouponAsync("welcome10", 100m);

        Assert.Equal("WELCOME10", result.Code);
    }

    [Fact]
    [Trait("Phase", "1")]
    public async Task ThrowsForUnknownCode()
    {
        _couponRepository.FirstOrDefaultAsync(Arg.Any<CouponByCodeSpecification>(), default).Returns((Coupon?)null);
        var service = new CouponService(_couponRepository, _basketRepository, _clock);

        await Assert.ThrowsAsync<InvalidCouponException>(() => service.ValidateCouponAsync("missing", 100m));
    }
}
