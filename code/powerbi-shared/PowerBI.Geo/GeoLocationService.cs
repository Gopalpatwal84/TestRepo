using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PowerBI.Entities;
using Acom.IO;
using PowerBI.Geo.Models;
using Serilog;

namespace PowerBI.Geo
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IRepository<Country> _countryRepository;
        private const string OrionApiUrl = "https://inference.location.live.net/inferenceservice/v21/Pox/LookupGeoLocationByIPAddress";

        public GeoLocationService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country> GetCountryOrDefault(string ip)
        {
            var defaultCountry = _countryRepository.Get("UnitedStates");

            if (string.IsNullOrEmpty(ip) || ip.Equals("undefined", StringComparison.InvariantCultureIgnoreCase))
            {
                return defaultCountry;
            }

            try
            {
                string responseXml;
                using (var client = new HttpClient())
                {
                    var requestXml = $"<LookupGeoLocationByIpAddress xmlns=\"http://inference.location.live.com\"><IPAddress>{ip}</IPAddress></LookupGeoLocationByIpAddress>";
                    var httpContent = new StringContent(requestXml, Encoding.UTF8, "application/xml");
                    var response = await client.PostAsync(OrionApiUrl, httpContent);
                    Log.Information("Orion api call successful for {ip}", ip);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Log.Error("Orion Geo Api returned an unexpected status code {ip}. Status returned {StatusCode}", ip, response.StatusCode);
                        return defaultCountry;
                    }

                    responseXml = await response.Content.ReadAsStringAsync();
                }

                Log.Information("Fetched response xml for {ip}- {responseXml}", ip, responseXml);

                var serializer = new XmlSerializer(typeof(LookupGeoLocationByIpAddressResponse));
                var locationResponse = (LookupGeoLocationByIpAddressResponse)serializer.Deserialize(new StringReader(responseXml));

                if (locationResponse?.LookupGeoLocationByIpAddressResult == null)
                {
                    Log.Error("Could not resolve location for {ip}", ip);
                    return defaultCountry;
                }

                var geoResponse = locationResponse.LookupGeoLocationByIpAddressResult;

                var result = (_countryRepository.Get()).FirstOrDefault(x => x.Code.Equals(geoResponse.Country, StringComparison.InvariantCultureIgnoreCase)) ?? defaultCountry;
                Log.Information("Fetched country information successfully for {ip}- {Slug}", ip, result.Slug);

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Orion Geo Api error for {ip} - {Message}", ip, ex.Message);
                if(ex.InnerException != null)
                {
                    Log.Error(ex.InnerException, "Orion Geo Api inner eexception for {ip}", ip);
                }

                return defaultCountry;
            }
        }
    }
}
