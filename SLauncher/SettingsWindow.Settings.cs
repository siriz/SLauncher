using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SLauncher
{
    /// <summary>
    /// Settings toggle switches and controls partial class for SettingsWindow
/// </summary>
    public sealed partial class SettingsWindow
    {
   /// <summary>
        /// Handle fullscreen toggle switch
   /// </summary>
        private void FullscreenToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            // Update UserSettingsClass
 UserSettingsClass.UseFullscreen = FullscreenToggleSwitch.IsOn;
    UserSettingsClass.WriteSettingsFile();
  }

        /// <summary>
        /// Handle grid alignment combo box selection change
      /// </summary>
        private void GridAlignComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
     {
    // Save position as "Left" or "Center" (not translated text)
       if (GridAlignComboBox.SelectedIndex == 0)
     {
                UserSettingsClass.GridPosition = "Left";
  }
    else if (GridAlignComboBox.SelectedIndex == 1)
     {
         UserSettingsClass.GridPosition = "Center";
    }
      
            UserSettingsClass.WriteSettingsFile();
}

        /// <summary>
        /// Handle start with Windows toggle switch
   /// </summary>
        private void StartWithWindowsToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
    UserSettingsClass.StartWithWindows = StartWithWindowsToggleSwitch.IsOn;
  UserSettingsClass.WriteSettingsFile();

       // Update Windows registry
       StartupManager.UpdateStartupRegistration(StartWithWindowsToggleSwitch.IsOn);
     }
  }
}
