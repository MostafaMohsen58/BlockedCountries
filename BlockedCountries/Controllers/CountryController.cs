using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using BlockedCountries.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    /// <summary>
    /// Controller for managing country blocking operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryBlockingRepository _blockingRepository;
        private readonly IIpGeolocationService _ipGeolocationService;
        public CountryController(ICountryBlockingRepository blockingRepository,IIpGeolocationService ipGeolocationService)
        {
            _blockingRepository = blockingRepository;
            _ipGeolocationService = ipGeolocationService;
        }

        /// <summary>
        /// Blocks a country by its country code
        /// </summary>
        /// <param name="blockCountryDto">The country code to block</param>
        /// <returns>200 OK if successful, 400 Bad Request if invalid input, 409 Conflict if already blocked</returns>
        /// <response code="200">Country successfully blocked</response>
        /// <response code="400">Invalid country code or format</response>
        /// <response code="409">Country is already blocked</response>
        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] BlockCountryRequest blockCountryDto)
        {
            if (string.IsNullOrEmpty(blockCountryDto.CountryCode))
                return BadRequest("country code is required");
            
            if (blockCountryDto.CountryCode.Length != 2)
                return BadRequest("Invalid country code or format");

            if (!await _ipGeolocationService.IsValidCountryCodeAsync(blockCountryDto.CountryCode))
                return BadRequest("Invalid country code or format");

            var countryInfo = new CountryInfo
            {
                CountryCode = blockCountryDto.CountryCode,
                CountryName = await _ipGeolocationService.GetCountryNameAsync(blockCountryDto.CountryCode)
            };
            //var result =await _blockingRepository.BlockCountryAsync(blockCountryDto.CountryCode);
            var result = await _blockingRepository.BlockCountryAsync(countryInfo);


            return result ? Ok("Country successfully blocked") : Conflict("Country is already blocked");
        }

        /// <summary>
        /// Unblocks a previously blocked country
        /// </summary>
        /// <param name="countryCode">The two-letter country code to unblock</param>
        /// <returns>200 OK if successful, 404 Not Found if country wasn't blocked</returns>
        /// <response code="200">The country unblocked successfully</response>
        /// <response code="400">Invalid country or country already unblocked</response>
        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {
            var result = await _blockingRepository.UnblockCountryAsync(countryCode);
            return result ? Ok("The country unblocked successfully") : NotFound("Invalid country or country already unblocked");
        }


        /// <summary>
        /// Retrieves a paginated list of blocked countries
        /// </summary>
        /// <param name="page">The page number </param>
        /// <param name="pageSize">Number of items per page </param>
        /// <param name="search">Optional search term for filtering results</param>
        /// <returns>A paginated list of blocked countries</returns>
        [HttpGet("blocked")]
        [ProducesResponseType(typeof(PaginatedResponse<CountryInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResponse<CountryInfo>>> GetBlockedCountries(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = null)
        {
            var result = await _blockingRepository.GetBlockedCountriesAsync(page, pageSize, search);
            return Ok(result);
        }


        /// <summary>
        /// Temporarily blocks a country for a specified duration
        /// </summary>
        /// <param name="request">The temporal block request containing country code and duration</param>
        /// <remarks>
        /// The duration must be between 1 and 1440 minutes (24 hours).
        /// The country code must be a valid 2-letter ISO country code.
        /// </remarks>
        /// <returns>200 OK if successful, 400 for invalid input, 409 if already blocked</returns>
        /// <response code="200">Country is temporarily blocked successfully for _ Min</response>
        /// <response code="400">Invalid country code or Duration is wrong</response>
        /// <response code="409">Country is already blocked</response>
        [HttpPost("temporal-block")]
        public async Task<IActionResult> TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
        {
            //maximum duration is 24 hours
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes");

            //validate country code is valid one not XX
            if(!await _ipGeolocationService.IsValidCountryCodeAsync(request.CountryCode))
                return BadRequest("Invalid country code");

            var result = await _blockingRepository.TemporarilyBlockCountryAsync(request.CountryCode, request.DurationMinutes);
            return result ? Ok($"Country is blocked successfully for {request.DurationMinutes} Min") : Conflict("Country is already temporarily blocked");
        }
    }
}
