using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Microsoft.eShopWeb.FunctionalTests.Web.Pages.Basket;

[Collection("Sequential")]
public class ApplyCouponTests : IClassFixture<TestApplication>
{
    public ApplyCouponTests(TestApplication factory)
    {
        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true
        });
    }

    public HttpClient Client { get; }

    [Fact]
    [Trait("Phase", "2")]
    public async Task ApplyValidCouponShowsDiscount()
    {
        var home = await Client.GetAsync("/");
        var homeHtml = await home.Content.ReadAsStringAsync();
        var addItemForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("id", "2"),
            new("name", "shirt"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(homeHtml))
        });
        var addItemResponse = await Client.PostAsync("/basket/index", addItemForm);
        var basketHtml = await addItemResponse.Content.ReadAsStringAsync();
        var applyCouponForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("code", "WELCOME10"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(basketHtml))
        });

        var applyResponse = await Client.PostAsync("/Basket?handler=ApplyCoupon", applyCouponForm);
        var applyHtml = await applyResponse.Content.ReadAsStringAsync();

        Assert.Contains("WELCOME10", applyHtml);
        Assert.Contains("Discount", applyHtml);
    }

    [Fact]
    [Trait("Phase", "2")]
    public async Task ApplyInvalidCouponShowsValidationMessage()
    {
        var home = await Client.GetAsync("/");
        var homeHtml = await home.Content.ReadAsStringAsync();
        var addItemForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("id", "2"),
            new("name", "shirt"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(homeHtml))
        });
        var addItemResponse = await Client.PostAsync("/basket/index", addItemForm);
        var basketHtml = await addItemResponse.Content.ReadAsStringAsync();
        var applyCouponForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("code", "NOPE"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(basketHtml))
        });

        var applyResponse = await Client.PostAsync("/Basket?handler=ApplyCoupon", applyCouponForm);
        var applyHtml = await applyResponse.Content.ReadAsStringAsync();

        Assert.Contains("Invalid promo code", applyHtml);
    }

    [Fact]
    [Trait("Phase", "2")]
    public async Task RemoveCouponHidesCouponCode()
    {
        var home = await Client.GetAsync("/");
        var homeHtml = await home.Content.ReadAsStringAsync();
        var addItemForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("id", "2"),
            new("name", "shirt"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(homeHtml))
        });
        var addItemResponse = await Client.PostAsync("/basket/index", addItemForm);
        var basketHtml = await addItemResponse.Content.ReadAsStringAsync();
        var applyCouponForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("code", "WELCOME10"),
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(basketHtml))
        });
        var applied = await Client.PostAsync("/Basket?handler=ApplyCoupon", applyCouponForm);
        var appliedHtml = await applied.Content.ReadAsStringAsync();
        Assert.Contains("WELCOME10", appliedHtml);

        var removeCouponForm = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new(WebPageHelpers.TokenTag, WebPageHelpers.GetRequestVerificationToken(appliedHtml))
        });

        var removeResponse = await Client.PostAsync("/Basket?handler=RemoveCoupon", removeCouponForm);
        var removeHtml = await removeResponse.Content.ReadAsStringAsync();

        Assert.DoesNotContain("WELCOME10", removeHtml);
    }
}
