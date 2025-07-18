using System.Windows;
using NewsAggregator.ViewModels;

namespace NewsAggregator
{
    /// <summary>
    /// Main application window
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Start loading news when the window is loaded
            if (DataContext is MainViewModel viewModel)
            {
                await viewModel.LoadAllNewsAsync();
            }
        }
    }
}