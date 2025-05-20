using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using BlockedCountries.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
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
        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] BlockCountryRequest blockCountryDto)
        {
            if (string.IsNullOrEmpty(blockCountryDto.CountryCode))
                return BadRequest("country code is required");
            
            if (blockCountryDto.CountryCode.Length != 2)
                return BadRequest("country code must be 2 characters long");

            if (!await _ipGeolocationService.IsValidCountryCodeAsync(blockCountryDto.CountryCode))
                return BadRequest("Invalid country code");

            var result =await _blockingRepository.BlockCountryAsync(blockCountryDto.CountryCode);

            return result ? Ok("Country successfully blocked") : Conflict("Country is already blocked");
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {
            var result = await _blockingRepository.UnblockCountryAsync(countryCode);
            return result ? Ok() : NotFound();
        }


        [HttpGet("blocked")]
        public async Task<ActionResult<PaginatedResponse<CountryInfo>>> GetBlockedCountries(
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 10,
                                                    [FromQuery] string search = null)
        {
            var result = await _blockingRepository.GetBlockedCountriesAsync(page, pageSize, search);
            return Ok(result);
        }


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
            return result ? Ok() : Conflict("Country is already temporarily blocked");
        }
    }
}
