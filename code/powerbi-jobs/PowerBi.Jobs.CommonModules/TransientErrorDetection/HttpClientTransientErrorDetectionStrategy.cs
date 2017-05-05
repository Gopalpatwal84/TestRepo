using System;
using System.Net.Http;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace PowerBI.Jobs.CommonModules.TransientErrorDetection
{
    public class HttpClientTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            var httpException = ex as HttpRequestException;
            if (httpException != null)
            {
                return true;
            }

            return false;
        }
    }
}
