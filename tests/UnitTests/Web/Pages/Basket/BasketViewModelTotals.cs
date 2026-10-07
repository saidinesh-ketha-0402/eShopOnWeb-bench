using Microsoft.eShopWeb.Web.Pages.Basket;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.Web.Pages.Basket;

public class BasketViewModelTotals
{
    [Fact]
    [Trait("Phase", "2")]
    public void TotalUsesSubtotalMinusDiscount()
    {
        var basket = new BasketViewModel();
        basket.Items.Add(new BasketItemViewModel { Quantity = 2, UnitPrice = 10m });
        basket.DiscountAmount = 5m;

        Assert.Equal(basket.Subtotal() - basket.DiscountAmount, basket.Total());
    }
}
