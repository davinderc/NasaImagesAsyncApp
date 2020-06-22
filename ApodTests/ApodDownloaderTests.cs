using System;
using FluentAssertions;
using Xunit;
using NasaImagesAsyncApp;

namespace ApodTests
{
    public class ApodDownloaderTests
    {
        private const string _dateFormat = "yyyy-MM-dd";

        [Fact]
        public void ShouldReturnApodUrlForTodaysDate()
        {
            // Arrange
            var date = DateTime.Today.ToString(_dateFormat);

            // Act
            var response = ApodDownloader.BuildUrl(date);

            // Assert
            response.Should().Contain("http://");
        }

        [Fact]
        public void ShouldReturnApodImageUrl()
        {
            // Arrange
            var date = DateTime.Today.ToString(_dateFormat);
            ApodDownloader.BuildUrl(date);

            // Act

            // Assert

        }
    }
}
