using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SLauncher.Classes;
using SLauncher.Controls.GridViewItems;
using System;

namespace SLauncher
{
    /// <summary>
    /// UI management partial class for MainWindow
    /// </summary>
    public sealed partial class MainWindow
  {
        // Track if settings window is already open
        private bool isSettingsWindowOpen = false;

        /// <summary>
   /// Initialize UI text from localized resources
        /// </summary>
  private void InitializeLocalizedUI()
     {
       try
       {
      System.Diagnostics.Debug.WriteLine("[MainWindow.UI] Initializing localized UI...");
 
    // Set window title
      string appTitle = LocalizationManager.GetString("AppTitle");
  System.Diagnostics.Debug.WriteLine($"[MainWindow.UI] AppTitle: {appTitle}");
      this.Title = appTitle;
     AppTitleBar.Title = appTitle;
   
   // Set SearchBox placeholder
  string searchPlaceholder = LocalizationManager.GetString("SearchPlaceholder");
      System.Diagnostics.Debug.WriteLine($"[MainWindow.UI] SearchPlaceholder: {searchPlaceholder}");
       SearchBox.PlaceholderText = searchPlaceholder;
       
 // Set button contents
    AddFileBtn.Content = LocalizationManager.GetString("AddFileButton");
     AddFolderBtn.Content = LocalizationManager.GetString("AddFolderButton");
       AddWebsiteBtn.Content = LocalizationManager.GetString("AddWebsiteButton");
  
      // Set tooltips
Microsoft.UI.Xaml.Controls.ToolTipService.SetToolTip(SettingsButton, LocalizationManager.GetString("TooltipSettings"));
   Microsoft.UI.Xaml.Controls.ToolTipService.SetToolTip(CloseButton, LocalizationManager.GetString("TooltipClose"));
   
    // Set empty notice text
      if (EmptyNotice != null && EmptyNotice.Children.Count >= 3)
         {
      if (EmptyNotice.Children[1] is TextBlock titleBlock)
      {
    titleBlock.Text = LocalizationManager.GetString("EmptyNoticeTitle");
 }
    if (EmptyNotice.Children[2] is TextBlock messageBlock)
          {
   messageBlock.Text = LocalizationManager.GetString("EmptyNoticeMessage");
  }
          }
      
        // Set drag-drop interface text
        if (DragDropInterface != null && DragDropInterface.Children.Count > 0)
    {
  if (DragDropInterface.Children[0] is StackPanel dragPanel && dragPanel.Children.Count >= 2)
            {
     if (dragPanel.Children[1] is TextBlock dragText)
 {
          dragText.Text = LocalizationManager.GetString("DragDropMessage");
                }
            }
 }
        
  // Set loading dialog text
        if (LoadingDialog != null && LoadingDialog.Children.Count > 0)
        {
            // Navigate through the Border -> StackPanel structure
         if (LoadingDialog.Children[0] is Border loadingBorder && 
         loadingBorder.Child is StackPanel outerPanel && 
           outerPanel.Children.Count >= 2 &&
    outerPanel.Children[1] is StackPanel textPanel && 
   textPanel.Children.Count >= 2)
    {
 if (textPanel.Children[0] is TextBlock loadingTitle)
     {
  loadingTitle.Text = LocalizationManager.GetString("LoadingTitle");
      }
                if (textPanel.Children[1] is TextBlock loadingSubtitle)
        {
  loadingSubtitle.Text = LocalizationManager.GetString("LoadingSubtitle");
          }
     }
    }
  
        System.Diagnostics.Debug.WriteLine("[MainWindow.UI] Localized UI initialized successfully");
 }
   catch (Exception ex)
  {
  System.Diagnostics.Debug.WriteLine($"[MainWindow.UI] Error initializing localized UI: {ex.Message}\n{ex.StackTrace}");
   }
   }

 /// <summary>
        /// Method that updates the UI based on the UserSettingsClass
        /// </summary>
        private void UpdateUIFromSettings()
        {
      // Adjust the size of items in ItemsGridView (Update from GridScale)
      foreach (var gridViewItem in ItemsGridView.Items)
     {
 if (gridViewItem is GridViewTile)
       {
      ((GridViewTile)gridViewItem).Size = UserSettingsClass.GridScale;
      }
   else if (gridViewItem is GridViewTileGroup)
      {
           GridViewTileGroup gridViewTileGroup = gridViewItem as GridViewTileGroup;
        gridViewTileGroup.Size = UserSettingsClass.GridScale;

       // Update the size of the items in the GridViewTileGroup as well
          foreach (GridViewTile gridViewTile in gridViewTileGroup.Items)
         {
       gridViewTile.Size = UserSettingsClass.GridScale;
   }
           }
          }

         // Set windowing mode to fullscreen if applicable (Update from UseFullscreen)
            if (UserSettingsClass.UseFullscreen == true)
            {
       // Hide custom titlebar
      this.ExtendsContentIntoTitleBar = false;
           AppTitleBar.Visibility = Visibility.Collapsed;

 // Adjust controls
       CloseButton.Visibility = Visibility.Visible;
   ControlsGrid.Margin = new Thickness(20, 10, 20, 0);

         // Set fullscreen
   this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);
    }
    else
  {
        // Set custom titlebar
              this.ExtendsContentIntoTitleBar = true;
           AppTitleBar.Visibility = Visibility.Visible;
     this.SetTitleBar(AppTitleBar);

  // Adjust controls
  CloseButton.Visibility = Visibility.Collapsed;
   ControlsGrid.Margin = new Thickness(20, 0, 20, 0);

          // Set normal windowing mode
   this.AppWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Default);
            }

    // Align the GridView (Update from GridPosition)
            if (UserSettingsClass.GridPosition == "Left")
       {
     AlignGridViewLeft();
   }
            else if (UserSettingsClass.GridPosition == "Center")
            {
    AlignGridViewCenter();
         }
        }

        /// <summary>
/// Method that aligns the ItemsGridView to the left
        /// </summary>
        private void AlignGridViewLeft()
        {
    ItemsGridView.HorizontalAlignment = HorizontalAlignment.Stretch;

     // Fix the width of the ItemsGridView to take up the entire space available
    ItemsGridView.Width = ControlsGrid.Width;

 // Adjust scrollbar margins as well
        ScrollViewerExtensions.SetVerticalScrollBarMargin(ItemsGridView, new Thickness(0, 0, 0, 0));
        }

    private void AlignGridViewCenter()
  {
 ItemsGridView.HorizontalAlignment = HorizontalAlignment.Center;

            // Fix the width of the ItemsGridView to perfectly match the row of GridViewTiles/GridViewTileGroups
            // Since the ItemsGridView has HorizontalAlignment = Center, this will thus center the ItemsGridView
  // +4 is because by default, a GridViewItem has a right margin of 4
            ItemsGridView.Width = Math.Floor(ControlsGrid.Width / (105 * Math.Sqrt(UserSettingsClass.GridScale) + 4)) * ((105 * Math.Sqrt(UserSettingsClass.GridScale) + 4));

       // Adjust scrollbar margins as well
            ScrollViewerExtensions.SetVerticalScrollBarMargin(ItemsGridView, new Thickness(0, 0, -20, 0));
        }

        /// <summary>
      /// Create Icon Scale Slider dynamically to avoid XAML compiler bug
 /// </summary>
        private void CreateIconScaleSlider()
    {
            try
        {
     // Create icon FontIcon
          var icon = new FontIcon
                {
              Glyph = "\xE71E", // Zoom icon
   FontSize = 16,
           VerticalAlignment = VerticalAlignment.Center,
           Opacity = 0.7
        };
   Microsoft.UI.Xaml.Controls.ToolTipService.SetToolTip(icon, "Icon Scale");

    // Create Slider
         IconScaleSlider = new Slider
           {
  Width = 150,
     VerticalAlignment = VerticalAlignment.Center,
             Minimum = 0.25,
  Maximum = 6.00,
   Value = UserSettingsClass.GridScale,
       SmallChange = 0.05,
    StepFrequency = 0.05,
      TickFrequency = 0.05
          };
  IconScaleSlider.ValueChanged += IconScaleSlider_ValueChanged;
           Microsoft.UI.Xaml.Controls.ToolTipService.SetToolTip(IconScaleSlider, "Adjust icon scale");

      // Create scale value TextBlock
              ScaleValueText = new TextBlock
      {
      VerticalAlignment = VerticalAlignment.Center,
  FontSize = 12,
     Opacity = 0.7,
        Text = $"{UserSettingsClass.GridScale:F2}x",
              MinWidth = 40
          };
       Microsoft.UI.Xaml.Controls.ToolTipService.SetToolTip(ScaleValueText, "Current scale");

 // Add controls to container
       IconScaleContainer.Children.Add(icon);
     IconScaleContainer.Children.Add(IconScaleSlider);
                IconScaleContainer.Children.Add(ScaleValueText);
       }
   catch (Exception ex)
       {
        System.Diagnostics.Debug.WriteLine($"Error creating icon scale slider: {ex}");
         }
  }

        // Icon Scale Slider event handler
        private void IconScaleSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
       // Update scale value text
       double scale = Math.Round(IconScaleSlider.Value, 2);
            ScaleValueText.Text = $"{scale:F2}x";
 
        // Update UserSettingsClass
        UserSettingsClass.GridScale = scale;
            UserSettingsClass.WriteSettingsFile();
    
     // Update all item sizes in real-time
       foreach (var gridViewItem in ItemsGridView.Items)
     {
     if (gridViewItem is GridViewTile)
     {
         ((GridViewTile)gridViewItem).Size = scale;
   }
          else if (gridViewItem is GridViewTileGroup)
         {
     GridViewTileGroup gridViewTileGroup = gridViewItem as GridViewTileGroup;
             gridViewTileGroup.Size = scale;

 // Update the size of the items in the GridViewTileGroup as well
           foreach (GridViewTile gridViewTile in gridViewTileGroup.Items)
 {
               gridViewTile.Size = scale;
  }
         }
            }
   
          // Re-align the GridView
         if (UserSettingsClass.GridPosition == "Left")
{
           AlignGridViewLeft();
          }
        else if (UserSettingsClass.GridPosition == "Center")
  {
    AlignGridViewCenter();
       }
        }

        /// <summary>
        /// Handle Ctrl + Mouse Wheel for icon scaling
        /// </summary>
        private void ItemsGridView_PointerWheelChanged(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
         try
            {
        var pointer = e.GetCurrentPoint(ItemsGridView);
      var properties = pointer.Properties;
   
         // Check if Ctrl key is pressed
          var ctrlPressed = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control)
  .HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

   if (ctrlPressed && IconScaleSlider != null)
         {
          // Prevent default scrolling behavior
        e.Handled = true;
       
       // Get mouse wheel delta (positive = scroll up, negative = scroll down)
      int wheelDelta = properties.MouseWheelDelta;
     
           // Calculate new scale value
               // Each wheel "click" changes by 0.1
      double scaleChange = (wheelDelta > 0) ? 0.1 : -0.1;
     double newScale = Math.Round(IconScaleSlider.Value + scaleChange, 2);
           
 // Clamp to min/max values
     newScale = Math.Max(0.25, Math.Min(6.00, newScale));
   
  // Update slider (which will trigger ValueChanged event)
        IconScaleSlider.Value = newScale;
            }
        }
         catch (Exception ex)
            {
         System.Diagnostics.Debug.WriteLine($"Error handling mouse wheel: {ex}");
   }
        }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
      {
        // Prevent opening multiple settings windows
            if (isSettingsWindowOpen)
  {
        return;
            }

            isSettingsWindowOpen = true;
       
            // Disable settings button
       SettingsButton.IsEnabled = false;

        // Open Settings Window as a modal window
  SettingsWindow settingsWindow = new SettingsWindow();
         UIFunctionsClass.CreateModalWindow(settingsWindow, this);

          // Update the UI once the SettingsWindow is closed
            settingsWindow.Closed += (s, e) =>
 {
             UpdateUIFromSettings();
          isSettingsWindowOpen = false;
                
   // Re-enable settings button
  SettingsButton.IsEnabled = true;
            };
        }

     // For fullscreen mode - Exit LauncherX
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
       Application.Current.Exit();
    }

        // When window resized
        private void WindowEx_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
     // Align the GridView (Update from GridPosition)
            if (UserSettingsClass.GridPosition == "Left")
  {
          AlignGridViewLeft();
          }
            else if (UserSettingsClass.GridPosition == "Center")
  {
 AlignGridViewCenter();
            }
        }
    }
}
