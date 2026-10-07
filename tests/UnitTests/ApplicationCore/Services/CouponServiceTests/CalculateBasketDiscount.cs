using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.CouponServiceTests;

public class CalculateBasketDiscount
{
    [Fact]
    [Trait("Phase", "1")]
    public async Task ReturnsZeroWhenBasketHasNoCoupon()
    {
        var basketRepository = Substitute.For<IRepository<Basket>>();
        var couponRepository = Substitute.For<IRepository<Coupon>>();
        var clock = new FakeClock();
        var basket = new Basket("buyer-1");
        basket.AddItem(1, 10m, 1);
        basketRepository.FirstOrDefaultAsync(Arg.Any<BasketWithItemsSpecification>(), default).Returns(basket);
        var service = new CouponService(couponRepository, basketRepository, clock);

        var discount = await service.CalculateBasketDiscountAsync(1);

        Assert.Equal(0m, discount);
    }

    [Fact]
    [Trait("Phase", "1")]
    public async Task ReturnsCalculatedDiscountWhenCouponIsValid()
    {
        var basketRepository = Substitute.For<IRepository<Basket>>();
        var couponRepository = Substitute.For<IRepository<Coupon>>();
        var clock = new FakeClock();
        var basket = new Basket("buyer-1");
        basket.AddItem(1, 100m, 1);
        basket.ApplyCoupon("WELCOME10");
        var coupon = new Coupon("WELCOME10", DiscountType.Percentage, 10m);

        basketRepository.FirstOrDefaultAsync(Arg.Any<BasketWithItemsSpecification>(), default).Returns(basket);
        couponRepository.FirstOrDefaultAsync(Arg.Any<CouponByCodeSpecification>(), default).Returns(coupon);
        var service = new CouponService(couponRepository, basketRepository, clock);

        var discount = await service.CalculateBasketDiscountAsync(1);

        Assert.Equal(10m, discount);
    }
}
