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
        /// <summary>
        /// Load localized text for XAML static elements
    /// </summary>
        private void LoadLocalizedXamlElements()
        {
    try
   {
        // Find TextBlocks by traversing the visual tree
         var scrollViewer = Container.Children[1] as ScrollViewer;
          if (scrollViewer?.Content is StackPanel mainPanel)
    {
      // Settings title and subtitle
           if (mainPanel.Children[0] is TextBlock settingsTitle)
           {
   settingsTitle.Text = LocalizationManager.GetString("SettingsTitle");
              }
  if (mainPanel.Children[1] is TextBlock settingsSubtitle)
        {
       settingsSubtitle.Text = LocalizationManager.GetString("SettingsSubtitle");
      }

         // Find and update SettingsCard elements
                    for (int i = 0; i < mainPanel.Children.Count; i++)
         {
         var child = mainPanel.Children[i];

               if (child is SettingsCard settingsCard)
            {
             // Update based on the card's current header
             string currentHeader = settingsCard.Header?.ToString() ?? "";

     if (currentHeader.Contains("fullscreen") || currentHeader.Contains("全画面") || currentHeader.Contains("전체"))
             {
            settingsCard.Header = LocalizationManager.GetString("SettingsFullscreenHeader");
      settingsCard.Description = LocalizationManager.GetString("SettingsFullscreenDescription");
  }
        else if (currentHeader.Contains("Grid") || currentHeader.Contains("グリッド") || currentHeader.Contains("그리드"))
    {
            settingsCard.Header = LocalizationManager.GetString("SettingsGridAlignmentHeader");
        settingsCard.Description = LocalizationManager.GetString("SettingsGridAlignmentDescription");
       }
    else if (currentHeader.Contains("Start") || currentHeader.Contains("起動") || currentHeader.Contains("시작"))
 {
             settingsCard.Header = LocalizationManager.GetString("SettingsStartWithWindowsHeader");
    settingsCard.Description = LocalizationManager.GetString("SettingsStartWithWindowsDescription");
 }
           else if (currentHeader.Contains("Hotkey") || currentHeader.Contains("ホットキー") || currentHeader.Contains("단축키"))
  {
     settingsCard.Header = LocalizationManager.GetString("SettingsGlobalHotkeyHeader");
       settingsCard.Description = LocalizationManager.GetString("SettingsGlobalHotkeyDescription");

     // Update tooltip
      if (settingsCard.Content is Button hotkeyBtn)
             {
      Microsoft.UI.Xaml.Controls.ToolTipService.SetToolTip(hotkeyBtn, LocalizationManager.GetString("HotkeyTooltip"));
    }
        }
   else if (currentHeader.Contains("Language") || currentHeader.Contains("言語") || currentHeader.Contains("언어"))
       {
      settingsCard.Header = LocalizationManager.GetString("SettingsLanguageHeader");
  settingsCard.Description = LocalizationManager.GetString("SettingsLanguageDescription");
}
       else if (currentHeader.Contains("Favicon") || currentHeader.Contains("ファビコン") || currentHeader.Contains("파비콘"))
       {
   settingsCard.Header = LocalizationManager.GetString("SettingsFaviconCacheHeader");
          settingsCard.Description = LocalizationManager.GetString("SettingsFaviconCacheDescription");
        }
         else if (currentHeader.Contains("Location") || currentHeader.Contains("場所") || currentHeader.Contains("위치"))
    {
           settingsCard.Header = LocalizationManager.GetString("SettingsCacheLocationHeader");
      settingsCard.Description = LocalizationManager.GetString("SettingsCacheLocationDescription");

        // Update "Open Folder" button
   if (settingsCard.Content is StackPanel sp && sp.Children.Count > 1 && sp.Children[1] is Button openFolderBtn)
           {
           openFolderBtn.Content = LocalizationManager.GetString("SettingsOpenFolderButton");
                 }
          }
       else if (currentHeader.Contains("Export") || currentHeader.Contains("エクスポート") || currentHeader.Contains("내보내기"))
       {
           settingsCard.Header = LocalizationManager.GetString("SettingsExportHeader");
           settingsCard.Description = LocalizationManager.GetString("SettingsExportDescription");

           if (settingsCard.Content is Button exportBtn)
           {
               exportBtn.Content = LocalizationManager.GetString("SettingsExportButton");
           }
       }
       else if (currentHeader.Contains("Import") || currentHeader.Contains("インポート") || currentHeader.Contains("가져오기"))
       {
           settingsCard.Header = LocalizationManager.GetString("SettingsImportHeader");
           settingsCard.Description = LocalizationManager.GetString("SettingsImportDescription");

           if (settingsCard.Content is Button importBtn)
           {
               importBtn.Content = LocalizationManager.GetString("SettingsImportButton");
           }
       }
}
         else if (child is TextBlock textBlock)
      {
   // Cache Management title and subtitle
        if (textBlock.Text.Contains("Cache") || textBlock.Text.Contains("キャッシュ") || textBlock.Text.Contains("캐시") ||
      textBlock.Text.Contains("Website") || textBlock.Text.Contains("ウェブサイト") || textBlock.Text.Contains("웹사이트"))
     {
     if (textBlock.FontSize == 20) // Title
      {
  textBlock.Text = LocalizationManager.GetString("SettingsCacheManagementTitle");
     }
    else if (textBlock.FontSize == 13) // Subtitle
    {
        textBlock.Text = LocalizationManager.GetString("SettingsCacheManagementSubtitle");
       }
  }
       // About title
        else if (textBlock.Text.Contains("About") || textBlock.Text.Contains("について") || textBlock.Text.Contains("정보"))
      {
         textBlock.Text = LocalizationManager.GetString("SettingsAboutTitle");
    }
       // Backup & Restore title and subtitle
       else if (textBlock.Text.Contains("Backup") || textBlock.Text.Contains("バックアップ") || textBlock.Text.Contains("백업") ||
                textBlock.Text.Contains("per-user") || textBlock.Text.Contains("実行ファイル") || textBlock.Text.Contains("실행 파일"))
       {
           if (textBlock.FontSize == 20) // Title
           {
               textBlock.Text = LocalizationManager.GetString("SettingsBackupRestoreTitle");
           }
           else if (textBlock.FontSize == 13) // Subtitle
           {
               textBlock.Text = LocalizationManager.GetString("SettingsBackupRestoreSubtitle");
           }
       }
               }
      else if (child is SettingsExpander expander)
            {
      // About SLauncher expander
     if (expander.Header?.ToString().Contains("About") == true ||
        expander.Header?.ToString().Contains("について") == true ||
        expander.Header?.ToString().Contains("정보") == true)
        {
     expander.Header = LocalizationManager.GetString("SettingsAboutHeader");
     expander.Description = LocalizationManager.GetString("SettingsAboutDescription");
}
         }
                }
 }
                System.Diagnostics.Debug.WriteLine("[SettingsWindow] Localized XAML elements loaded");
            }
  catch (Exception ex)
     {
     System.Diagnostics.Debug.WriteLine($"[SettingsWindow] Error loading localized XAML: {ex.Message}");
            }
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

     if (!string.IsNullOrEmpty(newLanguageCode))
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
