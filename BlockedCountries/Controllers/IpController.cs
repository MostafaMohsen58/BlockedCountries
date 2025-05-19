using BlockedCountries.Models;
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
        public IpController(IIpGeolocationService ipGeolocationService)
        {
            _ipGeolocationService = ipGeolocationService;
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
    }
}
