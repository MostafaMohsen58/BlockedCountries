using BlockedCountries.Models;
using BlockedCountries.Repositories.interfaces;
using BlockedCountries.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlockedCountries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IIpGeolocationService _ipGeolocationService;
        private readonly ICountryBlockingRepository _blockingRepository;
        private readonly IBlockedAttemptsLoggerRepository _attemptsLogger;
        public IpController(IIpGeolocationService ipGeolocationService, ICountryBlockingRepository blockingRepository
            , IBlockedAttemptsLoggerRepository attemptsLogger)
        {
            _ipGeolocationService = ipGeolocationService;
            _blockingRepository = blockingRepository;
            _attemptsLogger = attemptsLogger;
        }
        [HttpGet("lookup")]
        public async Task<ActionResult<CountryInfo>> lookupIp([FromQuery] string ipAddress=null)
        {
            ipAddress ??= GetCallerIp();
            if (!IsValidIpAddress(ipAddress))
            {
                return BadRequest("Invalid IP address format.");
            }

            try
            {
                var countryInfo =await _ipGeolocationService.GetCountryInfoByIpAsync(ipAddress);
                if (countryInfo == null)
                {
                    return NotFound("IP address not found.");
                }
                return Ok(countryInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [NonAction]
        private string GetCallerIp()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            //if (ip == "::1")      // IPv6 localhost
            //    return "127.0.0.1";

            return ip ?? "0.0.0.0";
        }
        [NonAction]
        private bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }
        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = GetCallerIp();
            var userAgent = Request.Headers.UserAgent.ToString();

            try
            {
                var countryInfo = await _ipGeolocationService.GetCountryInfoByIpAsync(ip);
                var isBlocked = await _blockingRepository.IsCountryBlockedAsync(countryInfo.CountryCode);

                await _attemptsLogger.LogAttemptAsync(new BlockedAttemptLog
                {
                    IpAddress = ip,
                    Timestamp = DateTime.UtcNow,
                    CountryCode = countryInfo.CountryCode,
                    BlockedStatus = isBlocked,
                    UserAgent = userAgent
                });

                return Ok(new { IsBlocked = isBlocked, CountryCode = countryInfo.CountryCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error checking block status");
            }
        }
    }
}
