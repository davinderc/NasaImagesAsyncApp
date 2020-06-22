using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using NasaImagesAsyncApp;

namespace ApodTests
{
    public class ApodDownloaderTests
    {
        
        private readonly DateTime _todayDate = DateTime.Today;
        private readonly ApodDownloader _apodDownloader = new ApodDownloader();

        [Fact]
        public void ShouldReturnApodUrlForTodayDate()
        {
            // Arrange
            var date = _todayDate;

            // Act
            var response = _apodDownloader.BuildUrl(date);

            // Assert
            response.Should().Contain($"https://api.nasa.gov/planetary/apod?api_key=9D6EVFHS4mLEdvx6ZtN5i2XRv84kkfL3OwJYJLuQ&date=");
        }

        [Fact]
        public async Task ShouldReturnApodImageUrl()
        {
            // Arrange
            var date = DateTime.Today;
            var response = _apodDownloader.BuildUrl(date);

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
            var response = _apodDownloader.BuildUrl(date);

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
            var apodUrl = _apodDownloader.BuildUrl(date);
            var imageUrl = await _apodDownloader.GetImageUrl(apodUrl);
            var imageResponse = await _apodDownloader.GetImage(imageUrl);

            // Assert
            imageResponse.Content.Headers.ContentType.Should().Be(System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg"));
        }
    }
}
