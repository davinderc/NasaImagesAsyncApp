using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace NasaImagesAsyncApp
{
    public class ApodDownloader
    {
        private const string ApiKey = "9D6EVFHS4mLEdvx6ZtN5i2XRv84kkfL3OwJYJLuQ";
        private static HttpClient _httpClient;
        private static UriBuilder _uriBuilder;
        private static NameValueCollection _query;
        public ApodDownloader()
        {
            _httpClient = new HttpClient();
            _uriBuilder = new UriBuilder("https://api.nasa.gov/planetary/apod") {Port = -1};
            _query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            _query["api_key"] = ApiKey;
        }
        public static string BuildUrl(string date)
        {
            _query["date"] = date;
            _uriBuilder.Query = _query.ToString();
            var url = _uriBuilder.ToString();
            return url;
        }

        public static async Task<HttpResponseMessage> GetImage(string imageUrl)
        {
            return await _httpClient.GetAsync(imageUrl);
        }

        public static async Task<string> GetImageUrl(string apodUrl)
        {
            var response = await _httpClient.GetAsync(apodUrl);
            var apodMetadataString = await response.Content.ReadAsStringAsync();
            var apodMetadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(apodMetadataString);
            var imageUrl = apodMetadata["hdurl"];
            return imageUrl;
        }
    }
}
