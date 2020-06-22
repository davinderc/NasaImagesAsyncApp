using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace NasaImagesAsyncApp
{
    public class ApodDownloader
    {
        private const string ApiKey = "9D6EVFHS4mLEdvx6ZtN5i2XRv84kkfL3OwJYJLuQ";
        private const string _dateFormat = "yyyy-MM-dd";
        private HttpClient _httpClient;
        private UriBuilder _uriBuilder;
        private NameValueCollection _query;
        private string _baseDirectory = $"C:/Users/dach/Dev/cSharpTraining/NasaImagesAsyncApp/NasaImagesAsyncApp/";

        public ApodDownloader()
        {
            _httpClient = new HttpClient();
            _uriBuilder = new UriBuilder("https://api.nasa.gov/planetary/apod") {Port = -1};
            _query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            _query["api_key"] = ApiKey;
        }
        public string BuildUrl(DateTime date)
        {
            _query["date"] = date.ToString(_dateFormat);
            _uriBuilder.Query = _query.ToString();
            var url = _uriBuilder.ToString();
            return url;
        }

        public async Task<HttpResponseMessage> GetImage(string imageUrl)
        {
            return await _httpClient.GetAsync(imageUrl);
        }

        public async Task<string> GetImageUrl(string apodUrl)
        {
            
            var response = await _httpClient.GetAsync(apodUrl);
            var apodMetadataString = await response.Content.ReadAsStringAsync();
            var apodMetadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(apodMetadataString);
            var imageUrl = apodMetadata["hdurl"];
            return imageUrl;
        }

        public async Task<Image> DownloadImageForDate(DateTime date)
        {
            var apodUrl = BuildUrl(date);
            var imageUrl = await GetImageUrl(apodUrl);
            var image = await GetImage(imageUrl);
            var imageStream = Image.FromStream(await image.Content.ReadAsStreamAsync());
            imageStream.Save(_baseDirectory + $"{ date.ToString(_dateFormat)}.jpeg");
            return imageStream;
        }
    }
}
