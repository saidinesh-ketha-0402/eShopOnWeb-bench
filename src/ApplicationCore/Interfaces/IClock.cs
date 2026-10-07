using System;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IClock
{
    DateTime UtcNow { get; }
}
