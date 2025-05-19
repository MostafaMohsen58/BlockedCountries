namespace BlockedCountries.Models
{
    public class TemporalBlockRequest
    {
        public string CountryCode { get; set; }
        public int DurationMinutes { get; set; }
    }
}
