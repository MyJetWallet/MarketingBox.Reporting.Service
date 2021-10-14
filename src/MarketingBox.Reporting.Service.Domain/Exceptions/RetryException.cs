using System;

namespace MarketingBox.Reporting.Service.Domain.Exceptions
{
    public class RetryException : Exception
    {
        public RetryException(string message) : base (message)
        {
        }
    }
}
