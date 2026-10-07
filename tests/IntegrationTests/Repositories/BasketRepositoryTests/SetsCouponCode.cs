using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.Infrastructure.Data;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.BasketRepositoryTests;

public class SetsCouponCode
{
    [Fact]
    [Trait("Phase", "1")]
    public async Task PersistsCouponCode()
    {
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase($"Catalog-{Guid.NewGuid()}")
            .Options;
        await using var catalogContext = new CatalogContext(options);
        var repository = new EfRepository<Basket>(catalogContext);
        var basket = new Basket("buyer-1");
        basket.ApplyCoupon(" welcome10 ");

        await repository.AddAsync(basket);
        await catalogContext.SaveChangesAsync();
        var persisted = await repository.GetByIdAsync(basket.Id);

        Assert.NotNull(persisted);
        Assert.Equal("WELCOME10", persisted!.CouponCode);
    }
}
