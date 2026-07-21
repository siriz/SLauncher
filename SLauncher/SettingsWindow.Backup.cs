using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace SLauncher
{
    /// <summary>
    /// Backup & restore (export/import settings) partial class for SettingsWindow
    /// </summary>
    public sealed partial class SettingsWindow
    {
        /// <summary>
        /// Handle export settings button click
        /// </summary>
        private async void ExportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "SLauncher Settings (*.slauncherbackup)|*.slauncherbackup",
                FileName = $"SLauncher-Settings-{DateTime.Now:yyyy-MM-dd}.slauncherbackup"
            };

            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            try
            {
                UserSettingsClass.ExportSettings(saveFileDialog.FileName);

                ContentDialog successDialog = new ContentDialog
                {
                    Title = LocalizationManager.GetString("DialogSuccessTitle"),
                    Content = LocalizationManager.GetString("DialogExportSuccessContent"),
                    CloseButtonText = LocalizationManager.GetString("ButtonOK"),
                    XamlRoot = this.Content.XamlRoot
                };
                await successDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = LocalizationManager.GetString("DialogExportErrorTitle"),
                    Content = $"{ex.Message}",
                    CloseButtonText = LocalizationManager.GetString("ButtonOK"),
                    XamlRoot = this.Content.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Handle import settings button click
        /// </summary>
        private async void ImportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "SLauncher Settings (*.slauncherbackup)|*.slauncherbackup|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            ContentDialog confirmDialog = new ContentDialog
            {
                Title = LocalizationManager.GetString("DialogImportConfirmTitle"),
                Content = LocalizationManager.GetString("DialogImportConfirmContent"),
                PrimaryButtonText = LocalizationManager.GetString("ButtonImport"),
                CloseButtonText = LocalizationManager.GetString("ButtonCancel"),
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };

            ContentDialogResult confirmResult = await confirmDialog.ShowAsync();
            if (confirmResult != ContentDialogResult.Primary)
            {
                return;
            }

            try
            {
                UserSettingsClass.ImportSettings(openFileDialog.FileName);

                // Settings/items/tabs are only read once at startup, so the user needs to
                // restart SLauncher themselves for the imported data to take effect
                // (same approach as changing the global hotkey)
                ContentDialog restartDialog = new ContentDialog
                {
                    Title = LocalizationManager.GetString("DialogImportSuccessTitle"),
                    Content = LocalizationManager.GetString("DialogImportSuccessContent"),
                    CloseButtonText = LocalizationManager.GetString("ButtonOK"),
                    XamlRoot = this.Content.XamlRoot
                };
                await restartDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = LocalizationManager.GetString("DialogImportErrorTitle"),
                    Content = $"{ex.Message}",
                    CloseButtonText = LocalizationManager.GetString("ButtonOK"),
                    XamlRoot = this.Content.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}
