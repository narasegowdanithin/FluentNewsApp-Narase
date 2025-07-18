using System.Windows;
using NewsAggregator.Services;
using NewsAggregator.ViewModels;

namespace NewsAggregator
{
    /// <summary>
    /// Application entry point and configuration
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Simple dependency injection setup
            var newsService = new NewsApiService();
            var mainViewModel = new MainViewModel(newsService);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();
        }
    }
}