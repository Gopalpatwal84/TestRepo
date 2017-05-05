using System.Threading.Tasks;
using PowerBI.Entities;

namespace PowerBI.Geo
{
    public interface IGeoLocationService
    {
        Task<Country> GetCountryOrDefault(string ip);
    }
}
