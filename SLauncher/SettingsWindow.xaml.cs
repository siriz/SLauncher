using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace SLauncher
{
    /// <summary>
    /// Settings window - partial classes split into:
    /// - SettingsWindow.Localization.cs (localization and UI updates)
    /// - SettingsWindow.Cache.cs (cache management)
    /// - SettingsWindow.Hotkey.cs (hotkey configuration)
  /// - SettingsWindow.Settings.cs (toggle switches and settings)
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
          LoadLocalizedXamlElements();

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

      private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close Window
 this.Close();
        }
    }
}


