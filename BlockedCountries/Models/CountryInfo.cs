namespace BlockedCountries.Models
{
    public class CountryInfo
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public DateTime? BlockedUntil { get; set; }
        public bool IsTemporarilyBlocked => BlockedUntil.HasValue && BlockedUntil.Value > DateTime.UtcNow;
    }
}
