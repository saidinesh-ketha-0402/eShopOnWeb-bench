using System.Net;
using System.Threading.Tasks;
using Microsoft.eShopWeb;
using Microsoft.eShopWeb.PublicApi.CouponEndpoints;
using Xunit;

namespace PublicApiIntegrationTests.CouponEndpoints;

public class GetCouponByCodeEndpointTest
{
    [Fact]
    [Trait("Phase", "2")]
    public async Task ReturnsCouponForKnownCode()
    {
        var client = ProgramTest.NewClient;
        var response = await client.GetAsync("/api/coupons/WELCOME10");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var model = content.FromJson<GetCouponByCodeResponse>();

        Assert.Equal("WELCOME10", model!.Coupon.Code);
    }

    [Fact]
    [Trait("Phase", "2")]
    public async Task ReturnsNotFoundForUnknownCode()
    {
        var client = ProgramTest.NewClient;
        var response = await client.GetAsync("/api/coupons/DOESNOTEXIST");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Contains("DOESNOTEXIST", content);
    }
}
