namespace PowerBI.KuduServices.TransientFaultHandling
{
    using System;
    using System.Net;
    using System.Net.Http;

    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    public class KuduTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            var isTransient = false;
            var httpException = ex as HttpRequestException;
            if (httpException != null)
            {
                var webException = httpException.InnerException as WebException;
                isTransient = webException != null || httpException.Message.Contains("502 (Bad Gateway)");
            }

            return isTransient;
        }
    }
}
