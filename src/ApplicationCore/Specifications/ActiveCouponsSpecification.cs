using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public sealed class ActiveCouponsSpecification : Specification<Coupon>
{
    public ActiveCouponsSpecification()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Code);
    }
}
