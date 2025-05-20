using BlockedCountries.Models;
using BlockedCountries.Repositories;
using BlockedCountries.Repositories.interfaces;
using BlockedCountries.Services;
using BlockedCountries.Services.Interfaces;
using Moq;

namespace BlockCountries.Tests
{
    public class CountryRepositoryTests
    {


        private readonly ICountryBlockingRepository _countryBlockingRepository;
        private readonly Mock<IIpGeolocationService> _ipGeolocationService;
        public CountryRepositoryTests()
        {
            _countryBlockingRepository = new CountryBlockingRepository();
            _ipGeolocationService = new Mock<IIpGeolocationService>();
        }
        [Fact]
        public async void AddCountry_WithValidCountry_ShouldReturnTrue()
        {
            //arrange
            CountryInfo countryInfo = new CountryInfo
            {
                CountryCode = "GB",
                CountryName = "United Kingdom"
            };

            //act
            var result = _countryBlockingRepository.BlockCountryAsync(countryInfo).Result;

            //assert
            Assert.True(result);
            Assert.True(await _countryBlockingRepository.IsCountryBlockedAsync(countryInfo.CountryCode));
        }
        [Fact]
        public async Task AddCountry_WithDuplicateCountry_ShouldReturnFalse()
        {
            //arrange
            CountryInfo countryInfo = new CountryInfo
            {
                CountryCode = "GB",
                CountryName = "United Kingdom"
            };

            await _countryBlockingRepository.BlockCountryAsync(countryInfo);

            //act
            var result = await _countryBlockingRepository.BlockCountryAsync(countryInfo);

            //assert
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveCountry_WithExistingCountry_ShouldReturnTrue()
        {
            // Arrange
            CountryInfo countryInfo = new CountryInfo
            {
                CountryCode = "US"
            };
            await _countryBlockingRepository.BlockCountryAsync(countryInfo);

            // Act
            var result = await _countryBlockingRepository.UnblockCountryAsync(countryInfo.CountryCode);

            // Assert
            Assert.True(result);
            Assert.False(await _countryBlockingRepository.IsCountryBlockedAsync(countryInfo.CountryCode));
        }

        [Fact]
        public async Task RemoveCountry_WithNonExistingCountry_ShouldReturnFalse()
        {
            //Act
            var result = await _countryBlockingRepository.UnblockCountryAsync("EG");

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddTemporaryBlock_WithValidDuration_ShouldReturnTrue()
        {
            // Arrange
            var countryCode = "US";
            var countryName = "United States";
            var durationMinutes = 60;

            // Act
            var result = await _countryBlockingRepository.TemporarilyBlockCountryAsync(countryCode, countryName, durationMinutes);

            // Assert
            Assert.True(result);
            Assert.True(await _countryBlockingRepository.IsCountryBlockedAsync(countryCode));
        }



        [Fact]
        public async Task IsCountryBlocked_WithExpiredTemporaryBlock_ShouldReturnFalse()
        {
            // Arrange
            await _countryBlockingRepository.TemporarilyBlockCountryAsync("US", "United States", 0); // Immediate expiration

            // Act
            var result = await _countryBlockingRepository.IsCountryBlockedAsync("US");

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("XXX", false)] 
        [InlineData("US", true)]   // Only the valid one
        public async Task AddCountry_WithVariousInputs_ShouldValidateCorrectly(string countryCode, bool expectedResult)
        {
            // Arrange
            CountryInfo countryInfo = new CountryInfo
            {
                CountryCode = countryCode
            };

            // Mock the IsValidCountryCodeAsync method to return true for valid country codes
            _ipGeolocationService.Setup(x => x.IsValidCountryCodeAsync(countryCode))
                .ReturnsAsync(expectedResult);

            // Act
            bool result;
            if (_ipGeolocationService.Object.IsValidCountryCodeAsync(countryCode).Result)
            {
                result = await _countryBlockingRepository.BlockCountryAsync(countryInfo);
            }
            else
            {
                result = false;
            }

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
