using System;

namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int DiscountType { get; set; }
    public decimal Value { get; set; }
    public decimal MinimumBasketTotal { get; set; }
    public DateTime? ExpiresUtc { get; set; }
    public int? MaxRedemptions { get; set; }
    public int TimesRedeemed { get; set; }
    public bool IsActive { get; set; }
}
