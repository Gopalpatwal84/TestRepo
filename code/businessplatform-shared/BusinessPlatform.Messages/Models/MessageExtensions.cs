namespace BusinessPlatform.Messages.Models
{
    using System;

    public static class MessageExtensions
    {
        public static T DiagnosticInfo<T>(this IDiagnosticInfo obj, Func<IDiagnosticInfo, T> path)
        {
            return DiagnosticInfo(obj, path, default(T));
        }

        public static T DiagnosticInfo<T>(this IDiagnosticInfo obj, Func<IDiagnosticInfo, T> path, T defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return path.Invoke(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool IsFromCacheRefreshApi(this IDiagnosticInfo obj)
        {
            return obj.DiagnosticInfo(x => x.DiagnosticsInfo.Source == "CacheRefreshAPI");
        }
    }
}