using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var supported = new[] { "en-US", "ko-KR", "ja-JP", "zh-CN" };
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
                // Load English first so untranslated keys in other languages have a readable fallback.
                LoadResourceEntries("en-US");
                if (!languageCode.Equals("en-US", StringComparison.OrdinalIgnoreCase))
                {
                    LoadResourceEntries(languageCode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] ERROR loading resource file: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void LoadResourceEntries(string languageCode)
        {
            string resourceFile = Path.Combine(_resourceBasePath, languageCode, "Resources.resw");
            if (!File.Exists(resourceFile))
            {
                System.Diagnostics.Debug.WriteLine($"[LocalizationManager] Resource file not found: {resourceFile}");
                return;
            }

            string xmlContent = File.ReadAllText(resourceFile, System.Text.Encoding.UTF8);
            XDocument doc = XDocument.Parse(xmlContent);
            foreach (var element in doc.Descendants("data"))
            {
                string name = element.Attribute("name")?.Value;
                string value = element.Element("value")?.Value;
                if (!string.IsNullOrEmpty(name) && value != null)
                {
                    _currentResources[name] = value;
                }
            }
        }

        // These templates originate in C# interpolated strings. Keep their named tokens and
        // literal \\n in the resource files, but reproduce the original C# display behavior.
        public static string FormatSearchMessage(string key, string query, string error = null)
        {
            return Regex.Replace(GetString(key), @"\\n|\{query\}|\{ex\.Message\}", match =>
            {
                switch (match.Value)
                {
                    case @"\n": return "\n";
                    case "{query}": return query ?? string.Empty;
                    case "{ex.Message}": return error ?? string.Empty;
                    default: return match.Value;
                }
            });
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
                ("ko-KR", "한국어"),
                ("ja-JP", "日本語"),
                ("zh-CN", "简体中文")
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
