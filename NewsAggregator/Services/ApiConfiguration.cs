
using Newtonsoft.Json;
using System.IO;

namespace NewsAggregator.Services
{
    /// <summary>
    /// Configuration for the News API
    /// </summary>
    public static class ApiConfiguration
    {
        private static string? _apiKey;
        public static string ApiKey
        {
            get
            {
                if (_apiKey == null)
                {
                    var json = File.ReadAllText("appsettings.json");
                    dynamic settings = JsonConvert.DeserializeObject(json);
                    _apiKey = settings.ApiKey;
                }
                return _apiKey;
            }
        }
        //public const string ApiKey = "API_KEY";//now moved to appsettings
        public const string BaseUrl = "https://newsapi.org/v2/";
        public const int MaxArticlesPerCategory = 10;
    }
}