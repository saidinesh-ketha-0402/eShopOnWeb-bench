using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.CouponServiceTests;

public class RemoveCouponFromBasket
{
    [Fact]
    [Trait("Phase", "1")]
    public async Task ClearsCouponCode()
    {
        var basketRepository = Substitute.For<IRepository<Basket>>();
        var couponRepository = Substitute.For<IRepository<Coupon>>();
        var clock = new FakeClock();
        var basket = new Basket("buyer-1");
        basket.ApplyCoupon("WELCOME10");
        basketRepository.GetByIdAsync(1, default).Returns(basket);
        var service = new CouponService(couponRepository, basketRepository, clock);

        await service.RemoveCouponFromBasketAsync(1);

        Assert.Null(basket.CouponCode);
        await basketRepository.Received().UpdateAsync(basket, default);
    }
}
