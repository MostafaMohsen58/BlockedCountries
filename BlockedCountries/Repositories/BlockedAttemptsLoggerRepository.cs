using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.Repositories
{
    public class BlockedAttemptsLoggerRepository : IBlockedAttemptsLoggerRepository
    {
        private readonly ConcurrentBag<BlockedAttemptLog> _blockedAttemptLogs = new ConcurrentBag<BlockedAttemptLog>();
        public async Task LogAttemptAsync(BlockedAttemptLog log)
        {
            _blockedAttemptLogs.Add(log);
        }
        public async Task<PaginatedResponse<BlockedAttemptLog>> GetLogsAsync(int page, int pageSize)
        {
            int total = _blockedAttemptLogs.Count;
            int totalPages = (int) Math.Ceiling(total / (double)pageSize);
            var logs = _blockedAttemptLogs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ///return new Task<PaginatedResponse<BlockedAttemptLog>>(() =>
            ///    new PaginatedResponse<BlockedAttemptLog>
            ///    {
            ///        TotalCount = total,
            ///        TotalPages = totalPages,
            ///        CurrentPage = page,
            ///        PageSize = pageSize,
            ///        Items = logs
            ///    });
            return new PaginatedResponse<BlockedAttemptLog>
            {
                Items = logs,
                TotalCount = total,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        
    }
}
