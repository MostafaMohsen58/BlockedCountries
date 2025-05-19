using BlockedCountries.Models;

namespace BlockedCountries.Repositories.interfaces
{
    public interface ICountryBlockingRepository
    {
        Task<bool> BlockCountryAsync(string countryCode);
        Task<bool> UnblockCountryAsync(string countryCode);
        Task<bool> TemporarilyBlockCountryAsync(string countryCode, int durationMinutes);
        Task<PaginatedResponse<CountryInfo>> GetBlockedCountriesAsync(int page, int pageSize, string searchTerm = null);
        Task<bool> IsCountryBlockedAsync(string countryCode);
    }
}
