﻿using BlockedCountries.Models;
using BlockedCountries.Services.Interfaces;
using Newtonsoft.Json;

namespace BlockedCountries.Services
{
    public class IpGeolocationService : IIpGeolocationService
    {
        public static readonly Dictionary<string, string> CountryNames = new Dictionary<string, string>
        {
            {"AF", "Afghanistan"}, {"AX", "Aland Islands"}, {"AL", "Albania"}, {"DZ", "Algeria"},
            {"AS", "American Samoa"}, {"AD", "Andorra"}, {"AO", "Angola"}, {"AI", "Anguilla"},
            {"AQ", "Antarctica"}, {"AG", "Antigua and Barbuda"}, {"AR", "Argentina"}, {"AM", "Armenia"},
            {"AW", "Aruba"}, {"AU", "Australia"}, {"AT", "Austria"}, {"AZ", "Azerbaijan"},
            {"BS", "Bahamas"}, {"BH", "Bahrain"}, {"BD", "Bangladesh"}, {"BB", "Barbados"},
            {"BY", "Belarus"}, {"BE", "Belgium"}, {"BZ", "Belize"}, {"BJ", "Benin"},
            {"BM", "Bermuda"}, {"BT", "Bhutan"}, {"BO", "Bolivia"}, {"BA", "Bosnia and Herzegovina"},
            {"BW", "Botswana"}, {"BR", "Brazil"}, {"BN", "Brunei"}, {"BG", "Bulgaria"},
            {"BF", "Burkina Faso"}, {"BI", "Burundi"}, {"KH", "Cambodia"}, {"CM", "Cameroon"},
            {"CA", "Canada"}, {"CV", "Cape Verde"}, {"KY", "Cayman Islands"}, {"CF", "Central African Republic"},
            {"TD", "Chad"}, {"CL", "Chile"}, {"CN", "China"}, {"CO", "Colombia"},
            {"KM", "Comoros"}, {"CG", "Congo"}, {"CD", "Congo, Democratic Republic"}, {"CK", "Cook Islands"},
            {"CR", "Costa Rica"}, {"HR", "Croatia"}, {"CU", "Cuba"}, {"CY", "Cyprus"},
            {"CZ", "Czech Republic"}, {"DK", "Denmark"}, {"DJ", "Djibouti"}, {"DM", "Dominica"},
            {"DO", "Dominican Republic"}, {"EC", "Ecuador"}, {"EG", "Egypt"}, {"SV", "El Salvador"},
            {"GQ", "Equatorial Guinea"}, {"ER", "Eritrea"}, {"EE", "Estonia"}, {"ET", "Ethiopia"},
            {"FK", "Falkland Islands"}, {"FO", "Faroe Islands"}, {"FJ", "Fiji"}, {"FI", "Finland"},
            {"FR", "France"}, {"GF", "French Guiana"}, {"PF", "French Polynesia"}, {"GA", "Gabon"},
            {"GM", "Gambia"}, {"GE", "Georgia"}, {"DE", "Germany"}, {"GH", "Ghana"},
            {"GI", "Gibraltar"}, {"GR", "Greece"}, {"GL", "Greenland"}, {"GD", "Grenada"},
            {"GP", "Guadeloupe"}, {"GU", "Guam"}, {"GT", "Guatemala"}, {"GN", "Guinea"},
            {"GW", "Guinea-Bissau"}, {"GY", "Guyana"}, {"HT", "Haiti"}, {"HN", "Honduras"},
            {"HK", "Hong Kong"}, {"HU", "Hungary"}, {"IS", "Iceland"}, {"IN", "India"},
            {"ID", "Indonesia"}, {"IR", "Iran"}, {"IQ", "Iraq"}, {"IE", "Ireland"},
            {"IL", "Israel"}, {"IT", "Italy"}, {"JM", "Jamaica"}, {"JP", "Japan"},
            {"JO", "Jordan"}, {"KZ", "Kazakhstan"}, {"KE", "Kenya"}, {"KI", "Kiribati"},
            {"KP", "North Korea"}, {"KR", "South Korea"}, {"KW", "Kuwait"}, {"KG", "Kyrgyzstan"},
            {"LA", "Laos"}, {"LV", "Latvia"}, {"LB", "Lebanon"}, {"LS", "Lesotho"},
            {"LR", "Liberia"}, {"LY", "Libya"}, {"LI", "Liechtenstein"}, {"LT", "Lithuania"},
            {"LU", "Luxembourg"}, {"MO", "Macao"}, {"MG", "Madagascar"}, {"MW", "Malawi"},
            {"MY", "Malaysia"}, {"MV", "Maldives"}, {"ML", "Mali"}, {"MT", "Malta"},
            {"MH", "Marshall Islands"}, {"MQ", "Martinique"}, {"MR", "Mauritania"}, {"MU", "Mauritius"},
            {"YT", "Mayotte"}, {"MX", "Mexico"}, {"FM", "Micronesia"}, {"MD", "Moldova"},
            {"MC", "Monaco"}, {"MN", "Mongolia"}, {"ME", "Montenegro"}, {"MS", "Montserrat"},
            {"MA", "Morocco"}, {"MZ", "Mozambique"}, {"MM", "Myanmar"}, {"NA", "Namibia"},
            {"NR", "Nauru"}, {"NP", "Nepal"}, {"NL", "Netherlands"}, {"NC", "New Caledonia"},
            {"NZ", "New Zealand"}, {"NI", "Nicaragua"}, {"NE", "Niger"}, {"NG", "Nigeria"},
            {"NU", "Niue"}, {"NF", "Norfolk Island"}, {"MK", "North Macedonia"}, {"MP", "Northern Mariana Islands"},
            {"NO", "Norway"}, {"OM", "Oman"}, {"PK", "Pakistan"}, {"PW", "Palau"},
            {"PS", "Palestine"}, {"PA", "Panama"}, {"PG", "Papua New Guinea"}, {"PY", "Paraguay"},
            {"PE", "Peru"}, {"PH", "Philippines"}, {"PN", "Pitcairn"}, {"PL", "Poland"},
            {"PT", "Portugal"}, {"PR", "Puerto Rico"}, {"QA", "Qatar"}, {"RE", "Reunion"},
            {"RO", "Romania"}, {"RU", "Russia"}, {"RW", "Rwanda"}, {"BL", "Saint Barthelemy"},
            {"SH", "Saint Helena"}, {"KN", "Saint Kitts and Nevis"}, {"LC", "Saint Lucia"},
            {"MF", "Saint Martin"}, {"PM", "Saint Pierre and Miquelon"}, {"VC", "Saint Vincent and the Grenadines"},
            {"WS", "Samoa"}, {"SM", "San Marino"}, {"ST", "Sao Tome and Principe"}, {"SA", "Saudi Arabia"},
            {"SN", "Senegal"}, {"RS", "Serbia"}, {"SC", "Seychelles"}, {"SL", "Sierra Leone"},
            {"SG", "Singapore"}, {"SX", "Sint Maarten"}, {"SK", "Slovakia"}, {"SI", "Slovenia"},
            {"SB", "Solomon Islands"}, {"SO", "Somalia"}, {"ZA", "South Africa"},
            {"GS", "South Georgia and South Sandwich Islands"}, {"SS", "South Sudan"}, {"ES", "Spain"},
            {"LK", "Sri Lanka"}, {"SD", "Sudan"}, {"SR", "Suriname"}, {"SJ", "Svalbard and Jan Mayen"},
            {"SE", "Sweden"}, {"CH", "Switzerland"}, {"SY", "Syria"}, {"TW", "Taiwan"},
            {"TJ", "Tajikistan"}, {"TZ", "Tanzania"}, {"TH", "Thailand"}, {"TL", "Timor-Leste"},
            {"TG", "Togo"}, {"TK", "Tokelau"}, {"TO", "Tonga"}, {"TT", "Trinidad and Tobago"},
            {"TN", "Tunisia"}, {"TR", "Turkey"}, {"TM", "Turkmenistan"}, {"TC", "Turks and Caicos Islands"},
            {"TV", "Tuvalu"}, {"UG", "Uganda"}, {"UA", "Ukraine"}, {"AE", "United Arab Emirates"},
            {"GB", "United Kingdom"}, {"US", "United States"}, {"UM", "United States Minor Outlying Islands"},
            {"UY", "Uruguay"}, {"UZ", "Uzbekistan"}, {"VU", "Vanuatu"}, {"VE", "Venezuela"},
            {"VN", "Vietnam"}, {"VG", "Virgin Islands, British"}, {"VI", "Virgin Islands, U.S."},
            {"WF", "Wallis and Futuna"}, {"EH", "Western Sahara"}, {"YE", "Yemen"},
            {"ZM", "Zambia"}, {"ZW", "Zimbabwe"}
        };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public IpGeolocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<IpGeolocationResponse> GetCountryInfoByIpAsync(string ipAddress)
        {
            var apiKey = _configuration["IpGeolocation:ApiKey"];
            var request =await _httpClient.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={apiKey}&ip={ipAddress}");

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IpGeolocationResponse>(response);
                
                return new IpGeolocationResponse
                {
                    CountryCode = data.CountryCode,
                    CountryName = data.CountryName
                };
            }
            else
            {
                throw new Exception("Error fetching data from IP Geolocation API");
            }
        }
        public async Task<bool> IsValidCountryCodeAsync(string countryCode)
        {
            //return ValidCountryCodes.Contains(countryCode?.ToUpper() ?? string.Empty);
            return CountryNames.ContainsKey(countryCode.ToUpper());
        }

        public async Task<string> GetCountryNameAsync(string countryCode)
        {
            if (!await IsValidCountryCodeAsync(countryCode))
            {
                throw new ArgumentException("Invalid country code");
            }

            return CountryNames.ContainsKey(countryCode.ToUpper())
                ? CountryNames[countryCode.ToUpper()]: countryCode.ToUpper();
        }
    }
}
