using BlockedCountries.Models;

namespace BlockedCountries.Services.Interfaces
{
    public interface IIpGeolocationService
    {
        Task<CountryInfo> GetCountryInfoByIpAsync(string ipAddress);
    }
}
