using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.eShopWeb.PublicApi.CouponEndpoints;
using Xunit;

namespace PublicApiIntegrationTests.CouponEndpoints;

public class CreateCouponEndpointTest
{
    [Fact]
    [Trait("Phase", "2")]
    public async Task AdminCanCreateCoupon()
    {
        var client = ProgramTest.NewClient;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiTokenHelper.GetAdminUserToken());
        var request = new CreateCouponRequest
        {
            Code = "P2NEW",
            DiscountType = 1,
            Value = 10m
        };

        var response = await client.PostAsync("/api/coupons",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    [Trait("Phase", "2")]
    public async Task NonAdminCannotCreateCoupon()
    {
        var client = ProgramTest.NewClient;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiTokenHelper.GetNormalUserToken());
        var request = new CreateCouponRequest
        {
            Code = "P2NOADMIN",
            DiscountType = 1,
            Value = 10m
        };

        var response = await client.PostAsync("/api/coupons",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden);
    }

    [Fact]
    [Trait("Phase", "2")]
    public async Task DuplicateCodeReturnsConflict()
    {
        var client = ProgramTest.NewClient;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiTokenHelper.GetAdminUserToken());
        var request = new CreateCouponRequest
        {
            Code = "WELCOME10",
            DiscountType = 1,
            Value = 10m
        };

        var response = await client.PostAsync("/api/coupons",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
