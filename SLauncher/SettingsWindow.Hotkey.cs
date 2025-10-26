using SLauncher.Classes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace SLauncher
{
    /// <summary>
    /// Hotkey configuration partial class for SettingsWindow
    /// </summary>
    public sealed partial class SettingsWindow
    {
     /// <summary>
      /// Update hotkey button text
    /// </summary>
        private void UpdateHotkeyButtonText()
        {
            HotkeyButton.Content = UserSettingsClass.GlobalHotkey;
    }

        /// <summary>
        /// Handle hotkey button click
        /// </summary>
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

    /// <summary>
      /// Create hotkey dialog content
        /// </summary>
        private StackPanel CreateHotkeyDialogContent()
        {
         var panel = new StackPanel { Spacing = 10 };

     // Modifier label
            panel.Children.Add(new TextBlock
     {
     Text = LocalizationManager.GetString("HotkeyModifierLabel"),
      FontWeight = Microsoft.UI.Text.FontWeights.SemiBold
  });

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
  panel.Children.Add(new TextBlock
    {
      Text = LocalizationManager.GetString("HotkeyKeyLabel"),
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
 Margin = new Thickness(0, 10, 0, 0)
  });

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
    }
}
