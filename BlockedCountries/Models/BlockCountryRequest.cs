namespace BlockedCountries.Models
{
    /// <summary>
    /// Request model for blocking a country
    /// </summary>
    public class BlockCountryRequest
    {
        /// <summary>
        /// Two-letter ISO country code (e.g., "US", "GB")
        /// </summary>
        /// <example>US</example>
        public string CountryCode { get; set; }
    }
}
