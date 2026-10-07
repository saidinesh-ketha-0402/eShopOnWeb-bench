using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class CouponRedemptionLimitReachedException : Exception
{
    public CouponRedemptionLimitReachedException(string code)
        : base($"Coupon '{code}' has reached its redemption limit.")
    {
    }
}
