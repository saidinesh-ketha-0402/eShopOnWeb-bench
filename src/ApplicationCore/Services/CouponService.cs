using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class CouponService : ICouponService
{
    private readonly IRepository<Coupon> _couponRepository;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IClock _clock;

    public CouponService(
        IRepository<Coupon> couponRepository,
        IRepository<Basket> basketRepository,
        IClock clock)
    {
        _couponRepository = couponRepository;
        _basketRepository = basketRepository;
        _clock = clock;
    }

    public async Task<Coupon> ValidateCouponAsync(string code, decimal basketSubtotal)
    {
        var spec = new CouponByCodeSpecification(code);
        var coupon = await _couponRepository.FirstOrDefaultAsync(spec);

        Guard.Against.InvalidCoupon(coupon is null, $"Coupon '{code}' is invalid.");
        Guard.Against.InvalidCoupon(!coupon!.IsValidFor(basketSubtotal, _clock.UtcNow), $"Coupon '{coupon.Code}' is invalid.");
        return coupon;
    }

    public async Task ApplyCouponToBasketAsync(int basketId, string code)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        Guard.Against.Null(basket, nameof(basket));

        var basketSubtotal = basket!.Items.Sum(i => i.UnitPrice * i.Quantity);
        var coupon = await ValidateCouponAsync(code, basketSubtotal);
        basket.ApplyCoupon(coupon.Code);
        await _basketRepository.UpdateAsync(basket);
    }

    public async Task RemoveCouponFromBasketAsync(int basketId)
    {
        var basket = await _basketRepository.GetByIdAsync(basketId);
        Guard.Against.Null(basket, nameof(basket));
        basket!.RemoveCoupon();
        await _basketRepository.UpdateAsync(basket);
    }

    public async Task<decimal> CalculateBasketDiscountAsync(int basketId)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        Guard.Against.Null(basket, nameof(basket));

        if (string.IsNullOrWhiteSpace(basket!.CouponCode))
        {
            return 0;
        }

        var basketSubtotal = basket.Items.Sum(i => i.UnitPrice * i.Quantity);
        var coupon = await ValidateCouponAsync(basket.CouponCode, basketSubtotal);
        return coupon.CalculateDiscount(basketSubtotal);
    }
}
