using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace PublicApiIntegrationTests.CouponEndpoints;

public class DeleteCouponEndpointTest
{
    [Fact]
    [Trait("Phase", "2")]
    public async Task AdminDeleteReturnsOkAndCouponIsGone()
    {
        var client = ProgramTest.NewClient;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiTokenHelper.GetAdminUserToken());

        var deleteResponse = await client.DeleteAsync("/api/coupons/WELCOME10");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var getResponse = await client.GetAsync("/api/coupons/WELCOME10");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
