using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public sealed class CouponByCodeSpecification : Specification<Coupon>
{
    public CouponByCodeSpecification(string code)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        Query
            .Where(c => c.Code.ToUpper() == normalizedCode)
            .Take(1);
    }
}
