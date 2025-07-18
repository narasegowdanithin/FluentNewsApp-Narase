# News Aggregator WPF Application

A modern Windows desktop application built with .NET 9 and WPF that aggregates news from multiple categories using the NewsAPI service. The application demonstrates parallel asynchronous programming, MVVM architecture, and features a beautiful Fluent Design UI with theme support.

## Table of Contents

- [Features](#features)
- [Screenshots](#screenshots)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Technical Details](#technical-details)
- [API Reference](#api-reference)


##   Features

- **Parallel News Loading**: Fetches news from multiple categories simultaneously
- **Real-time Updates**: Each category section updates independently as data arrives
- **Theme Support**: Light, Dark, and System theme modes
- **Error Handling**: Graceful error handling with user-friendly messages
- **Modern UI**: Native .NET 9 Fluent Design System
- **Responsive Design**: Clean, intuitive interface with loading indicators
- **MVVM Architecture**: Clean separation of concerns with testable code

## Screenshots

### Light Theme
![Light Theme](https://github.com/narasegowdanithin/FluentNewsApp-Narase/blob/master/NewsAggregator/Images/LightTheme.png)

### Dark Theme
![Dark Theme](https://github.com/narasegowdanithin/FluentNewsApp-Narase/blob/master/NewsAggregator/Images/DarkTheme.png)


##  Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern with clear separation of concerns:

```
NewsAggregator/
├── Models/          # Data structures
├── ViewModels/      # Business logic and state
├── MainWindow & App # UI (XAML)
├── Services/        # API integration
├── Converters/      # Value converters
└── Resources/       # Styles and themes
```

### Key Components:

- **Models**: Simple data classes (`NewsArticle`, `NewsCategory`, `ThemeMode`)
- **ViewModels**: Handle UI logic and state management
- **Services**: API communication and theme management
- **Views**: XAML-based UI with data binding

## Prerequisites

- **.NET 9 SDK**
- **Windows 10/11** (for WPF support)
- **Visual Studio 2022** 
- **[NewsAPI Key](https://newsapi.org/)**
  
## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/narasegowdanithin/FluentNewsApp-Narase.git)
   cd NewsAggregator
   ```

2. **Create configuration file**
   ```bash
   # add the configuration file
    appsettings.json
   
   # Edit appsettings.json and add your API key
   ```

3. **Get your API Key**
   - Sign up at [NewsAPI.org](https://newsapi.org/)
   - Copy your API key
   - Update `appsettings.json` with your key

4. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

5. **Build the project**
   ```bash
   dotnet build
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

##  Configuration

### Setting up your API Key

1. Get a free API key from [NewsAPI.org](https://newsapi.org/)

2. Create an `appsettings.json` file in the project root:
   ```json
   {
       "ApiKey": "your_api_key_here"
   }
   ```

3. **Important**: Add `appsettings.json` to your `.gitignore` file to prevent committing your API key:
   ```gitignore
   # API Configuration
   appsettings.json
   ```

### Configuration Loading

The application loads the API key from the appsettings.json file:
```csharp
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
    public const string BaseUrl = "https://newsapi.org/v2/";
    public const int MaxArticlesPerCategory = 10;
}
```

### Build Configuration

Ensure your `appsettings.json` is copied to the output directory by adding this to your `.csproj` file:
```xml
<ItemGroup>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

### Security Note

**Never commit your actual API key to source control!** Always use:
- Configuration files (excluded from git)

##  Usage

### Basic Operation

1. **Launch**: Start the application to automatically load news from all categories
2. **Refresh**: Click "Refresh All" to reload all news sections
3. **Theme**: Select Light, Dark, or System from the dropdown

### Features in Action

- **Parallel Loading**: All three categories load simultaneously
- **Independent Updates**: Each section updates as its data arrives
- **Error Recovery**: Use Refresh to retry failed sections

##  Project Structure

```
NewsAggregator/
├── NewsAggregator.csproj        # Project configuration
├── App.xaml                     # Application resources
├── App.xaml.cs                  # Application startup
├── MainWindow.xaml              # Main UI
├── MainWindow.xaml.cs           # UI code-behind
├── appsettings.json             # API configuration (git-ignored)
├── RelayCommand.cs              # RelayCommand for execute and canexecute implements ICommand 
│
├── Models/
│   ├── NewsArticle.cs          # Article data model
│   ├── NewsCategory.cs         # Category enumeration
│   └── ThemeMode.cs            # Theme options
│
├── ViewModels/
│   ├── ViewModelBase.cs        # Base class with INotifyPropertyChanged
│   ├── MainViewModel.cs        # Main window view model
│   └── NewsCategoryViewModel.cs # Category section view model
│
├── Services/
│   ├── INewsService.cs         # Service interface
│   ├── NewsApiService.cs       # NewsAPI implementation
│   ├── ApiConfiguration.cs     # API settings loader
│   └── ThemeService.cs         # Theme management
│
├── Converters/
│   └── BooleanToVisibilityConverter.cs         # UI converters
│   └── InverseBooleanConverter.cs              # UI converters
│   └── InverseBooleanToVisibilityConverter.cs  # UI converters
│
└── Resources/
    └── Styles.xaml             # Custom styles
```

##  Technical Details

### Async/Parallel Programming

The application demonstrates advanced async patterns:

```csharp
public async Task LoadAllNewsAsync()
{
    // Create parallel tasks for each category
    var loadTasks = Categories.Select(category => category.LoadNewsAsync());
    
    // Wait for all to complete
    await Task.WhenAll(loadTasks);
}
```

### MVVM Implementation

- **Data Binding**: Two-way binding between View and ViewModel
- **Commands**: ICommand pattern for user actions
- **Property Notification**: INotifyPropertyChanged for reactive UI

  
### Data Binding Patterns

The application uses appropriate binding modes for different scenarios:

#### Two-Way Binding
- **Theme Selection**: `<ComboBox SelectedValue="{Binding CurrentThemeMode}">` - User can change theme and it updates the ViewModel

#### One-Way Binding (Majority)
- **Display Data**: All news articles, titles, and dates
- **UI States**: Loading indicators, error messages, visibility
- **Collections**: `ItemsSource` bindings for dynamic content

#### Command Bindings
- **Refresh Button**: Uses ICommand pattern instead of event handlers
- **Decoupled Logic**: UI actions trigger ViewModel methods

This demonstrates proper WPF binding practices - using two-way binding only where user input is needed, keeping the UI reactive and the architecture clean.


### Theme System

Supports three modes:
- **Light**: Force light theme
- **Dark**: Force dark theme
- **System**: Follow Windows theme settings

### Error Handling

- Category-specific error messages
- Non-blocking UI during failures
- Graceful degradation

##  API Reference

### INewsService Interface

```csharp
public interface INewsService
{
    Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(NewsCategory category);
}
```

### NewsArticle Model

```csharp
public class NewsArticle
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Source { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
}
```
