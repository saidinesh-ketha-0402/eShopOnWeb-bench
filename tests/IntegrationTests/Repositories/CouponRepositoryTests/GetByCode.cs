using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.CouponAggregate;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Infrastructure.Data;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.CouponRepositoryTests;

public class GetByCode
{
    [Fact]
    [Trait("Phase", "1")]
    public async Task ReturnsCouponUsingCaseInsensitiveSpec()
    {
        var options = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase($"Catalog-{Guid.NewGuid()}")
            .Options;
        await using var catalogContext = new CatalogContext(options);
        var repository = new EfRepository<Coupon>(catalogContext);
        await repository.AddAsync(new Coupon("WELCOME10", DiscountType.Percentage, 10m));
        await catalogContext.SaveChangesAsync();

        var result = await repository.FirstOrDefaultAsync(new CouponByCodeSpecification("welcome10"));

        Assert.NotNull(result);
        Assert.Equal("WELCOME10", result!.Code);
    }
}
