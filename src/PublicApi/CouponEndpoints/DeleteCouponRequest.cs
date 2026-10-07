namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class DeleteCouponRequest : BaseRequest
{
    public string Code { get; set; }
}
