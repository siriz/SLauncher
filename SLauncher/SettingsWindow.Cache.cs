using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.IO;

namespace SLauncher
{
    /// <summary>
    /// Cache management partial class for SettingsWindow
    /// </summary>
    public sealed partial class SettingsWindow
    {
     /// <summary>
        /// Update cache information display
  /// </summary>
 private void UpdateCacheInfo()
        {
     // Update cache size
            long cacheSize = IconHelpers.GetFaviconCacheSize();
            string sizeText = cacheSize == 0 ? LocalizationManager.GetString("CacheSizeEmpty") :
        cacheSize < 1024 ? $"{cacheSize} bytes" :
      cacheSize < 1024 * 1024 ? $"{cacheSize / 1024:F2} KB" :
     $"{cacheSize / (1024.0 * 1024.0):F2} MB";

    CacheSizeTextBlock.Text = string.Format(LocalizationManager.GetString("SettingsCacheSizeFormat"), sizeText);

        // Update cache location
            CacheLocationTextBlock.Text = IconHelpers.GetFaviconCacheDirectory();
        }

        /// <summary>
        /// Handle clear cache button click
  /// </summary>
        private async void ClearCacheButton_Click(object sender, RoutedEventArgs e)
     {
          // Show confirmation dialog
 ContentDialog confirmDialog = new ContentDialog
         {
        Title = LocalizationManager.GetString("DialogClearCacheTitle"),
        Content = LocalizationManager.GetString("DialogClearCacheContent"),
                PrimaryButtonText = LocalizationManager.GetString("DialogClearCacheConfirm"),
    CloseButtonText = LocalizationManager.GetString("ButtonCancel"),
        DefaultButton = ContentDialogButton.Close,
   XamlRoot = this.Content.XamlRoot
        };

            var result = await confirmDialog.ShowAsync();

 if (result == ContentDialogResult.Primary)
            {
         // Clear cache
          IconHelpers.ClearFaviconCache();

         // Update cache info
      UpdateCacheInfo();

                // Show success message
      ContentDialog successDialog = new ContentDialog
         {
          Title = LocalizationManager.GetString("DialogSuccessTitle"),
    Content = LocalizationManager.GetString("DialogCacheCleared"),
        CloseButtonText = LocalizationManager.GetString("ButtonOK"),
        XamlRoot = this.Content.XamlRoot
       };
       await successDialog.ShowAsync();
     }
        }

      /// <summary>
  /// Handle open cache folder button click
   /// </summary>
        private void OpenCacheFolderButton_Click(object sender, RoutedEventArgs e)
        {
     string cacheDir = IconHelpers.GetFaviconCacheDirectory();

 // Create directory if it doesn't exist
     if (!Directory.Exists(cacheDir))
          {
          Directory.CreateDirectory(cacheDir);
            }

            // Open in Windows Explorer
      Process.Start("explorer.exe", cacheDir);
        }
    }
}
