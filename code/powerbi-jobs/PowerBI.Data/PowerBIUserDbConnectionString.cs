namespace PowerBI.Data
{
    using System.Configuration;
    using Acom.Configuration;

    public class PowerBIUserDbConnectionString
    {
        /// <summary>
        /// It casts to a string implicity
        /// AS it casts to a string we get the connection string for a strongly typed class
        /// </summary>
        public static implicit operator string(PowerBIUserDbConnectionString connectionString)
        {
            var name = "PowerBIUserDBConnectionString" + (KnownSlots.IsProduction ? string.Empty : "-Int");
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
