using BlockedCountries.Models;

namespace BlockedCountries.Services.Interfaces
{
    public interface IIpGeolocationService
    {
        Task<IpGeolocationResponse> GetCountryInfoByIpAsync(string ipAddress);
        Task<bool> IsValidCountryCodeAsync(string countryCode);
    }
}
