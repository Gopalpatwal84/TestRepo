using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PowerBI.Geo.Models
{
    public class LookupGeoLocationByIpAddressResult
    {
        public string CountryConfidence;

        public string Country;

        public string State;

        public string TimeZone;

        public string PostalCode;
    }

    [XmlRoot(ElementName = "LookupGeoLocationByIpAddressResponse", Namespace = "http://inference.location.live.com")]

    public class LookupGeoLocationByIpAddressResponse
    {
        public LookupGeoLocationByIpAddressResult LookupGeoLocationByIpAddressResult { get; set; }
    }
}
