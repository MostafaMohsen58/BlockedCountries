using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryBlockingRepository _blockingService;
        public CountryController(ICountryBlockingRepository blockingService)
        {
            _blockingService = blockingService;
        }
        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
                return BadRequest("country code is required");
            else if (countryCode.Length != 2)
                return BadRequest("country code must be 2 characters long");

            var result =await _blockingService.BlockCountryAsync(countryCode);

            return result ? Ok() : Conflict("Country is already blocked");
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {
            //if (string.IsNullOrEmpty(countryCode))
            //    return BadRequest("country code is required");
            //else if (countryCode.Length != 2)
            //    return BadRequest("country code must be 2 characters long");
            var result = await _blockingService.UnblockCountryAsync(countryCode);
            return result ? Ok() : NotFound();
        }


        [HttpGet("blocked")]
        public async Task<ActionResult<PaginatedResponse<CountryInfo>>> GetBlockedCountries(
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 10,
                                                    [FromQuery] string search = null)
        {
            var result = await _blockingService.GetBlockedCountriesAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost("temporal-block")]
        public async Task<IActionResult> TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
        {
            //maximum duration is 24 hours
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes");

            var result = await _blockingService.TemporarilyBlockCountryAsync(request.CountryCode, request.DurationMinutes);
            return result ? Ok() : Conflict("Country is already temporarily blocked");
        }
    }
}
