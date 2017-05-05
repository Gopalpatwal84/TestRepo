using System;
using System.Data.SqlTypes;

namespace PowerBI.Data
{
    public static class SqlExtensions
    {
        public static DateTime AsSqlDate(this DateTime date)
        {
            if (date < SqlDateTime.MinValue.Value)
                return SqlDateTime.MinValue.Value;
            return date;
        }

        public static DateTime? AsSqlDate(this DateTime? date)
        {
            if (date.GetValueOrDefault() < SqlDateTime.MinValue.Value)
                return null;
            return date;
        }

        public static DateTime? Nullable(this DateTime date)
        {
            return date;
        }
    }
}