using System;

namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class CreateCouponRequest : BaseRequest
{
    public string Code { get; set; }
    public int DiscountType { get; set; }
    public decimal Value { get; set; }
    public decimal MinimumBasketTotal { get; set; }
    public DateTime? ExpiresUtc { get; set; }
    public int? MaxRedemptions { get; set; }
}
