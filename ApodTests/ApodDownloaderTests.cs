using System;
using FluentAssertions;
using Xunit;
using NasaImagesAsyncApp;

namespace ApodTests
{
    public class ApodDownloaderTests
    {
        private const string _dateFormat = "yyyy-MM-dd";
        private static readonly ApodDownloader apodDownloader = new ApodDownloader();

        [Fact]
        public void ShouldReturnApodUrlForTodaysDate()
        {
            // Arrange
            var date = DateTime.Today.ToString(_dateFormat);

            // Act
            var response = apodDownloader.buildUrl(date);

            // Assert
            response.Should().Contain("http://");
        }

        [Fact]
        public void ShouldReturnApodImageUrl()
        {
            // Arrange
            var date = DateTime.Today.ToString(_dateFormat);
            apodDownloader.buildUrl(date);

            // Act

            // Assert

        }
    }
}
