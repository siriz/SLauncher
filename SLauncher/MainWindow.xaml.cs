using SLauncher.Classes;
using SLauncher.Controls.GridViewItems;
using SLauncher.Controls.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using CommunityToolkit.WinUI;
using System.IO;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SLauncher
{
    /// <summary>
    /// Main window - partial classes split into:
    /// - MainWindow.Tabs.cs (tab management)
    /// - MainWindow.Items.cs (item management)
    /// - MainWindow.DragDrop.cs (drag and drop)
    /// - MainWindow.Search.cs (search functionality)
    /// - MainWindow.UI.cs (UI updates)
    /// - MainWindow.Hotkeys.cs (hotkeys and system tray)
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx, INotifyPropertyChanged
    {
        // Dynamically created controls for icon scale slider
        private Slider IconScaleSlider;
        private TextBlock ScaleValueText;

        // System tray icon
        private SystemTrayIcon trayIcon;

        // Global hotkey manager
        private GlobalHotkeyManager hotkeyManager;

        // Tab management - track previous tab for saving
        private Microsoft.UI.Xaml.Controls.TabViewItem _previousTab;

        // Tab color presets - use English keys that match Resources.resw
        private readonly Dictionary<string, Color> _tabColorPresets = new Dictionary<string, Color>
        {
         { "Default", Color.FromArgb(0, 0, 0, 0) },
     { "Red", Color.FromArgb(80, 255, 69, 58) },
            { "Orange", Color.FromArgb(80, 255, 159, 10) },
            { "Yellow", Color.FromArgb(80, 255, 214, 10) },
        { "Green", Color.FromArgb(80, 48, 209, 88) },
            { "Blue", Color.FromArgb(80, 10, 132, 255) },
      { "Indigo", Color.FromArgb(80, 94, 92, 230) },
    { "Purple", Color.FromArgb(80, 191, 90, 242) },
            { "Pink", Color.FromArgb(80, 255, 55, 95) },
            { "Gray", Color.FromArgb(80, 142, 142, 147) }
        };

        // Dictionary to store tab colors (to persist across selection changes)
        private readonly Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color> _tabColors = new Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Color>();

        // Tabs collection
        private ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();
        public ObservableCollection<TabItem> Tabs
        {
 get => _tabs;
     set
            {
           _tabs = value;
         OnPropertyChanged();
            }
      }

        private TabItem _currentTab;
        public TabItem CurrentTab
 {
         get => _currentTab;
            set
            {
   _currentTab = value;
     OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

        public MainWindow()
  {
            this.InitializeComponent();

       // Initialize tabs - will be replaced by LoadSavedTabs if save exists
     // InitializeTabs();  // Commented out - will be called in Container_Loaded

           // Create a new event handler for when the items in the ItemsGridView have changed (either new items added/removed or items are reset)
      ItemsGridView.Items.VectorChanged += ItemsGridViewItems_VectorChanged;

       // Subscribe to language change event
     LocalizationManager.LanguageChanged += OnLanguageChanged;

          // Workaround for full screen window messing up the taskbar
         // https://github.com/microsoft/microsoft-ui-xaml/issues/8431
       // This property should only be set if the "Automatically hide the taskbar" in Windows 11,
 // or "Automatically hide the taskbar in desktop mode" in Windows 10 is enabled.
            // Setting this property when the setting is disabled will result in the taskbar overlapping the application
     if (Shell32.IsAutoHideTaskbarEnabled())
        Shell32.SetPropW(WinRT.Interop.WindowNative.GetWindowHandle(this), "NonRudeHWND", new IntPtr(1));

         // Used in-tandem with the code in App.xaml.cs, for WinUIEx to save window position: https://github.com/dotMorten/WinUIEx/issues/61
      this.PersistenceId = "SLauncher-250f2258-1995-4edb-9db7-329a61a90a07";
 }

   /// <summary>
   /// Handle language change event - update UI in real-time
  /// </summary>
        private void OnLanguageChanged(object sender, EventArgs e)
{
       // Update UI on the UI thread
  DispatcherQueue.TryEnqueue(() =>
       {
   System.Diagnostics.Debug.WriteLine("[MainWindow] Language changed, updating UI...");
      InitializeLocalizedUI();
         System.Diagnostics.Debug.WriteLine("[MainWindow] UI updated with new language");
      });
  }

  // Event Handlers
        private async void Container_Loaded(object sender, RoutedEventArgs e)
        {
       System.Diagnostics.Debug.WriteLine("DEBUG Container_Loaded START");

        // Run localization diagnostics
          LocalizationDiagnostics.RunDiagnostics();

   // Initialize localized UI text
    InitializeLocalizedUI();

       // Set placeholder titlebar for now, before WASDK 1.6
            this.ExtendsContentIntoTitleBar = true;
this.SetTitleBar(AppTitleBar);

     // Set Window icon
            UIFunctionsClass.SetWindowLauncherXIcon(this);

  // Set Window Background
      UIFunctionsClass.SetWindowBackground(this, ContainerFallbackBackgroundBrush);

            // Always set window to be on top
 this.IsAlwaysOnTop = true;

        // Create icon scale slider dynamically to avoid XAML compiler bug
  CreateIconScaleSlider();

         // Add mouse wheel event handler for Ctrl + Wheel scaling
            ItemsGridView.PointerWheelChanged += ItemsGridView_PointerWheelChanged;
     
       // Initialize system tray icon
            InitializeTrayIcon();
   
        // Hook up window closing event
            this.AppWindow.Closing += Window_Closing;

 // Hook up window closed event (for saving)
    this.Closed += WindowEx_Closed;

            // Initialize global hotkey (Ctrl+Space)
     InitializeGlobalHotkey();

   // Show LoadingDialog while loading items and settings
            LoadingDialog.Visibility = Visibility.Visible;

       // Create settings directories
   UserSettingsClass.CreateSettingsDirectories();
   
   // Sync startup registration with settings
            if (UserSettingsClass.StartWithWindows)
     {
      StartupManager.RegisterStartup();
     }

            // Upgrade settings and write new settings file if necessary
    if (UserSettingsClass.UpgradeRequired())
      {
           UserSettingsClass.UpgradeUserSettings();
                UserSettingsClass.WriteSettingsFile();
        UserSettingsClass.ClearOldTempDirectories();

          // Retrieve user settings from file
           UserSettingsClass.TryReadSettingsFile();

                // Once we have initialised the UserSettingsClass with the correct values, update the UI
      UpdateUIFromSettings();

       // Monitor when the window is resized so that we can adjust the position of the GridView as necesssary
      this.SizeChanged += WindowEx_SizeChanged;

              // Upgrade items as well
    List<Dictionary<string, object>> oldSLauncherItems = await UserSettingsClass.UpgradeOldLauncherXItems();

    foreach (Dictionary<string, object> gridViewTileProps in oldSLauncherItems)
       {
      string executingPath = gridViewTileProps["ExecutingPath"] as string;
       string executingArguments = gridViewTileProps["ExecutingArguments"] as string;
            string displayText = gridViewTileProps["DisplayText"] as string;
       BitmapImage imageSource = gridViewTileProps["ImageSource"] as BitmapImage;
  AddGridViewTile(executingPath, executingArguments, displayText, imageSource);
         }

         // Save items after
        UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);
 }
   else
            {
      // Retrieve user settings from file
    UserSettingsClass.TryReadSettingsFile();

        // Once we have initialised the UserSettingsClass with the correct values, update the UI
     UpdateUIFromSettings();

        // Monitor when the window is resized so that we can adjust the position of the GridView as necesssary
                this.SizeChanged += WindowEx_SizeChanged;

                // Load SLauncher items as normal
    List<UserControl> controls = await UserSettingsClass.LoadLauncherXItems();

      // Try to load saved tabs
     var tabsData = UserSettingsClass.LoadTabs();
      if (tabsData != null && tabsData.Tabs.Count > 0)
      {
   // Load tabs from saved data
        LoadSavedTabs(controls);
        }
         else
         {
     // No saved tabs, use default tab and load all items
               System.Diagnostics.Debug.WriteLine("No saved tabs found, creating default tab");
       InitializeTabs();
  System.Diagnostics.Debug.WriteLine($"After InitializeTabs: MainTabView.TabItems.Count = {MainTabView.TabItems.Count}");
       
        DeserialiseListToGridViewItems(controls);
     System.Diagnostics.Debug.WriteLine($"After DeserialiseListToGridViewItems: ItemsGridView.Items.Count = {ItemsGridView.Items.Count}");

     // ? IMPORTANT: Save loaded items to the default tab's Tag
                SaveCurrentTabItems();
          
     if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem defaultTab)
    {
     System.Diagnostics.Debug.WriteLine($"Default tab Tag after SaveCurrentTabItems: {(defaultTab.Tag as List<UserControl>)?.Count ?? -1} items");
 }
  }
            }

         // Check if there were errors adding files
          if (UserSettingsClass.ErrorAddingItems() == true)
       {
     AddItemsErrorWindow addItemsErrorWindow = new AddItemsErrorWindow();

   foreach (string path in UserSettingsClass.ErrorPaths)
        {
         addItemsErrorWindow.Items.Add(path);
                }

 UIFunctionsClass.CreateModalWindow(addItemsErrorWindow, this);
            }

    // Hide LoadingDialog once done
         await Task.Delay(20);
          LoadingDialog.Visibility = Visibility.Collapsed;

      // Set focus to SearchBox
            SearchBox.Focus(FocusState.Programmatic);
      }

        // The last event handler - save items when the window is closed
        private void WindowEx_Closed(object sender, WindowEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("===========================================");
   System.Diagnostics.Debug.WriteLine("DEBUG: WindowEx_Closed - Starting save process");
            System.Diagnostics.Debug.WriteLine($"Current time: {System.DateTime.Now}");
        System.Diagnostics.Debug.WriteLine($"MainTabView.TabItems.Count: {MainTabView?.TabItems?.Count ?? -1}");
        System.Diagnostics.Debug.WriteLine($"ItemsGridView.Items.Count: {ItemsGridView?.Items?.Count ?? -1}");
    
            // Save all tabs (this will also save all items)
     SaveAllTabs();
         
    // Note: SaveAllTabs now handles saving items internally
        // UserSettingsClass.SaveLauncherXItems is called inside SaveAllTabs

         System.Diagnostics.Debug.WriteLine("DEBUG: WindowEx_Closed - Save complete");
  
            // Verify tabs.json was created
    string tabsFilePath = System.IO.Path.Combine(UserSettingsClass.SettingsDir, "tabs.json");
  if (System.IO.File.Exists(tabsFilePath))
      {
             System.Diagnostics.Debug.WriteLine($"? tabs.json EXISTS at: {tabsFilePath}");
     try
        {
           string content = System.IO.File.ReadAllText(tabsFilePath);
 System.Diagnostics.Debug.WriteLine($"tabs.json content:\n{content}");
     }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error reading tabs.json: {ex.Message}");
}
          }
   else
            {
System.Diagnostics.Debug.WriteLine($"? tabs.json DOES NOT EXIST at: {tabsFilePath}");
 }
        System.Diagnostics.Debug.WriteLine("===========================================");

       // Dispose resources
       trayIcon?.Dispose();
            hotkeyManager?.Dispose();
        }
    }
}


