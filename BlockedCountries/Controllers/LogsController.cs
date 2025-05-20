using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    /// <summary>
    /// Controller for managing access logs and blocked attempt records
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptsLoggerRepository _attemptsLogger;
        public LogsController(IBlockedAttemptsLoggerRepository attemptsLogger)
        {
            _attemptsLogger = attemptsLogger;
        }

        /// <summary>
        /// Retrieves a paginated list of blocked access attempts
        /// </summary>
        /// <param name="page">The page number </param>
        /// <param name="pageSize">Number of items per page </param>
        /// <returns>A paginated list of blocked access attempts</returns>
        /// <response code="200">Returns the list of blocked attempts</response>
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
