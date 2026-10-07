using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class ListCouponsResponse : BaseResponse
{
    public ListCouponsResponse(Guid correlationId) : base(correlationId)
    {
    }

    public ListCouponsResponse()
    {
    }

    public List<CouponDto> Coupons { get; set; } = new List<CouponDto>();
}
