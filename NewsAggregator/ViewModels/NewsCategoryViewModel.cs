using System.Collections.ObjectModel;
using NewsAggregator.Models;
using NewsAggregator.Services;

namespace NewsAggregator.ViewModels
{
    /// <summary>
    /// ViewModel for a single news category section
    /// </summary>
    public class NewsCategoryViewModel : ViewModelBase
    {
        private readonly INewsService _newsService;
        private bool _isLoading;
        private bool _hasError;
        private string _errorMessage = string.Empty;
        private ObservableCollection<NewsArticle> _articles = new();

        public NewsCategoryViewModel(NewsCategory category, INewsService newsService)
        {
            Category = category;
            _newsService = newsService;
            Articles = new ObservableCollection<NewsArticle>();
        }

        public NewsCategory Category { get; }

        public string CategoryName => Category.ToString();

        public string CategoryIcon => Category switch
        {
            NewsCategory.Technology => "📰",
            NewsCategory.Business => "💼",
            NewsCategory.Sports => "⚽",
            _ => "📄"
        };

        public ObservableCollection<NewsArticle> Articles
        {
            get => _articles;
            set => SetProperty(ref _articles, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Loads news articles for this category
        /// </summary>
        public async Task LoadNewsAsync()
        {
            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = string.Empty;
                Articles.Clear();

                var articles = await _newsService.GetNewsByCategoryAsync(Category);

                foreach (var article in articles)
                {
                    Articles.Add(article);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Error fetching {CategoryName} news: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}