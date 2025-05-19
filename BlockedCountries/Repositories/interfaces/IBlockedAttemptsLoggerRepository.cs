using BlockedCountries.Models;

namespace BlockedCountries.Repositories.interfaces
{
    public interface IBlockedAttemptsLoggerRepository
    {
        Task LogAttemptAsync(BlockedAttemptLog log);
        Task<PaginatedResponse<BlockedAttemptLog>> GetLogsAsync(int page, int pageSize);
    }
}
