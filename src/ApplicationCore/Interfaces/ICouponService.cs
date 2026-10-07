using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface ICouponService
{
    Task<Coupon> ValidateCouponAsync(string code, decimal basketSubtotal);
    Task ApplyCouponToBasketAsync(int basketId, string code);
    Task RemoveCouponFromBasketAsync(int basketId);
    Task<decimal> CalculateBasketDiscountAsync(int basketId);
}
