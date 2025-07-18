using System.Collections.ObjectModel;
using System.Windows.Input;
using NewsAggregator.Models;
using NewsAggregator.Services;

namespace NewsAggregator.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INewsService _newsService;
        private bool _isRefreshing;
        private ThemeMode _currentThemeMode = ThemeMode.System;
        private ICommand? _refreshCommand;
        private ICommand? _setThemeCommand;

        public MainViewModel(INewsService newsService)
        {
            _newsService = newsService;
            Categories = new ObservableCollection<NewsCategoryViewModel>();
            InitializeCategories();

            // Apply initial theme based on system settings
            ThemeService.ApplyTheme(_currentThemeMode);
        }

        public ObservableCollection<NewsCategoryViewModel> Categories { get; }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ThemeMode CurrentThemeMode
        {
            get => _currentThemeMode;
            set
            {
                if (SetProperty(ref _currentThemeMode, value))
                {
                    ThemeService.ApplyTheme(value);
                }
            }
        }

        public ICommand RefreshCommand => _refreshCommand ??= new RelayCommand(async () => await RefreshAllAsync());

        public ICommand SetThemeCommand => _setThemeCommand ??= new RelayCommand<ThemeMode>(async (mode) => await SetThemeAsync(mode));

        private void InitializeCategories()
        {
            // Initialize categories in the specified order
            Categories.Add(new NewsCategoryViewModel(NewsCategory.Technology, _newsService));
            Categories.Add(new NewsCategoryViewModel(NewsCategory.Business, _newsService));
            Categories.Add(new NewsCategoryViewModel(NewsCategory.Sports, _newsService));
        }

        /// <summary>
        /// Loads all news categories in parallel
        /// </summary>
        public async Task LoadAllNewsAsync()
        {
            // Create tasks for each category to load in parallel
            var loadTasks = Categories.Select(category => category.LoadNewsAsync());

            // Execute all tasks concurrently
            await Task.WhenAll(loadTasks);
        }

        /// <summary>
        /// Refreshes all news categories
        /// </summary>
        private async Task RefreshAllAsync()
        {
            if (IsRefreshing)
                return;

            try
            {
                IsRefreshing = true;
                await LoadAllNewsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Sets the theme mode
        /// </summary>
        private async Task SetThemeAsync(ThemeMode mode)
        {
            CurrentThemeMode = mode;
            await Task.CompletedTask;
        }
    }
}