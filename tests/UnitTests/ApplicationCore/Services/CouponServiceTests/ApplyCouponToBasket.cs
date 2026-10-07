using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.CouponServiceTests;

public class ApplyCouponToBasket
{
    [Fact]
    [Trait("Phase", "1")]
    public async Task AppliesNormalizedCouponCodeToBasket()
    {
        var basketRepository = Substitute.For<IRepository<Basket>>();
        var couponRepository = Substitute.For<IRepository<Coupon>>();
        var clock = new FakeClock();
        var basket = new Basket("buyer-1");
        basket.AddItem(1, 20m, 2);
        var coupon = new Coupon("WELCOME10", DiscountType.Percentage, 10m);

        basketRepository.FirstOrDefaultAsync(Arg.Any<BasketWithItemsSpecification>(), default).Returns(basket);
        couponRepository.FirstOrDefaultAsync(Arg.Any<CouponByCodeSpecification>(), default).Returns(coupon);
        var service = new CouponService(couponRepository, basketRepository, clock);

        await service.ApplyCouponToBasketAsync(1, " welcome10 ");

        Assert.Equal("WELCOME10", basket.CouponCode);
        await basketRepository.Received().UpdateAsync(basket, default);
    }
}
