using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace NasaImagesAsyncApp
{
    public class ApodDownloader
    {
        private const string ApiKey = "9D6EVFHS4mLEdvx6ZtN5i2XRv84kkfL3OwJYJLuQ";
        private const string DateFormat = "yyyy-MM-dd";
        private const string HdUrlKeyString = "hdurl";
        private readonly HttpClient _httpClient;
        private readonly UriBuilder _uriBuilder;
        private readonly NameValueCollection _query;

        public string BaseDirectory { get; set; } = $"C:/Users/dach/Dev/cSharpTraining/NasaImagesAsyncApp/NasaImagesAsyncApp/images/";

        public ApodDownloader()
        {
            _httpClient = new HttpClient();
            _uriBuilder = new UriBuilder("https://api.nasa.gov/planetary/apod") {Port = -1};
            _query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            _query["api_key"] = ApiKey;
        }
        public string BuildUrl(string date)
        {
            _query["date"] = date;
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
            var imageUrl = "";
            if (response.IsSuccessStatusCode)
            {
                 var apodMetadataString = await response.Content.ReadAsStringAsync();
                var apodMetadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(apodMetadataString);
                if (apodMetadata.ContainsKey("media_type") && apodMetadata["media_type"] == "image")
                {
                    imageUrl = apodMetadata[HdUrlKeyString];
                    return imageUrl;
                }
            }
            Console.WriteLine("No image response received!");
            return imageUrl;
        }

        public async Task<Image> DownloadImageForDate(string date)
        {
            var apodUrl = BuildUrl(date);
            var imageUrl = await GetImageUrl(apodUrl);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var image = await GetImage(imageUrl);
                var imageStream = Image.FromStream(await image.Content.ReadAsStreamAsync());
                return imageStream;
            }
            return null;
        }

        public List<string> GetSelectionRangeDateStrings(SelectionRange selectedDateRange)
        {
            var currentDate = selectedDateRange.Start;
            List<string> dateStrings = new List<string>();
            while (currentDate <= selectedDateRange.End)
            {
                dateStrings.Add(FormatDate(currentDate));
                currentDate = currentDate.AddDays(1);
            }
            return dateStrings;
        }

        public string FormatDate(DateTime currentDate)
        {
            return currentDate.ToString(DateFormat);
        }
    }
}
