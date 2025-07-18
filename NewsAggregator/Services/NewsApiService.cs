using System.Net.Http;
using Newtonsoft.Json;
using NewsAggregator.Models;

namespace NewsAggregator.Services
{
    /// <summary>
    /// Implementation of INewsService using NewsAPI.org
    /// </summary>
    public class NewsApiService : INewsService
    {
        private readonly HttpClient _httpClient;

        public NewsApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "NewsAggregator/1.0 (.NET 9 WPF Application)");
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(NewsCategory category)
        {
            try
            {
                string categoryQuery = GetCategoryQuery(category);
                // Request 20 articles to ensure we get at least 10 valid ones after filtering
                string url = $"{ApiConfiguration.BaseUrl}top-headlines?category={categoryQuery}&sortBy=publishedAt&apiKey={ApiConfiguration.ApiKey}&pageSize=20";

                var response = await _httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonConvert.DeserializeObject<NewsApiResponse>(json);
                    throw new Exception($"API Error: {errorResponse?.Message ?? response.StatusCode.ToString()}");
                }

                var newsResponse = JsonConvert.DeserializeObject<NewsApiResponse>(json);

                if (newsResponse?.Status == "ok" && newsResponse.Articles != null)
                {
                    return newsResponse.Articles
                        .Where(a => !string.IsNullOrWhiteSpace(a.Title)) // Filter out articles without titles
                        .Take(ApiConfiguration.MaxArticlesPerCategory)
                        .Select(a => new NewsArticle
                        {
                            Title = a.Title!.Trim(), // We know it's not null due to Where clause
                            Description = a.Description ?? string.Empty,
                            Url = a.Url ?? string.Empty,
                            PublishedAt = DateTime.TryParse(a.PublishedAt, out var date) ? date : DateTime.Now,
                            Source = a.Source?.Name ?? "Unknown Source",
                            Author = a.Author ?? "Unknown Author",
                            UrlToImage = a.UrlToImage ?? string.Empty
                        });
                }

                return Enumerable.Empty<NewsArticle>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error while fetching {category} news", ex);
            }
           
            catch (Exception ex)
            {   
                throw new Exception($"Error processing {category} news:{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        private string GetCategoryQuery(NewsCategory category)
        {
            return category switch
            {
                NewsCategory.Technology => "technology",
                NewsCategory.Business => "business",
                NewsCategory.Sports => "sports",
                _ => "general"
            };
        }

        // DTOs for deserializing NewsAPI response
        private class NewsApiResponse
        {
            public string? Status { get; set; }
            public int TotalResults { get; set; }
            public List<NewsApiArticle>? Articles { get; set; }
            public string? Code { get; set; }
            public string? Message { get; set; }
        }

        private class NewsApiArticle
        {
            public NewsApiSource? Source { get; set; }
            public string? Author { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Url { get; set; }
            public string? UrlToImage { get; set; }
            public string? PublishedAt { get; set; }
        }

        private class NewsApiSource
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
    }
}