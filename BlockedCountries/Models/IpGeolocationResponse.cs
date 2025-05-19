using Newtonsoft.Json;

namespace BlockedCountries.Models
{
    public class IpGeolocationResponse
    {
        [JsonProperty("country_code2")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }
    }
}
