using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace PowerBI.Jobs.CommonModules.TransientErrorDetection
{
    public class RetryPolicyFactory
    {
        public static RetryPolicy CreateHttpClientTransientErrorDetectionPolicy()
        {
            var strategy = new ExponentialBackoff(RetryStrategy.DefaultClientRetryCount, RetryStrategy.DefaultMinBackoff, RetryStrategy.DefaultMaxBackoff, RetryStrategy.DefaultClientBackoff);
            return new RetryPolicy<HttpClientTransientErrorDetectionStrategy>(strategy);
        }
    }
}
