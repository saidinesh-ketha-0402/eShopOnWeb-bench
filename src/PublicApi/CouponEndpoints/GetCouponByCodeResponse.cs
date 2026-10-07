using System;

namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class GetCouponByCodeResponse : BaseResponse
{
    public GetCouponByCodeResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetCouponByCodeResponse()
    {
    }

    public CouponDto Coupon { get; set; }
}
