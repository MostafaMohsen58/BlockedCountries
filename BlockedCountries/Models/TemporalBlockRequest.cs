/// <summary>
/// Request model for temporarily blocking a country
/// </summary>
public class TemporalBlockRequest
{
    /// <summary>
    /// Two-letter ISO country code (e.g., "US", "GB")
    /// </summary>
    /// <example>US</example>
    public string CountryCode { get; set; }

    /// <summary>
    /// Duration in minutes for which the country should be blocked (1-1440)
    /// </summary>
    /// <example>120</example>
    public int DurationMinutes { get; set; }
}
