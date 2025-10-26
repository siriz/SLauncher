using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SLauncher.Classes;
using SLauncher.Controls.GridViewItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;

namespace SLauncher
{
    /// <summary>
    /// Tab management partial class for MainWindow
    /// </summary>
    public sealed partial class MainWindow
    {
   // Tab management methods
 
        /// <summary>
  /// Initialize tabs - create default tab if none exist
  /// </summary>
        private void InitializeTabs()
  {
            // Create default tab
      var defaultTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
        defaultTab.Header = LocalizationManager.GetString("DefaultTabName");

      // Initialize Tag with empty list
defaultTab.Tag = new List<UserControl>();
      
  // Set transparent background for selected tab
  defaultTab.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
  
   // Add context menu to tab
  AttachTabContextMenu(defaultTab);
   
   MainTabView.TabItems.Add(defaultTab);
  MainTabView.SelectedItem = defaultTab;
        
 // Set as previous tab
  _previousTab = defaultTab;
        }

/// <summary>
        /// Handle adding a new tab
  /// </summary>
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
      {
     // Save current tab items before creating new tab
   SaveCurrentTabItems();
  
    // Create new tab
 var newTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
 newTab.Header = LocalizationManager.GetString("NewTabName", MainTabView.TabItems.Count + 1);

  // Initialize Tag with empty list
  newTab.Tag = new List<UserControl>();

  // Set light gray background for unselected tab
  newTab.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 238, 238, 238)); // #EEEEEE

// Add context menu to tab
  AttachTabContextMenu(newTab);
   
    // Add to TabView
   MainTabView.TabItems.Add(newTab);
   MainTabView.SelectedItem = newTab;
 
        // Clear items for new tab (already empty, but for consistency)
   ItemsGridView.Items.Clear();
     }

        /// <summary>
        /// Attach context menu to a tab
        /// </summary>
        private void AttachTabContextMenu(Microsoft.UI.Xaml.Controls.TabViewItem tab)
        {
  var contextMenu = new MenuFlyout();
            
 // Rename menu item
     var renameItem = new MenuFlyoutItem
 {
   Text = LocalizationManager.GetString("TabRename"),
      Icon = new SymbolIcon(Symbol.Rename)
  };
   renameItem.Click += (s, e) => RenameTab_Click(tab);
  contextMenu.Items.Add(renameItem);
      
   // Color submenu
  var colorItem = new MenuFlyoutSubItem
       {
  Text = LocalizationManager.GetString("TabChangeColor"),
      Icon = new SymbolIcon(Symbol.FontColor)
    };
 
 // Add color presets
     foreach (var colorPreset in _tabColorPresets)
    {
    // Create menu item with localized color name
         var colorMenuItem = new MenuFlyoutItem
    {
   Text = $"  {LocalizationManager.GetString($"Color{colorPreset.Key}")}"
    };
  
          colorMenuItem.Click += (s, e) => ChangeTabColor_Click(tab, colorPreset.Value);
            
       // Use a filled circle icon as preview (Segoe MDL2 Assets)
   var colorIcon = new FontIcon
         {
   Glyph = "\uE91F",  // CircleShapeSolid - filled circle
  Foreground = new SolidColorBrush(colorPreset.Value),
    FontSize = 14,
   FontFamily = new FontFamily("Segoe MDL2 Assets")
       };
     colorMenuItem.Icon = colorIcon;
          
    colorItem.Items.Add(colorMenuItem);
      }
 
       contextMenu.Items.Add(colorItem);

   // Separator
   contextMenu.Items.Add(new MenuFlyoutSeparator());
            
 // Delete menu item
   var deleteItem = new MenuFlyoutItem
     {
           Text = LocalizationManager.GetString("TabDelete"),
       Icon = new SymbolIcon(Symbol.Delete)
       };
     deleteItem.Click += (s, e) => DeleteTab_Click(tab);
         contextMenu.Items.Add(deleteItem);
     
          tab.ContextFlyout = contextMenu;
  }

    /// <summary>
        /// Handle tab color change
     /// </summary>
        private void ChangeTabColor_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
 {
      // Store the color in the dictionary
      _tabColors[tab] = color;
            
          // Apply the color with a separator for visual distinction
            UpdateTabColorSeparator(tab, color);
  }

        /// <summary>
  /// Update tab color with a colored circle instead of border
     /// </summary>
        private void UpdateTabColorSeparator(Microsoft.UI.Xaml.Controls.TabViewItem tab, Color color)
        {
   // Remove border
   tab.BorderBrush = null;
         tab.BorderThickness = new Thickness(0);
     
   // Use a solid filled circle Ellipse as icon
   var ellipse = new Microsoft.UI.Xaml.Shapes.Ellipse
   {
       Width = 12,
       Height = 12,
       Fill = new SolidColorBrush(color),
       Stretch = Stretch.UniformToFill
   };
   
   // Create a Grid to center the ellipse
   var grid = new Grid
   {
       Width = 16,
       Height = 16
   };
   grid.Children.Add(ellipse);
   
   // Create IconSource from the visual element
var iconSource = new Microsoft.UI.Xaml.Controls.BitmapIconSource();
   
   // Alternative: Use FontIcon with a proper filled circle
   tab.IconSource = new Microsoft.UI.Xaml.Controls.FontIconSource
   {
    Glyph = "\uE91F",  // Segoe MDL2 Assets: CircleShapeSolid
       Foreground = new SolidColorBrush(color),
       FontSize = 12,
       FontFamily = new FontFamily("Segoe MDL2 Assets")
   };
        }

        /// <summary>
        /// Handle tab rename
        /// </summary>
        private async void RenameTab_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab)
     {
        var textBox = new TextBox
       {
 Text = tab.Header?.ToString() ?? "",
 PlaceholderText = LocalizationManager.GetString("TabRenamePlaceholder"),
      Width = 250
   };
 
 var dialog = new ContentDialog
     {
        Title = LocalizationManager.GetString("TabRenameTitle"),
       Content = textBox,
  PrimaryButtonText = LocalizationManager.GetString("ButtonOK"),
 CloseButtonText = LocalizationManager.GetString("ButtonCancel"),
  DefaultButton = ContentDialogButton.Primary,
XamlRoot = this.Content.XamlRoot
       };
  
  var result = await dialog.ShowAsync();
 
    if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(textBox.Text))
       {
 tab.Header = textBox.Text;
        }
        }

        /// <summary>
        /// Handle tab delete
        /// </summary>
      private async void DeleteTab_Click(Microsoft.UI.Xaml.Controls.TabViewItem tab)
     {
     // Don't allow deleting the last tab
    if (MainTabView.TabItems.Count <= 1)
  {
 var errorDialog = new ContentDialog
 {
      Title = LocalizationManager.GetString("TabCannotDeleteTitle"),
       Content = LocalizationManager.GetString("TabCannotDeleteMessage"),
            CloseButtonText = LocalizationManager.GetString("ButtonOK"),
       XamlRoot = this.Content.XamlRoot
          };
    await errorDialog.ShowAsync();
        return;
    }
    
      // Count items in the tab
 int itemCount = 0;
        if (tab.Tag is List<UserControl> items)
 {
     itemCount = items.Count;
   }
    else if (ReferenceEquals(tab, MainTabView.SelectedItem))
{
    // If this is the current tab, count from ItemsGridView
  itemCount = ItemsGridView.Items.Count;
    }
    
   // Show confirmation dialog
      string message = itemCount > 0
         ? LocalizationManager.GetString("TabDeleteWithItemsMessage", itemCount)
       : LocalizationManager.GetString("TabDeleteMessage");
     
    var confirmDialog = new ContentDialog
      {
  Title = LocalizationManager.GetString("TabDeleteTitle"),
     Content = message,
  PrimaryButtonText = LocalizationManager.GetString("ButtonDelete"),
        CloseButtonText = LocalizationManager.GetString("ButtonCancel"),
 DefaultButton = ContentDialogButton.Close,
       XamlRoot = this.Content.XamlRoot
  };
     
  var result = await confirmDialog.ShowAsync();
   
      if (result == ContentDialogResult.Primary)
    {
// If deleting current tab, save it first
    if (ReferenceEquals(tab, MainTabView.SelectedItem))
      {
  SaveCurrentTabItems();
 }
    
      // Remove tab color from dictionary
if (_tabColors.ContainsKey(tab))
     {
     _tabColors.Remove(tab);
      }
   
           // Remove the tab
   MainTabView.TabItems.Remove(tab);
      
      // Update previous tab reference
   if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
   {
    _previousTab = selectedTab;
   
   // Restore selected tab's color
           if (_tabColors.ContainsKey(selectedTab))
{
          UpdateTabColorSeparator(selectedTab, _tabColors[selectedTab]);
       }
     }
       }
   }

        /// <summary>
        /// Handle tab selection changed
        /// </summary>
        private void MainTabView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
      {
      // Save items from the PREVIOUS tab (before selection changed)
 if (_previousTab != null)
    {
  var items = new List<UserControl>();
  foreach (var item in ItemsGridView.Items)
   {
      if (item is UserControl control)
      {
     items.Add(control);
    }
}
       _previousTab.Tag = items;
     
      // Set light gray background for unselected tab
      _previousTab.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 238, 238, 238)); // #EEEEEE
  
      // Restore previous tab's color if it had one
if (_tabColors.ContainsKey(_previousTab))
      {
     UpdateTabColorSeparator(_previousTab, _tabColors[_previousTab]);
       }
    }
 
     // Update previous tab reference
     if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem newTab)
          {
     _previousTab = newTab;
  
       // Clear selected tab's background to show it's selected
      newTab.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
     
       // Restore newly selected tab's color if it has one (full brightness)
      if (_tabColors.ContainsKey(newTab))
       {
        UpdateTabColorSeparator(newTab, _tabColors[newTab]);
    }
      }
      
 // Load items for newly selected tab
        LoadCurrentTabItems();
        }
        /// <summary>
        /// Handle tab close request
        /// </summary>
     private async void MainTabView_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
     {
   // Don't allow closing the last tab
 if (MainTabView.TabItems.Count <= 1)
    {
     var errorDialog = new ContentDialog
  {
 Title = LocalizationManager.GetString("TabCannotDeleteTitle"),
       Content = LocalizationManager.GetString("TabCannotDeleteMessage"),
      CloseButtonText = LocalizationManager.GetString("ButtonOK"),
       XamlRoot = this.Content.XamlRoot
  };
   await errorDialog.ShowAsync();
  return;
     }
    
       // Count items in the tab
 int itemCount = 0;
        if (args.Tab.Tag is List<UserControl> items)
            {
     itemCount = items.Count;
   }
    else if (ReferenceEquals(args.Tab, MainTabView.SelectedItem))
{
    // If this is the current tab, count from ItemsGridView
  itemCount = ItemsGridView.Items.Count;
    }
 
      // Show confirmation dialog
  string message = itemCount > 0
  ? LocalizationManager.GetString("TabDeleteWithItemsMessage", itemCount)
       : LocalizationManager.GetString("TabDeleteMessage");
    
          var confirmDialog = new ContentDialog
         {
  Title = LocalizationManager.GetString("TabDeleteTitle"),
     Content = message,
  PrimaryButtonText = LocalizationManager.GetString("ButtonDelete"),
        CloseButtonText = LocalizationManager.GetString("ButtonCancel"),
 DefaultButton = ContentDialogButton.Close,
  XamlRoot = this.Content.XamlRoot
  };
      
     var result = await confirmDialog.ShowAsync();
   
  if (result != ContentDialogResult.Primary)
         {
   // User cancelled
     return;
   }
  
         // Save current tab before closing
          SaveCurrentTabItems();

         // Remove tab color from dictionary
    if (_tabColors.ContainsKey(args.Tab))
       {
     _tabColors.Remove(args.Tab);
          }

         // Remove the tab
            MainTabView.TabItems.Remove(args.Tab);

 // Update previous tab reference
  if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
          {
    _previousTab = selectedTab;
        
// Restore selected tab's color
 if (_tabColors.ContainsKey(selectedTab))
    {
    UpdateTabColorSeparator(selectedTab, _tabColors[selectedTab]);
      }
  }
        }

        /// <summary>
        /// Save current ItemsGridView items to current tab's Tag
        /// </summary>
        private void SaveCurrentTabItems()
        {
System.Diagnostics.Debug.WriteLine("SaveCurrentTabItems called");
   
       if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem currentTab)
      {
        System.Diagnostics.Debug.WriteLine($"Current tab: {currentTab.Header}");
        System.Diagnostics.Debug.WriteLine($"ItemsGridView.Items.Count: {ItemsGridView.Items.Count}");
        
        var items = new List<UserControl>();
   foreach (var item in ItemsGridView.Items)
   {
      if (item is UserControl control)
{
                items.Add(control);
  }
      }
      
     System.Diagnostics.Debug.WriteLine($"Saving {items.Count} items to tab.Tag");
      currentTab.Tag = items;
   }
   else
   {
       System.Diagnostics.Debug.WriteLine("ERROR: MainTabView.SelectedItem is NOT a TabViewItem!");
   }
      }

        /// <summary>
        /// Load items from current tab's Tag to ItemsGridView
        /// </summary>
    private void LoadCurrentTabItems()
   {
       ItemsGridView.Items.Clear();
            
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem currentTab)
            {
 if (currentTab.Tag is List<UserControl> items)
   {
       foreach (var item in items)
    {
  if (item is GridViewTile tile)
     {
   tile.Drop += GridViewTile_Drop;
        tile.DragEnter += GridViewTile_DragEnter;
            tile.DragLeave += GridViewTile_DragLeave;
              ItemsGridView.Items.Add(tile);
        }
          else if (item is GridViewTileGroup group)
          {
       group.DragEnter += GridViewTileGroup_DragEnter;
    group.DragLeave += GridViewTileGroup_DragLeave;
   group.Drop += GridViewTileGroup_Drop;
      ItemsGridView.Items.Add(group);
    }
  }
       }
          }
        }

        /// <summary>
  /// Save all tabs to disk
 /// </summary>
    public void SaveAllTabs()
     {
    try
    {
  System.Diagnostics.Debug.WriteLine("=== SaveAllTabs START ===");
  System.Diagnostics.Debug.WriteLine($"MainTabView.TabItems.Count = {MainTabView.TabItems.Count}");
  
// Save current tab items first
     SaveCurrentTabItems();

   // Get selected tab index
    int selectedIndex = MainTabView.SelectedIndex;
   System.Diagnostics.Debug.WriteLine($"Selected tab index: {selectedIndex}");

  // Build a complete list of all unique items from all tabs
  var allUniqueItems = new List<UserControl>();
  
  // Also build a list of tabs (convert from WinRT collection to standard List)
  var tabsList = new List<Microsoft.UI.Xaml.Controls.TabViewItem>();
  
  for (int i = 0; i < MainTabView.TabItems.Count; i++)
{
  if (MainTabView.TabItems[i] is Microsoft.UI.Xaml.Controls.TabViewItem tab)
   {
       tabsList.Add(tab);  // Add to standard list
       
 string tabName = tab.Header?.ToString() ?? $"Tab {i}";
int tabItemCount = 0;
 
if (tab.Tag is List<UserControl> tabItems)
   {
 tabItemCount = tabItems.Count;
  foreach (var item in tabItems)
{
 if (!allUniqueItems.Contains(item))
    {
     allUniqueItems.Add(item);
 }
   }
 }
  else
  {
      System.Diagnostics.Debug.WriteLine($"WARNING: Tab {i} ('{tabName}') Tag is NOT a List<UserControl>! Tag type: {tab.Tag?.GetType().Name ?? "null"}");
  }
  
  System.Diagnostics.Debug.WriteLine($"Tab {i} ('{tabName}'): {tabItemCount} items");
 }
   else
   {
       System.Diagnostics.Debug.WriteLine($"WARNING: TabItems[{i}] is NOT a TabViewItem!");
   }
   }

  System.Diagnostics.Debug.WriteLine($"Total unique items across all tabs: {allUniqueItems.Count}");

  if (allUniqueItems.Count == 0)
  {
      System.Diagnostics.Debug.WriteLine("WARNING: No items to save! Tabs will be empty.");
  }

  // Save all unique items to Files directory FIRST
    UserSettingsClass.SaveLauncherXItems(allUniqueItems);
 System.Diagnostics.Debug.WriteLine("Saved all items to Files directory");

   // Now save tabs data - pass the standard List instead of WinRT collection
      UserSettingsClass.SaveTabsWithItemList(
   tabsList,  // ? Use standard List<TabViewItem>
      allUniqueItems,
     _tabColors,
  selectedIndex);

  System.Diagnostics.Debug.WriteLine($"=== SaveAllTabs COMPLETE: Saved {MainTabView.TabItems.Count} tabs ===\n");
 }
   catch (Exception ex)
  {
 System.Diagnostics.Debug.WriteLine($"ERROR in SaveAllTabs: {ex.Message}\n{ex.StackTrace}");
  }
   }

        /// <summary>
   /// Load tabs from disk
        /// </summary>
   public void LoadSavedTabs(List<UserControl> allItems)
        {
            try
 {
   var tabsData = UserSettingsClass.LoadTabs();
       if (tabsData == null || tabsData.Tabs.Count == 0)
  {
 System.Diagnostics.Debug.WriteLine("No saved tabs found, using default tab");
         return;
 }

          // Clear existing tabs
  MainTabView.TabItems.Clear();
       _tabColors.Clear();

      // Distribute items to tabs
   var tabItemsMap = UserSettingsClass.DistributeItemsToTabs(allItems, tabsData);

       // Create tabs from saved data
   for (int i = 0; i < tabsData.Tabs.Count; i++)
  {
    var tabData = tabsData.Tabs[i];
     var tab = new Microsoft.UI.Xaml.Controls.TabViewItem();
        
  // Set tab header
  tab.Header = tabData.Name;

   // Don't set icon initially - will be set if tab has a color

   // Set initial background (will be updated based on selection)
if (tabData.IsSelected)
  {
      tab.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
  }
    else
    {
    tab.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 238, 238, 238)); // #EEEEEE
      }

   // Set tab color - this will set the colored circle icon
  if (!string.IsNullOrEmpty(tabData.Color) && tabData.Color != "#00000000")
 {
         try
  {
 var colorStr = tabData.Color.TrimStart('#');
byte a = Convert.ToByte(colorStr.Substring(0, 2), 16);
    byte r = Convert.ToByte(colorStr.Substring(2, 2), 16);
  byte g = Convert.ToByte(colorStr.Substring(4, 2), 16);
 byte b = Convert.ToByte(colorStr.Substring(6, 2), 16);
var color = Color.FromArgb(a, r, g, b);
    _tabColors[tab] = color;
     UpdateTabColorSeparator(tab, color);  // This will set the colored circle icon
  }
   catch
      {
     // Invalid color format, ignore
       }
     }

  // Set tab items
    if (tabItemsMap.ContainsKey(tabData.Id))
   {
 tab.Tag = tabItemsMap[tabData.Id];
  }
   else
  {
  tab.Tag = new List<UserControl>();
       }

  // Add context menu
     AttachTabContextMenu(tab);

      // Add tab to TabView
 MainTabView.TabItems.Add(tab);
    }

 // Select the previously selected tab
   if (tabsData.SelectedTabIndex >= 0 && tabsData.SelectedTabIndex < MainTabView.TabItems.Count)
 {
              MainTabView.SelectedIndex = tabsData.SelectedTabIndex;
         }
        else
       {
   MainTabView.SelectedIndex = 0;
    }

 // Update previous tab reference
     _previousTab = MainTabView.SelectedItem as Microsoft.UI.Xaml.Controls.TabViewItem;

    // Load items for selected tab
    LoadCurrentTabItems();

     System.Diagnostics.Debug.WriteLine($"Loaded {tabsData.Tabs.Count} tabs");
       }
   catch (Exception ex)
     {
        System.Diagnostics.Debug.WriteLine($"Error loading tabs: {ex}");
 // Fall back to default tab
    InitializeTabs();
    }
   }
    }
}
