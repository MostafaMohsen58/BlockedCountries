using BlockedCountries.Models;
using BlockedCountries.Services.Interfaces;
using System.Text.Json;

namespace BlockedCountries.Services
{
    public class IpGeolocationService : IIpGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public IpGeolocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<CountryInfo> GetCountryInfoByIpAsync(string ipAddress)
        {
            var apiKey = _configuration["IpGeolocation:ApiKey"];
            var request =await _httpClient.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={apiKey}&ip={ipAddress}");

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonDocument>(response);
                //return data;
                return new CountryInfo
                {
                    CountryCode = data.RootElement.GetProperty("country_code2").GetString(),
                    CountryName = data.RootElement.GetProperty("country_name").GetString()
                };
            }
            else
            {
                throw new Exception("Error fetching data from IP Geolocation API");
            }
        }
    }
}
