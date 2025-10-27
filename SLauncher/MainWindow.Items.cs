using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using SLauncher.Classes;
using SLauncher.Controls.Dialogs;
using SLauncher.Controls.GridViewItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLauncher
{
    /// <summary>
    /// Item management partial class for MainWindow
    /// </summary>
    public sealed partial class MainWindow
    {
   /// <summary>
 /// Method that adds a new GridViewTile to the ItemsGridView
        /// </summary>
        /// <param name="executingPath">ExecutingPath in GridViewTile</param>
     /// <param name="executingArguments">ExecutingArguments in GridViewTile</param>
        /// <param name="displayText">DisplayText in GridViewTile</param>
    /// <param name="imageSource">ImageSource in GridViewTile</param>
        /// <returns>The GridViewTile created</returns>
        private GridViewTile AddGridViewTile(string executingPath, string executingArguments, string displayText, BitmapImage imageSource)
        {
  // Create new GridViewTile for each item
 GridViewTile gridViewTile = new GridViewTile();
      gridViewTile.ExecutingPath = executingPath;
            gridViewTile.ExecutingArguments = executingArguments;
   gridViewTile.DisplayText = displayText;
 gridViewTile.Link = executingPath; // NEW: Initialize Link with executingPath
 gridViewTile.ImageSource = imageSource;
    gridViewTile.Size = UserSettingsClass.GridScale;
            gridViewTile.Drop += GridViewTile_Drop;
     gridViewTile.DragEnter += GridViewTile_DragEnter;
   gridViewTile.DragLeave += GridViewTile_DragLeave;
    ItemsGridView.Items.Add(gridViewTile);

     return gridViewTile;
    }

        /// <summary>
        /// Method that converts the items in the ItemsGridView to a List of UserControls (containing both GridViewTile and GridViewTileGroups, in order)
        /// </summary>
     /// <returns>List of UserControls in ItemsGridView</returns>
        private List<UserControl> SerialiseGridViewItemsToList()
        {
     List<UserControl> controls = new List<UserControl>();

          // Retrieve all the items in SLauncher
     foreach (UserControl gridViewItem in ItemsGridView.Items)
            {
    if (gridViewItem is GridViewTile)
           {
   GridViewTile gridViewTile = gridViewItem as GridViewTile;
       controls.Add(gridViewTile);
      }
         else if (gridViewItem is GridViewTileGroup)
       {
         GridViewTileGroup gridViewTileGroup = gridViewItem as GridViewTileGroup;
         controls.Add(gridViewTileGroup);
           }
  }

      return controls;
        }

        /// <summary>
        /// Converts a list of UserControls (containing both GridViewTile and GridViewTileGroups, in order) to items in the ItemsGridView
        /// </summary>
        /// <param name="controls"></param>
        private void DeserialiseListToGridViewItems(List<UserControl> controls)
        {
   ItemsGridView.Items.Clear();

 // Add the loaded controls to the ItemsGridView
            foreach (UserControl control in controls)
  {
           if (control is GridViewTile)
             {
  // Hook up event handlers
         GridViewTile gridViewTile = (GridViewTile)control;
gridViewTile.Drop += GridViewTile_Drop;
   gridViewTile.DragEnter += GridViewTile_DragEnter;
      gridViewTile.DragLeave += GridViewTile_DragLeave;
             ItemsGridView.Items.Add(gridViewTile);
   }
      else if (control is GridViewTileGroup)
           {
   // Hook up event handlers
           GridViewTileGroup gridViewTileGroup = (GridViewTileGroup)control;
             gridViewTileGroup.DragEnter += GridViewTileGroup_DragEnter;
         gridViewTileGroup.DragLeave += GridViewTileGroup_DragLeave;
 gridViewTileGroup.Drop += GridViewTileGroup_Drop;
      ItemsGridView.Items.Add(gridViewTileGroup);
          }
      }
  }

        private void ItemsGridViewItems_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
        {
            // Show/Hide the EmptyNotice depending on whether there are items in the ItemsGridView
    if (ItemsGridView.Items.Count > 0)
    {
       EmptyNotice.Visibility = Visibility.Collapsed;
          }
            else
          {
  EmptyNotice.Visibility = Visibility.Visible;
        }
        }

 private async void AddFileBtn_Click(object sender, RoutedEventArgs e)
        {
            AddFileDialog addFileDialog = new AddFileDialog()
            {
    XamlRoot = Container.XamlRoot
            };

      ContentDialogResult result = await addFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
   {
           // Show LoadingDialog while loading items and settings
        LoadingDialog.Visibility = Visibility.Visible;
       await Task.Delay(10);

      // Add the files from the addFileDialog
        foreach (AddFileDialogListViewItem fileItem in addFileDialog.AddedFiles)
                {
     // Create new GridViewTile for each item
 AddGridViewTile(fileItem.ExecutingPath, fileItem.ExecutingArguments, fileItem.DisplayText, fileItem.FileIcon);
    }

        // Save items
 UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);

     // Hide LoadingDialog
   await Task.Delay(20);
         LoadingDialog.Visibility = Visibility.Collapsed;
            }
        }

 private async void AddFolderBtn_Click(object sender, RoutedEventArgs e)
        {
  AddFolderDialog addFolderDialog = new AddFolderDialog()
            {
         XamlRoot = Container.XamlRoot
   };

    ContentDialogResult result = await addFolderDialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
      {
   return;
            }

      // Show LoadingDialog while loading items and settings
        LoadingDialog.Visibility = Visibility.Visible;
            await Task.Delay(10);

            // Add the folders from the addFolderDialog (Shortcut mode only)
foreach (AddFolderDialogListViewItem folderItem in addFolderDialog.AddedFolders)
{
          // Add folder as shortcut
      AddGridViewTile(folderItem.ExecutingPath, "", folderItem.DisplayText, folderItem.FolderIcon);
            }

        // Save items
            UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);

          // Hide LoadingDialog
            await Task.Delay(20);
   LoadingDialog.Visibility = Visibility.Collapsed;
 }

        private async void AddWebsiteBtn_Click(object sender, RoutedEventArgs e)
        {
            AddWebsiteDialog addWebsiteDialog = new AddWebsiteDialog()
        {
    XamlRoot = Container.XamlRoot
     };

        ContentDialogResult result = await addWebsiteDialog.ShowAsync();

if (result == ContentDialogResult.Primary)
            {
              // Show LoadingDialog while loading items and settings
              LoadingDialog.Visibility = Visibility.Visible;
 await Task.Delay(10);

             // Get the icon of the website
   BitmapImage websiteIcon = IconHelpers.GetWebsiteIcon(addWebsiteDialog.InputWebsiteUrl);

  // Create new GridViewTile to display the website
  AddGridViewTile(addWebsiteDialog.InputWebsiteUrl, "", addWebsiteDialog.InputWebsiteUrl, websiteIcon);
            }

            // Save items
    UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);

      // Hide LoadingDialog
       await Task.Delay(20);
            LoadingDialog.Visibility = Visibility.Collapsed;
        }
    }
}
