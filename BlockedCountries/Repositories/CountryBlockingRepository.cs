using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.Repositories
{
    public class CountryBlockingRepository:ICountryBlockingRepository
    {
        private readonly ConcurrentDictionary<string, CountryInfo> _blockedCountries = new();
        //private readonly ILogger<CountryBlockingService> _logger;

        //public CountryBlockingRepository(ILogger<CountryBlockingService> logger)
        //{
        //    _logger = logger;
        //}

        //Add 
        public async Task<bool> BlockCountryAsync(string countryCode)
        {
            var countryInfo = new CountryInfo { CountryCode = countryCode.ToUpper() };
            return _blockedCountries.TryAdd(countryCode.ToUpper(), countryInfo);
        }

        //Remove
        public async Task<bool> UnblockCountryAsync(string countryCode)
        {
            return _blockedCountries.TryRemove(countryCode.ToUpper(), out _);
        }

        //Add country to block list with expiration time
        public async Task<bool> TemporarilyBlockCountryAsync(string countryCode, int durationMinutes)
        {
            var countryInfo = new CountryInfo
            {
                CountryCode = countryCode.ToUpper(),
                BlockedUntil = DateTime.UtcNow.AddMinutes(durationMinutes)
            };

            return _blockedCountries.TryAdd(countryCode.ToUpper(), countryInfo);
        }

        //Get all blocked countries with pagination
        public async Task<PaginatedResponse<CountryInfo>> GetBlockedCountriesAsync(int page, int pageSize, string searchTerm = null)
        {
            var query = _blockedCountries.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToUpper();
                query = query.Where(c => c.CountryCode.Contains(searchTerm) ||
                                       (c.CountryName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResponse<CountryInfo>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        //Check if country is blocked
        public async Task<bool> IsCountryBlockedAsync(string countryCode)
        {
            return _blockedCountries.TryGetValue(countryCode.ToUpper(), out var info) &&
                   (!info.BlockedUntil.HasValue || info.BlockedUntil.Value > DateTime.UtcNow);
        }
    }
}
