using System;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.CouponServiceTests;

public class FakeClock : IClock
{
    public DateTime UtcNow { get; set; } = DateTime.UtcNow;
}
