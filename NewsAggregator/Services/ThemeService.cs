using System.Windows;
using Microsoft.Win32;

namespace NewsAggregator.Services
{
    /// <summary>
    /// Service to handle theme management
    /// </summary>
    public class ThemeService
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryValueName = "AppsUseLightTheme";

        /// <summary>
        /// Gets whether the system is using light theme
        /// </summary>
        public static bool IsSystemLightTheme()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    var value = key?.GetValue(RegistryValueName);
                    if (value != null)
                    {
                        return (int)value == 1;
                    }
                }
            }
            catch
            {
                // Default to dark if we can't read the registry
            }

            return false; // Default to dark theme
        }

        /// <summary>
        /// Applies the specified theme
        /// </summary>
        public static void ApplyTheme(bool isDark)
        {
            var app = Application.Current;
            if (app == null) return;

            var themeUri = new Uri(isDark
                ? "pack://application:,,,/PresentationFramework.Fluent;component/Themes/Fluent.Dark.xaml"
                : "pack://application:,,,/PresentationFramework.Fluent;component/Themes/Fluent.Light.xaml");

            var themeDictionary = new ResourceDictionary { Source = themeUri };

            // Replace the theme dictionary
            if (app.Resources.MergedDictionaries.Count > 0)
            {
                app.Resources.MergedDictionaries.RemoveAt(0);
            }
            app.Resources.MergedDictionaries.Insert(0, themeDictionary);
        }

        /// <summary>
        /// Applies theme based on the mode
        /// </summary>
        public static void ApplyTheme(NewsAggregator.Models.ThemeMode mode)
        {
            bool isDark = mode switch
            {
                NewsAggregator.Models.ThemeMode.Light => false,
                NewsAggregator.Models.ThemeMode.Dark => true,
                NewsAggregator.Models.ThemeMode.System => !IsSystemLightTheme(),
                _ => true
            };

            ApplyTheme(isDark);
        }
    }
}