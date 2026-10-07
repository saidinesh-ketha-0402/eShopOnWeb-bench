using System;

namespace Microsoft.eShopWeb.PublicApi.CouponEndpoints;

public class DeleteCouponResponse : BaseResponse
{
    public DeleteCouponResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteCouponResponse()
    {
    }
}
