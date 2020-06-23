using FluentAssertions;
using NasaImagesAsyncApp;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace ApodTests
{
    public class ApodDownloaderTests
    {
        
        private readonly DateTime _todayDate = DateTime.Today;
        private static readonly string ApiKey = "DEMO_KEY";
        private readonly ApodDownloader _apodDownloader = new ApodDownloader(ApiKey);

        [Fact]
        public void ShouldReturnApodUrlForTodayDate()
        {
            // Arrange
            var date = _todayDate;

            // Act
            var response = _apodDownloader.BuildUrl(_apodDownloader.FormatDate(date));

            // Assert
            response.Should().Contain($"https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=");
        }

        [Fact]
        public async Task ShouldReturnApodImageUrl()
        {
            // Arrange
            var date = DateTime.Today;
            var response = _apodDownloader.BuildUrl(_apodDownloader.FormatDate(date));

            // Act
            var imageUrl = await _apodDownloader.GetImageUrl(response);

            // Assert
            imageUrl.Should().Contain("https://apod.nasa.gov/apod/image/");

        }

        [Fact]
        public async Task ShouldFailOnDateWithoutAnApod()
        {
            // Arrange
            var date = new DateTime(2020, 06, 3);
            var response = _apodDownloader.BuildUrl(_apodDownloader.FormatDate(date));

            // Act
            var getImage = await _apodDownloader.GetImageUrl(response);

            // Assert
            getImage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldReturnStreamOfTypeImage()
        {
            // Arrange
            var date = new DateTime(2020, 06, 22);

            // Act
            var apodUrl = _apodDownloader.BuildUrl(_apodDownloader.FormatDate(date));
            var imageUrl = await _apodDownloader.GetImageUrl(apodUrl);
            var imageResponse = await _apodDownloader.GetImage(imageUrl);

            // Assert
            imageResponse.Content.Headers.ContentType.Should().Be(System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg"));
        }
    }
}
