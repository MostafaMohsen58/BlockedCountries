using BlockedCountries.Models;

namespace BlockedCountries.Repositories.interfaces
{
    public interface ICountryBlockingRepository
    {
        Task<bool> BlockCountryAsync(/*string countryCode*/CountryInfo countryInfo);
        Task<bool> UnblockCountryAsync(string countryCode);
        Task<bool> TemporarilyBlockCountryAsync(string countryCode,string countryName, int durationMinutes);
        Task<PaginatedResponse<CountryInfo>> GetBlockedCountriesAsync(int page, int pageSize, string searchTerm = null);
        Task<bool> IsCountryBlockedAsync(string countryCode);
    }
}
