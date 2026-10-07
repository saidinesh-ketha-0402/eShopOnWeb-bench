using System;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;

namespace Microsoft.eShopWeb.UnitTests.Builders;

public class CouponBuilder
{
    private string _code = "WELCOME10";
    private DiscountType _discountType = DiscountType.Percentage;
    private decimal _value = 10m;
    private decimal _minimumBasketTotal;
    private DateTime? _expiresUtc;
    private int? _maxRedemptions;
    private int _timesRedeemed;
    private bool _isActive = true;

    public CouponBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public CouponBuilder Percentage(decimal value)
    {
        _discountType = DiscountType.Percentage;
        _value = value;
        return this;
    }

    public CouponBuilder Fixed(decimal value)
    {
        _discountType = DiscountType.FixedAmount;
        _value = value;
        return this;
    }

    public CouponBuilder WithMinimum(decimal minimumBasketTotal)
    {
        _minimumBasketTotal = minimumBasketTotal;
        return this;
    }

    public CouponBuilder ExpiringAt(DateTime expiresUtc)
    {
        _expiresUtc = expiresUtc;
        return this;
    }

    public CouponBuilder WithMaxRedemptions(int maxRedemptions, int timesRedeemed = 0)
    {
        _maxRedemptions = maxRedemptions;
        _timesRedeemed = timesRedeemed;
        return this;
    }

    public CouponBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public Coupon Build()
    {
        return new Coupon(
            _code,
            _discountType,
            _value,
            _minimumBasketTotal,
            _expiresUtc,
            _maxRedemptions,
            _timesRedeemed,
            _isActive);
    }
}
