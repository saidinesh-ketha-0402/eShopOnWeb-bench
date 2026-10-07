using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions;

public class InvalidCouponException : Exception
{
    public InvalidCouponException(string message) : base(message)
    {
    }
}
