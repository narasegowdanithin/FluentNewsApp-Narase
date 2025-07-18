
namespace NewsAggregator.Models
{
    /// <summary>
    /// Represents a single news article
    /// </summary>
    public class NewsArticle
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string UrlToImage { get; set; } = string.Empty;
    }
}