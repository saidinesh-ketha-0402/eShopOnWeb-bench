using System.Threading.Tasks;
using Microsoft.eShopWeb;
using Microsoft.eShopWeb.PublicApi.CouponEndpoints;
using Xunit;

namespace PublicApiIntegrationTests.CouponEndpoints;

public class ListCouponsEndpointTest
{
    [Fact]
    [Trait("Phase", "2")]
    public async Task ReturnsSeededCoupons()
    {
        var client = ProgramTest.NewClient;
        var response = await client.GetAsync("/api/coupons");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var model = content.FromJson<ListCouponsResponse>();

        Assert.Contains(model!.Coupons, c => c.Code == "WELCOME10");
    }
}
