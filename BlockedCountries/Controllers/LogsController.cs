using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptsLoggerRepository _attemptsLogger;
        public LogsController(IBlockedAttemptsLoggerRepository attemptsLogger)
        {
            _attemptsLogger = attemptsLogger;
        }
        [HttpGet("blocked-attempts")]
        public async Task<ActionResult<PaginatedResponse<BlockedAttemptLog>>> GetLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _attemptsLogger.GetLogsAsync(page, pageSize);
            return Ok(result);
        }
    }
}
