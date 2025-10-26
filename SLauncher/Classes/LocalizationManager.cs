using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.Globalization;

namespace SLauncher.Classes
{
    /// <summary>
    /// Manages application localization and language switching
    /// </summary>
    public static class LocalizationManager
    {
        // Event for language changed
        public static event EventHandler LanguageChanged;

        private static Dictionary<string, string> _currentResources = new Dictionary<string, string>();
        private static string _currentLanguage;
        private static string _resourceBasePath;

        /// <summary>
        /// Initialize the localization manager
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Set resource base path (check multiple possible locations)
                _resourceBasePath = FindResourceBasePath();

                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Resource base path: {_resourceBasePath}");

                // Get saved language preference
                _currentLanguage = UserSettingsClass.GetLanguage();

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    // Use system default language
                    _currentLanguage = CultureInfo.CurrentUICulture.Name;

                    // Check if the system language is supported
                    if (!IsSupportedLanguage(_currentLanguage))
                    {
                        _currentLanguage = "en-US"; // Fallback to English
                    }
                }

                ApplyLanguage(_currentLanguage);

                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Localization initialized with language: {_currentLanguage}");
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Loaded {_currentResources.Count} resources");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Error initializing localization: {ex.Message}");
                // Fall back to English
                _currentLanguage = "en-US";
                LoadResourceFile(_currentLanguage);
            }
        }

        /// <summary>
        /// Find the resource base path
        /// </summary>
        private static string FindResourceBasePath()
        {
            // Try multiple possible locations
            var possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings"),
                Path.Combine(Directory.GetCurrentDirectory(), "Strings"),
                Path.Combine(AppContext.BaseDirectory, "Strings")
            };

            foreach (var path in possiblePaths)
            {
                if (Directory.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Found resource path: {path}");
                    return path;
                }
            }

            // Default to base directory
            var defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings");
            System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Using default path: {defaultPath}");
            return defaultPath;
        }

        /// <summary>
        /// Check if a language is supported
        /// </summary>
        private static bool IsSupportedLanguage(string languageCode)
        {
            var supported = new[] { "en-US", "ko-KR", "ja-JP" };
            return Array.Exists(supported, lang => lang.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Load resources from .resw file
        /// </summary>
        private static void LoadResourceFile(string languageCode)
        {
            try
            {
                _currentResources.Clear();

                string resourceFile = Path.Combine(_resourceBasePath, languageCode, "Resources.resw");

                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Attempting to load: {resourceFile}");
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Base path exists: {Directory.Exists(_resourceBasePath)}");

                if (!File.Exists(resourceFile))
                {
                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Resource file not found: {resourceFile}");

                    // List available language folders
                    if (Directory.Exists(_resourceBasePath))
                    {
                        var folders = Directory.GetDirectories(_resourceBasePath);
                        System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Available language folders: {string.Join(", ", folders.Select(Path.GetFileName))}");
                    }

                    // Try fallback to English
                    resourceFile = Path.Combine(_resourceBasePath, "en-US", "Resources.resw");
                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Trying fallback: {resourceFile}");
                }

                if (File.Exists(resourceFile))
                {
                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Loading resource file: {resourceFile}");

                    // Read with explicit UTF-8 encoding
                    string xmlContent = File.ReadAllText(resourceFile, System.Text.Encoding.UTF8);
                    XDocument doc = XDocument.Parse(xmlContent);
                    var dataElements = doc.Descendants("data");

                    int loadedCount = 0;
                    foreach (var element in dataElements)
                    {
                        string name = element.Attribute("name")?.Value;
                        string value = element.Element("value")?.Value;

                        if (!string.IsNullOrEmpty(name) && value != null)
                        {
                            _currentResources[name] = value;
                            loadedCount++;

                            // Debug first few entries
                            if (loadedCount <= 5)
                            {
                                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Loaded: {name} = {value}");
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Successfully loaded {_currentResources.Count} resources from {languageCode}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] ERROR: Resource file not found at {resourceFile}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] ERROR loading resource file: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Get localized string by key
        /// </summary>
        public static string GetString(string key)
        {
            try
            {
                if (_currentResources.ContainsKey(key))
                {
                    return _currentResources[key];
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Resource key '{key}' not found (returning key)");
                    return key; // Return key as fallback
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Error getting string for key '{key}': {ex.Message}");
                return key;
            }
        }

        /// <summary>
        /// Get localized string with format arguments
        /// </summary>
        public static string GetString(string key, params object[] args)
        {
            try
            {
                string format = GetString(key);
                return string.Format(format, args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Error formatting string for key '{key}': {ex.Message}");
                return key;
            }
        }

        /// <summary>
        /// Apply language setting
        /// </summary>
        public static void ApplyLanguage(string languageCode)
        {
            try
            {
                _currentLanguage = languageCode;

                // Load resource file
                LoadResourceFile(languageCode);

                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Language applied: {languageCode}");

                // Raise LanguageChanged event
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Error applying language '{languageCode}': {ex.Message}");
            }
        }

        /// <summary>
        /// Get current language code
        /// </summary>
        public static string GetCurrentLanguage()
        {
            return _currentLanguage ?? "en-US";
        }

        /// <summary>
        /// Get available languages
        /// </summary>
        public static (string Code, string DisplayName)[] GetAvailableLanguages()
        {
            return new[]
            {
                ("en-US", "English"),
                ("ko-KR", "«—±πæÓ"),
                ("ja-JP", "ÏÌ‹‚Âﬁ")
            };
        }

        /// <summary>
        /// Change language and save preference with real-time update
        /// </summary>
        public static void ChangeLanguage(string languageCode)
        {
            ApplyLanguage(languageCode);
            UserSettingsClass.SetLanguage(languageCode);
            UserSettingsClass.WriteSettingsFile();
        }
    }
}
