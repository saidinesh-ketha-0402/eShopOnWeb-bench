using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Microsoft.eShopWeb.FunctionalTests.Web.Pages.Basket;

[Collection("Sequential")]
public class CheckoutWithCouponTests : IClassFixture<TestApplication>
{
    public CheckoutWithCouponTests(TestApplication factory)
    {
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });
    }

    public HttpClient Client { get; }

    [Fact]
    [Trait("Phase", "2")]
    public async Task CheckoutDisplaysDiscountedTotalInMyOrders()
    {
        var home = await Client.GetAsync("/");
        var homeHtml = await home.Content.ReadAsStringAsync();
        var addItemForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("id", "2"),
            new("name", "shirt"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(homeHtml))
        });
        await Client.PostAsync("/basket/index", addItemForm);

        var basketPage = await Client.GetAsync("/basket");
        var basketHtml = await basketPage.Content.ReadAsStringAsync();
        var applyCouponForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("code", "WELCOME10"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(basketHtml))
        });
        await Client.PostAsync("/Basket?handler=ApplyCoupon", applyCouponForm);

        var loginResponse = await Client.GetAsync("/Identity/Account/Login");
        var loginForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("email", "demouser@microsoft.com"),
            new("password", "Pass@word1"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(await loginResponse.Content.ReadAsStringAsync()))
        });
        var loginPost = await Client.PostAsync("/Identity/Account/Login?ReturnUrl=%2FBasket%2FCheckout", loginForm);
        var loginHtml = await loginPost.Content.ReadAsStringAsync();

        var checkoutForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("Items[0].Id", "2"),
            new("Items[0].Quantity", "1"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(loginHtml))
        });
        await Client.PostAsync("/basket/checkout", checkoutForm);

        var myOrders = await Client.GetAsync("/Order/MyOrders");
        var myOrdersHtml = await myOrders.Content.ReadAsStringAsync();

        Assert.Contains("7.65", myOrdersHtml);
    }
}
