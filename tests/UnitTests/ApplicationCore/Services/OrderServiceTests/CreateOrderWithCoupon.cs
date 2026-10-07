using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.OrderServiceTests;

public class CreateOrderWithCoupon
{
    [Fact]
    [Trait("Phase", "2")]
    public async Task AppliesCouponDiscountToOrderWhenBasketHasValidCoupon()
    {
        var basketRepository = Substitute.For<IRepository<Basket>>();
        var itemRepository = Substitute.For<IRepository<CatalogItem>>();
        var orderRepository = Substitute.For<IRepository<Order>>();
        var uriComposer = Substitute.For<IUriComposer>();
        var basket = new Basket("buyer-1");
        basket.AddItem(1, 100m, 1);
        basket.ApplyCoupon("WELCOME10");
        basketRepository.FirstOrDefaultAsync(Arg.Any<BasketWithItemsSpecification>(), default).Returns(basket);
        itemRepository.ListAsync(Arg.Any<CatalogItemsSpecification>(), default)
            .Returns(new List<CatalogItem> { new CatalogItem(1, 1, "desc", "item", 100m, "pic") });
        uriComposer.ComposePicUri(Arg.Any<string>()).Returns("pic");
        var service = new OrderService(basketRepository, itemRepository, orderRepository, uriComposer);

        await service.CreateOrderAsync(1, new Address("1 st", "city", "state", "country", "12345"));

        await orderRepository.Received().AddAsync(
            Arg.Is<Order>(o => o.CouponCode == "WELCOME10" && o.DiscountAmount > 0),
            default);
    }

    [Fact]
    [Trait("Phase", "2")]
    public async Task IgnoresInvalidCouponWhenCreatingOrder()
    {
        var basketRepository = Substitute.For<IRepository<Basket>>();
        var itemRepository = Substitute.For<IRepository<CatalogItem>>();
        var orderRepository = Substitute.For<IRepository<Order>>();
        var uriComposer = Substitute.For<IUriComposer>();
        var basket = new Basket("buyer-1");
        basket.AddItem(1, 100m, 1);
        basket.ApplyCoupon("EXPIRED20");
        basketRepository.FirstOrDefaultAsync(Arg.Any<BasketWithItemsSpecification>(), default).Returns(basket);
        itemRepository.ListAsync(Arg.Any<CatalogItemsSpecification>(), default)
            .Returns(new List<CatalogItem> { new CatalogItem(1, 1, "desc", "item", 100m, "pic") });
        uriComposer.ComposePicUri(Arg.Any<string>()).Returns("pic");
        var service = new OrderService(basketRepository, itemRepository, orderRepository, uriComposer);

        await service.CreateOrderAsync(1, new Address("1 st", "city", "state", "country", "12345"));

        await orderRepository.Received().AddAsync(Arg.Any<Order>(), default);
        Assert.Null(basket.CouponCode);
    }
}
