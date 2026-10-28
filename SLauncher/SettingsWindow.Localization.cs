using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.WinUI.Controls;
using System;

namespace SLauncher
{
    /// <summary>
    /// Localization management partial class for SettingsWindow
    /// </summary>
    public sealed partial class SettingsWindow
{
        private bool _updatingLocalizedSettings;

        /// <summary>
        /// Load localized text for XAML static elements.
        /// </summary>
        private void LoadLocalizedXamlElements()
        {
            SettingsHeading.Text = LocalizationManager.GetString("SettingsTitle");
            SettingsSubheading.Text = LocalizationManager.GetString("SettingsSubtitle");
            FullscreenSettingsCard.Header = LocalizationManager.GetString("SettingsFullscreenHeader");
            FullscreenSettingsCard.Description = LocalizationManager.GetString("SettingsFullscreenDescription");
            GridAlignmentSettingsCard.Header = LocalizationManager.GetString("SettingsGridAlignmentHeader");
            GridAlignmentSettingsCard.Description = LocalizationManager.GetString("SettingsGridAlignmentDescription");
            StartupSettingsCard.Header = LocalizationManager.GetString("SettingsStartWithWindowsHeader");
            StartupSettingsCard.Description = LocalizationManager.GetString("SettingsStartWithWindowsDescription");
            HotkeySettingsCard.Header = LocalizationManager.GetString("SettingsGlobalHotkeyHeader");
            HotkeySettingsCard.Description = LocalizationManager.GetString("SettingsGlobalHotkeyDescription");
            ToolTipService.SetToolTip(HotkeyButton, LocalizationManager.GetString("HotkeyTooltip"));
            LanguageSettingsCard.Header = LocalizationManager.GetString("SettingsLanguageHeader");
            LanguageSettingsCard.Description = LocalizationManager.GetString("SettingsLanguageDescription");
            BackupHeading.Text = LocalizationManager.GetString("SettingsBackupRestoreTitle");
            BackupSubheading.Text = LocalizationManager.GetString("SettingsBackupRestoreSubtitle");
            ExportSettingsCard.Header = LocalizationManager.GetString("SettingsExportHeader");
            ExportSettingsCard.Description = LocalizationManager.GetString("SettingsExportDescription");
            ExportSettingsButton.Content = LocalizationManager.GetString("SettingsExportButton");
            ImportSettingsCard.Header = LocalizationManager.GetString("SettingsImportHeader");
            ImportSettingsCard.Description = LocalizationManager.GetString("SettingsImportDescription");
            ImportSettingsButton.Content = LocalizationManager.GetString("SettingsImportButton");
            CacheHeading.Text = LocalizationManager.GetString("SettingsCacheManagementTitle");
            CacheSubheading.Text = LocalizationManager.GetString("SettingsCacheManagementSubtitle");
            FaviconSettingsCard.Header = LocalizationManager.GetString("SettingsFaviconCacheHeader");
            FaviconSettingsCard.Description = LocalizationManager.GetString("SettingsFaviconCacheDescription");
            CacheLocationSettingsCard.Header = LocalizationManager.GetString("SettingsCacheLocationHeader");
            CacheLocationSettingsCard.Description = LocalizationManager.GetString("SettingsCacheLocationDescription");
            OpenCacheFolderButton.Content = LocalizationManager.GetString("SettingsOpenFolderButton");
            AboutHeading.Text = LocalizationManager.GetString("SettingsAboutTitle");
            AboutSettingsExpander.Header = LocalizationManager.GetString("SettingsAboutHeader");
            AboutSettingsExpander.Description = LocalizationManager.GetString("SettingsAboutDescription");
        }

  /// <summary>
        /// Update SettingsWindow UI with new language
        /// </summary>
        private void UpdateSettingsWindowUI()
        {
            try
            {
         // Window title
             this.Title = LocalizationManager.GetString("SettingsTitle");
       AppTitleBar.Title = LocalizationManager.GetString("SettingsTitle");

       // Close button
      CloseButton.Content = LocalizationManager.GetString("ButtonClose");

  // Toggle switches
       FullscreenToggleSwitch.OnContent = LocalizationManager.GetString("ToggleSwitchOn");
         FullscreenToggleSwitch.OffContent = LocalizationManager.GetString("ToggleSwitchOff");
      StartWithWindowsToggleSwitch.OnContent = LocalizationManager.GetString("ToggleSwitchOn");
       StartWithWindowsToggleSwitch.OffContent = LocalizationManager.GetString("ToggleSwitchOff");

  // Grid alignment combo box items - Save current setting value (Left/Center)
  string savedPosition = UserSettingsClass.GridPosition;
        _updatingLocalizedSettings = true;
        GridAlignComboBox.Items.Clear();
          GridAlignComboBox.Items.Add(LocalizationManager.GetString("GridAlignmentLeft"));
   GridAlignComboBox.Items.Add(LocalizationManager.GetString("GridAlignmentCenter"));

    // Restore selection based on saved position (not translated text)
   if (savedPosition == "Left")
   {
  GridAlignComboBox.SelectedIndex = 0;
 }
           else if (savedPosition == "Center")
    {
          GridAlignComboBox.SelectedIndex = 1;
  }
       else
  {
          // Default to Left
          GridAlignComboBox.SelectedIndex = 0;
  }

       _updatingLocalizedSettings = false;

       // Cache buttons
         ClearCacheButton.Content = LocalizationManager.GetString("SettingsClearCacheButton");

// Update cache info
           UpdateCacheInfo();

      // Reload all XAML static texts
       LoadLocalizedXamlElements();

         System.Diagnostics.Debug.WriteLine("[SettingsWindow] UI updated with new language");
       }
     catch (Exception ex)
            {
     _updatingLocalizedSettings = false;
     System.Diagnostics.Debug.WriteLine($"[SettingsWindow] Error updating UI: {ex.Message}");
    }
        }

        /// <summary>
        /// Set language selection in ComboBox
        /// </summary>
        private void SetLanguageSelection()
        {
      string currentLanguage = LocalizationManager.GetCurrentLanguage();

       // Find and select the matching ComboBoxItem
            for (int i = 0; i < LanguageComboBox.Items.Count; i++)
         {
      if (LanguageComboBox.Items[i] is ComboBoxItem item &&
             item.Tag?.ToString() == currentLanguage)
                {
          LanguageComboBox.SelectedIndex = i;
   return;
                }
 }

            // Default to English if not found
   LanguageComboBox.SelectedIndex = 0;
        }

        /// <summary>
  /// Handle language selection change
        /// </summary>
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
   if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
string newLanguageCode = selectedItem.Tag?.ToString();

     if (!string.IsNullOrEmpty(newLanguageCode) && newLanguageCode != LocalizationManager.GetCurrentLanguage())
       {
       System.Diagnostics.Debug.WriteLine($"[SettingsWindow] Language changed to: {newLanguageCode}");

         // Save and apply language preference immediately
         LocalizationManager.ChangeLanguage(newLanguageCode);

       // Update settings window UI immediately
    UpdateSettingsWindowUI();

        System.Diagnostics.Debug.WriteLine($"[SettingsWindow] Language applied in real-time");
    }
            }
        }
    }
}
