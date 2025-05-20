

using BlockedCountries.Repositories.interfaces;

namespace BlockedCountries.Services.Background
{
    public class TemporalBlockCleanupService:BackgroundService
    {
        private readonly ICountryBlockingRepository _blockingRepository;
        private readonly ILogger<TemporalBlockCleanupService> _logger;
        public TemporalBlockCleanupService(ICountryBlockingRepository blockingRepository,
            ILogger<TemporalBlockCleanupService> logger)
        {
            _blockingRepository = blockingRepository;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var allCountriesBlocked =await _blockingRepository.GetBlockedCountriesAsync(1, 1000);

                    foreach (var country in allCountriesBlocked.Items)
                    {
                        if(!country.IsTemporarilyBlocked && country.BlockedUntil !=null)
                        {
                            await _blockingRepository.UnblockCountryAsync(country.CountryCode);
                            _logger.LogInformation($"Unblocked expired for country: {country.CountryCode}");
                        }

                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {        
                    break;
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred while cleaning up temporary blocks.");
                }
            }
        }

    }
}
