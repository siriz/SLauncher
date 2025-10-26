using Microsoft.UI.Xaml;
using SLauncher.Classes;
using System;
using System.IO;

namespace SLauncher
{
    /// <summary>
    /// Hotkeys and system tray partial class for MainWindow
    /// </summary>
    public sealed partial class MainWindow
    {
        /// <summary>
        /// Initialize system tray icon
        /// </summary>
        private void InitializeTrayIcon()
        {
try
   {
     var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icon.ico");
          var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

      trayIcon = new SystemTrayIcon(hwnd, iconPath, "SLauncher - Double-click to open");

    // Set left click handler (double-click)
     trayIcon.SetOnLeftClick(() =>
    {
   this.DispatcherQueue.TryEnqueue(() =>
     {
           this.AppWindow.Show();
  this.Activate();
   
           // Set focus to SearchBox
  SearchBox.Focus(FocusState.Programmatic);
      });
 });

// Set context menu handlers
     trayIcon.SetOnOpenMenu(() =>
         {
     this.DispatcherQueue.TryEnqueue(() =>
 {
          this.AppWindow.Show();
              this.Activate();
   
    // Set focus to SearchBox
          SearchBox.Focus(FocusState.Programmatic);
    });
   });

    trayIcon.SetOnSettingsMenu(() =>
  {
   this.DispatcherQueue.TryEnqueue(() =>
{
         this.AppWindow.Show();
  this.Activate();
  
 // Open settings window
          var settingsWindow = new SettingsWindow();
     UIFunctionsClass.CreateModalWindow(settingsWindow, this);
     settingsWindow.Closed += (s, e) => UpdateUIFromSettings();
          });
    });

            trayIcon.SetOnExitMenu(() =>
    {
       this.DispatcherQueue.TryEnqueue(() =>
       {
     // Save all tabs before exit
     SaveAllTabs();
  
       // Dispose tray icon
  trayIcon?.Dispose();
 hotkeyManager?.Dispose();
    
       // Exit application
      Microsoft.UI.Xaml.Application.Current.Exit();
      });
    });
    }
catch (Exception ex)
   {
  System.Diagnostics.Debug.WriteLine($"Error initializing tray icon: {ex}");
            }
        }

        /// <summary>
        /// Handle window closing - minimize to tray if enabled
/// </summary>
        private void Window_Closing(object sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
  {
  if (UserSettingsClass.MinimizeToTray)
{
  // Just minimize to tray, don't save yet (will save on actual close)
        args.Cancel = true;
       this.AppWindow.Hide();
  }
  // else: allow window to close, WindowEx_Closed will handle saving
        }

        /// <summary>
        /// Initialize global hotkey (Ctrl+Space)
        /// </summary>
 private void InitializeGlobalHotkey()
  {
try
    {
    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
    hotkeyManager = new GlobalHotkeyManager(hwnd);

   // Register hotkey from settings
             bool success = hotkeyManager.RegisterHotkey(UserSettingsClass.GlobalHotkey, () =>
     {
    this.DispatcherQueue.TryEnqueue(() =>
       {
    ToggleWindowVisibility();
        });
   });

     if (!success)
     {
  System.Diagnostics.Debug.WriteLine($"Warning: Failed to register {UserSettingsClass.GlobalHotkey} hotkey. It may already be in use by another application.");
  }
     }
    catch (Exception ex)
        {
     System.Diagnostics.Debug.WriteLine($"Error initializing global hotkey: {ex}");
        }
        }

        /// <summary>
        /// Toggle window visibility (show/hide)
        /// </summary>
        private void ToggleWindowVisibility()
   {
   try
            {
      // Check if window is visible
     bool isVisible = this.AppWindow.IsVisible;

   if (isVisible)
       {
    // Window is visible, hide it
       System.Diagnostics.Debug.WriteLine("Ctrl+Space: Hiding window");
    this.AppWindow.Hide();
      }
    else
    {
// Window is hidden, show it
  System.Diagnostics.Debug.WriteLine("Ctrl+Space: Showing window");
    this.AppWindow.Show();
   this.Activate();
   
        // Set focus to SearchBox
    SearchBox.Focus(FocusState.Programmatic);
     }
     }
     catch (Exception ex)
            {
            System.Diagnostics.Debug.WriteLine($"Error toggling window visibility: {ex}");
     }
  }
    }
}
