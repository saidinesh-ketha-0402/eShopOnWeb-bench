using System;

namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class CreateCouponResponse : BaseResponse
{
    public CreateCouponResponse(Guid correlationId) : base(correlationId)
    {
    }

    public CreateCouponResponse()
    {
    }

    public CouponDto Coupon { get; set; }
}
