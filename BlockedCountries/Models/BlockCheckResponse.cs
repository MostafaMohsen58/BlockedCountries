namespace BlockedCountries.Models
{
    /// <summary>
    /// Response model for IP block check
    /// </summary>
    public class BlockCheckResponse
    {
        /// <summary>
        /// Indicates whether the IP address is from a blocked country
        /// </summary>
        /// <example>true</example>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// The two-letter country code of the IP address
        /// </summary>
        /// <example>US</example>
        public string CountryCode { get; set; }
    }
}
