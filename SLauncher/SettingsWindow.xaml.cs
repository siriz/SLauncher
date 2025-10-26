using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WinUIEx.Messaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SLauncher
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWindow : WinUIEx.WindowEx
    {
        public SettingsWindow()
        {
            this.InitializeComponent();
  
   // Subscribe to language change event
   LocalizationManager.LanguageChanged += OnLanguageChanged;
        }

/// <summary>
   /// Handle language change event
 /// </summary>
private void OnLanguageChanged(object sender, EventArgs e)
        {
       // Update UI on the UI thread
  DispatcherQueue.TryEnqueue(() =>
       {
        UpdateSettingsWindowUI();
     });
 }

      // Event Handlers
        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
 // Set placeholder titlebar for now, before WASDK 1.6
  this.ExtendsContentIntoTitleBar = true;
     this.SetTitleBar(AppTitleBar);

   // Set Window icon
     UIFunctionsClass.SetWindowLauncherXIcon(this);

     // Set Window Background
       UIFunctionsClass.SetWindowBackground(this, ContainerFallbackBackgroundBrush);

// Disable maximise
   UIFunctionsClass.PreventWindowMaximise(this);

       // Initialize UI with localized text
    UpdateSettingsWindowUI();

  // Update the textbox and slider to show correct values
FullscreenToggleSwitch.IsOn = UserSettingsClass.UseFullscreen;
            GridAlignComboBox.SelectedItem = UserSettingsClass.GridPosition;
    
   // Set startup toggle
  StartWithWindowsToggleSwitch.IsOn = UserSettingsClass.StartWithWindows;

      // Update hotkey button text
  UpdateHotkeyButtonText();

// Update cache information
UpdateCacheInfo();

        // Set language selection
        SetLanguageSelection();

    // Create event handlers for the textbox and slider to update settings when their value is changed
   // We only create the event handlers here to prevent them from firing when the window loads
// Since we are updating the settings using these event handlers, if they fire when the window is created, 
      // they will write wrong (blank) values to the UserSettingsClass
  FullscreenToggleSwitch.Toggled += FullscreenToggleSwitch_Toggled;
  GridAlignComboBox.SelectionChanged += GridAlignComboBox_SelectionChanged;

   // Make sure to unsubscribe from the event handlers after
   FullscreenToggleSwitch.Unloaded += (s, e) => FullscreenToggleSwitch.Toggled -= FullscreenToggleSwitch_Toggled;
  GridAlignComboBox.Unloaded += (s, e) => GridAlignComboBox.SelectionChanged -= GridAlignComboBox_SelectionChanged;
   }

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

        private void FullscreenToggleSwitch_Toggled(object sender, RoutedEventArgs e)
      {
  // Update UserSettingsClass
 UserSettingsClass.UseFullscreen = FullscreenToggleSwitch.IsOn;
   UserSettingsClass.WriteSettingsFile();
    }
        
 private void GridAlignComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
 // Update UserSettingsClass
   UserSettingsClass.GridPosition = GridAlignComboBox.SelectedItem.ToString();
 UserSettingsClass.WriteSettingsFile();
        }

   private void StartWithWindowsToggleSwitch_Toggled(object sender, RoutedEventArgs e)
 {
   UserSettingsClass.StartWithWindows = StartWithWindowsToggleSwitch.IsOn;
        UserSettingsClass.WriteSettingsFile();
 
       // Update Windows registry
   StartupManager.UpdateStartupRegistration(StartWithWindowsToggleSwitch.IsOn);
   }

private void UpdateHotkeyButtonText()
 {
 HotkeyButton.Content = UserSettingsClass.GlobalHotkey;
   }

  private async void HotkeyButton_Click(object sender, RoutedEventArgs e)
{
    ContentDialog hotkeyDialog = new ContentDialog
        {
   Title = LocalizationManager.GetString("DialogHotkeyTitle"),
            Content = CreateHotkeyDialogContent(),
  PrimaryButtonText = LocalizationManager.GetString("DialogHotkeySave"),
  CloseButtonText = LocalizationManager.GetString("ButtonCancel"),
DefaultButton = ContentDialogButton.Primary,
  XamlRoot = this.Content.XamlRoot
 };

  var result = await hotkeyDialog.ShowAsync();

 if (result == ContentDialogResult.Primary)
   {
     // Get selected hotkey from UI
   var content = hotkeyDialog.Content as StackPanel;
  var modifierCombo = content.Children[1] as ComboBox;
 var keyCombo = content.Children[3] as ComboBox;

      string modifier = (modifierCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
       string key = (keyCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
  
    string newHotkey = $"{modifier}+{key}";
 
    // Update settings
   UserSettingsClass.GlobalHotkey = newHotkey;
     UserSettingsClass.WriteSettingsFile();

     // Update button text
     UpdateHotkeyButtonText();

      // Show info that app needs restart
  ContentDialog restartDialog = new ContentDialog
     {
    Title = LocalizationManager.GetString("DialogRestartTitle"),
   Content = LocalizationManager.GetString("DialogRestartContent"),
CloseButtonText = LocalizationManager.GetString("ButtonOK"),
      XamlRoot = this.Content.XamlRoot
  };
        await restartDialog.ShowAsync();
    }
 }

        private StackPanel CreateHotkeyDialogContent()
   {
  var panel = new StackPanel { Spacing = 10 };

    // Modifier label
   panel.Children.Add(new TextBlock { Text = LocalizationManager.GetString("HotkeyModifierLabel"), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });

  // Modifier ComboBox
    var modifierCombo = new ComboBox { Width = 200, SelectedIndex = 0 };
  modifierCombo.Items.Add(new ComboBoxItem { Content = "Ctrl" });
modifierCombo.Items.Add(new ComboBoxItem { Content = "Alt" });
 modifierCombo.Items.Add(new ComboBoxItem { Content = "Shift" });
        modifierCombo.Items.Add(new ComboBoxItem { Content = "Ctrl+Shift" });
  modifierCombo.Items.Add(new ComboBoxItem { Content = "Ctrl+Alt" });

  // Set current modifier
       string currentHotkey = UserSettingsClass.GlobalHotkey;
   string[] parts = currentHotkey.Split('+');
 if (parts.Length >= 2)
  {
   string currentModifier = string.Join("+", parts.Take(parts.Length - 1));
      for (int i = 0; i < modifierCombo.Items.Count; i++)
       {
         if ((modifierCombo.Items[i] as ComboBoxItem).Content.ToString() == currentModifier)
      {
      modifierCombo.SelectedIndex = i;
      break;
       }
   }
     }
panel.Children.Add(modifierCombo);

     // Key label
  panel.Children.Add(new TextBlock { Text = LocalizationManager.GetString("HotkeyKeyLabel"), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Margin = new Thickness(0, 10, 0, 0) });

     // Key ComboBox
       var keyCombo = new ComboBox { Width = 200, SelectedIndex = 0 };
    keyCombo.Items.Add(new ComboBoxItem { Content = "Space" });
  keyCombo.Items.Add(new ComboBoxItem { Content = "Tab" });
    keyCombo.Items.Add(new ComboBoxItem { Content = "Enter" });
     keyCombo.Items.Add(new ComboBoxItem { Content = "Esc" });
            keyCombo.Items.Add(new ComboBoxItem { Content = "F1" });
  keyCombo.Items.Add(new ComboBoxItem { Content = "F2" });
     keyCombo.Items.Add(new ComboBoxItem { Content = "F3" });
   keyCombo.Items.Add(new ComboBoxItem { Content = "F4" });

     // Set current key
       if (parts.Length >= 2)
 {
        string currentKey = parts[parts.Length - 1];
 for (int i = 0; i < keyCombo.Items.Count; i++)
      {
      if ((keyCombo.Items[i] as ComboBoxItem).Content.ToString() == currentKey)
     {
    keyCombo.SelectedIndex = i;
      break;
 }
    }
        }
  panel.Children.Add(keyCombo);

 return panel;
   }

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

  // Grid alignment combo box items - Need to recreate the items
    string currentSelection = GridAlignComboBox.SelectedItem?.ToString();
    GridAlignComboBox.Items.Clear();
    GridAlignComboBox.Items.Add(LocalizationManager.GetString("GridAlignmentLeft"));
    GridAlignComboBox.Items.Add(LocalizationManager.GetString("GridAlignmentCenter"));
    
    // Restore selection
    if (currentSelection == "Left" || currentSelection == LocalizationManager.GetString("GridAlignmentLeft"))
    {
        GridAlignComboBox.SelectedIndex = 0;
 }
    else if (currentSelection == "Center" || currentSelection == LocalizationManager.GetString("GridAlignmentCenter"))
    {
        GridAlignComboBox.SelectedIndex = 1;
    }
    else
    {
        // Default to user's saved setting
   GridAlignComboBox.SelectedItem = UserSettingsClass.GridPosition == "Left" ? 
    GridAlignComboBox.Items[0] : GridAlignComboBox.Items[1];
    }

    // Cache buttons
     ClearCacheButton.Content = LocalizationManager.GetString("SettingsClearCacheButton");
     
     // Update cache size text (keeping the actual size, just translating "Cache size:")
     UpdateCacheInfo();
      
    System.Diagnostics.Debug.WriteLine("[SettingsWindow] UI updated with new language");
      }
 catch (Exception ex)
     {
   System.Diagnostics.Debug.WriteLine($"[SettingsWindow] Error updating UI: {ex.Message}");
          }
   }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
  // Close Window
    this.Close();
  }
    }
}


