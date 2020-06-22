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
        
        private DateTime _todaysDate = DateTime.Today;
        private ApodDownloader apodDownloader = new ApodDownloader();

        [Fact]
        public void ShouldReturnApodUrlForTodaysDate()
        {
            // Arrange
            var date = _todaysDate;

            // Act
            var response = apodDownloader.BuildUrl(date);

            // Assert
            response.Should().Contain($"https://api.nasa.gov/planetary/apod?api_key=9D6EVFHS4mLEdvx6ZtN5i2XRv84kkfL3OwJYJLuQ&date=");
        }

        [Fact]
        public async Task ShouldReturnApodImageUrl()
        {
            // Arrange
            var date = DateTime.Today;
            var response = apodDownloader.BuildUrl(date);

            // Act
            var imageUrl = await apodDownloader.GetImageUrl(response);

            // Assert
            imageUrl.Should().Contain("https://apod.nasa.gov/apod/image/");

        }

        [Fact]
        public async Task ShouldFailOnDateWithoutAnApod()
        {
            // Arrange
            var date = new DateTime(2020, 06, 10);
            var response = apodDownloader.BuildUrl(date);

            // Act
            Func<Task> getImage = async () => await apodDownloader.GetImageUrl(response);

            // Assert
            getImage.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public async Task ShouldReturnStreamOfTypeImage()
        {
            // Arrange
            var date = new DateTime(2020, 06, 22);

            // Act
            var apodUrl = apodDownloader.BuildUrl(date);
            var imageUrl = await apodDownloader.GetImageUrl(apodUrl);
            var imageResponse = await apodDownloader.GetImage(imageUrl);

            // Assert
            imageResponse.Content.Headers.ContentType.Should().Be(System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg"));
        }
    }
}
