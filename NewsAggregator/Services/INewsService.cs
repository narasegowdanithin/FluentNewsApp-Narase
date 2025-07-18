using NewsAggregator.Models;

namespace NewsAggregator.Services
{
    /// <summary>
    /// Interface for news service operations
    /// </summary>
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(NewsCategory category);
    }
}