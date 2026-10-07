using System;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;

public class Coupon : BaseEntity, IAggregateRoot
{
    public string Code { get; private set; }
    public DiscountType DiscountType { get; private set; }
    public decimal Value { get; private set; }
    public decimal MinimumBasketTotal { get; private set; }
    public DateTime? ExpiresUtc { get; private set; }
    public int? MaxRedemptions { get; private set; }
    public int TimesRedeemed { get; private set; }
    public bool IsActive { get; private set; }

    private Coupon()
    {
    }

    public Coupon(
        string code,
        DiscountType discountType,
        decimal value,
        decimal minimumBasketTotal = 0,
        DateTime? expiresUtc = null,
        int? maxRedemptions = null,
        int timesRedeemed = 0,
        bool isActive = true)
    {
        Code = NormalizeCode(code);
        Guard.Against.OutOfRange(Code.Length, nameof(code), 1, 20);
        Guard.Against.NegativeOrZero(value, nameof(value));
        if (discountType == DiscountType.Percentage)
        {
            Guard.Against.OutOfRange(value, nameof(value), 0.01m, 100m);
        }

        DiscountType = discountType;
        Value = value;
        MinimumBasketTotal = minimumBasketTotal;
        ExpiresUtc = expiresUtc;
        MaxRedemptions = maxRedemptions;
        TimesRedeemed = timesRedeemed;
        IsActive = isActive;
    }

    public bool IsValidFor(decimal basketSubtotal, DateTime utcNow)
    {
        if (!IsActive)
        {
            return false;
        }

        if (ExpiresUtc.HasValue && ExpiresUtc.Value <= utcNow)
        {
            return false;
        }

        if (MaxRedemptions.HasValue && TimesRedeemed >= MaxRedemptions.Value)
        {
            return false;
        }

        return basketSubtotal >= MinimumBasketTotal;
    }

    public decimal CalculateDiscount(decimal basketSubtotal)
    {
        if (basketSubtotal <= 0)
        {
            return 0;
        }

        decimal discount = DiscountType switch
        {
            DiscountType.Percentage => Math.Round(basketSubtotal * Value / 100m, 2),
            DiscountType.FixedAmount => Math.Min(Value, basketSubtotal),
            _ => 0
        };

        if (discount < 0)
        {
            return 0;
        }

        return Math.Min(discount, basketSubtotal);
    }

    public void Redeem()
    {
        if (MaxRedemptions.HasValue && TimesRedeemed >= MaxRedemptions.Value)
        {
            throw new CouponRedemptionLimitReachedException(Code);
        }

        TimesRedeemed++;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static string NormalizeCode(string code)
    {
        return Guard.Against.NullOrWhiteSpace(code, nameof(code)).Trim().ToUpperInvariant();
    }
}
