namespace Microsoft.eShopWeb.Web.Pages.Basket;

public class BasketViewModel
{
    public int Id { get; set; }
    public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
    public string? BuyerId { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }

    public decimal Subtotal()
    {
        return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }

    public decimal Total()
    {
        return Subtotal();
    }
}
