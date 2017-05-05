using System;
using System.Net;
using System.Net.Http;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace PowerBI.Support.TransientErrorDetectionStrategies
{
    public class MetisTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            return ex is HttpRequestException || ex is WebException;
        }
    }
}