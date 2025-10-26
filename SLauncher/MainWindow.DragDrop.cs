using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SLauncher.Classes;
using SLauncher.Controls.GridViewItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace SLauncher
{
    /// <summary>
    /// Drag and drop functionality partial class for MainWindow
    /// </summary>
    public sealed partial class MainWindow
    {
        // List to track items to remove after drag-drop operations
   List<UserControl> GridViewItemsToRemove = new List<UserControl>();

        /// <summary>
        /// Method that tries to show the DragDropInterface, if the data dragged in is valid
        /// </summary>
        /// <param name="e">The DragEventArgs from the event handler</param>
        public void TryShowDragDropInterface(DragEventArgs e)
        {
    if (e.DataView.Contains(StandardDataFormats.StorageItems) || e.DataView.Contains(StandardDataFormats.WebLink))
            {
           // User is dragging files/folders/websites into SLauncher
          DragDropInterface.Visibility = Visibility.Visible;
    }
            else
            {
                DragDropInterface.Visibility = Visibility.Collapsed;
          }
        }

        // Item reordering drag-drop events
        
        private void ItemsGridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
  {
       if (e.Items[0] is GridViewTile)
            {
          e.Data.Properties.Add("DraggedControl", (e.Items[0] as GridViewTile));
    }
            else if (e.Items[0] is GridViewTileGroup)
  {
         e.Data.Properties.Add("DraggedControl", (e.Items[0] as GridViewTileGroup));
  }
        }

        // GridViewTile drag events - for creating groups
        
        private void GridViewTile_DragEnter(object sender, DragEventArgs e)
        {
 if (e.Data != null && e.Data.Properties["DraggedControl"] != null && e.Data.Properties["DraggedControl"] is GridViewTile)
 {
       GridViewTile DraggedTile = e.Data.Properties["DraggedControl"] as GridViewTile;
      GridViewTile DraggedOverTile = sender as GridViewTile;

                if (DraggedTile.UniqueId != DraggedOverTile.UniqueId)
         {
   // Show some indication that a group can be formed
           DraggedOverTile.ShowCreateGroupIndicator();
          }
    }
        }

        private void GridViewTile_DragLeave(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.Properties["DraggedControl"] != null && e.Data.Properties["DraggedControl"] is GridViewTile)
        {
      GridViewTile DraggedTile = e.Data.Properties["DraggedControl"] as GridViewTile;
        GridViewTile DraggedOverTile = sender as GridViewTile;

       if (DraggedTile.UniqueId != DraggedOverTile.UniqueId)
       {
 // Hide the create group indicator
         DraggedOverTile.HideCreateGroupIndicator();
                }
            }
        }

        private void GridViewTile_Drop(object sender, DragEventArgs e)
        {
     if (e.Data != null && e.Data.Properties["DraggedControl"] != null && e.Data.Properties["DraggedControl"] is GridViewTile)
     {
  GridViewTile DroppedOnTile = sender as GridViewTile;
       GridViewTile DraggedTile = e.Data.Properties["DraggedControl"] as GridViewTile;
     DroppedOnTile.HideCreateGroupIndicator();
      DraggedTile.UnhighlightControl();

  if (DroppedOnTile.UniqueId == DraggedTile.UniqueId)
    {
           return;
          }

                // Create a new group when a GridViewTile is dropped over a GridViewTile
                GridViewTileGroup newGridViewTileGroup = new GridViewTileGroup();
      newGridViewTileGroup.Size = UserSettingsClass.GridScale;
  newGridViewTileGroup.DisplayText = "New group";
        newGridViewTileGroup.DragEnter += GridViewTileGroup_DragEnter;
  newGridViewTileGroup.DragLeave += GridViewTileGroup_DragLeave;
         newGridViewTileGroup.Drop += GridViewTileGroup_Drop;

                // Add GridViewTiles
     DraggedTile.AssociateGroup(newGridViewTileGroup);
       DroppedOnTile.AssociateGroup(newGridViewTileGroup);
    newGridViewTileGroup.Items.Add(DraggedTile);
       newGridViewTileGroup.Items.Add(DroppedOnTile);

                // Add the GridViewTileGroup
       int index = ItemsGridView.Items.IndexOf(DroppedOnTile);
              ItemsGridView.Items.Insert(index, newGridViewTileGroup);

       // Mark the old GridViewTile objects for deletion
       GridViewItemsToRemove.Add(DroppedOnTile);
                GridViewItemsToRemove.Add(DraggedTile);
            }
        }

        // GridViewTileGroup drag events - for adding to existing groups
        
        private void GridViewTileGroup_DragEnter(object sender, DragEventArgs e)
        {
  if (e.Data != null && e.Data.Properties["DraggedControl"] != null && e.Data.Properties["DraggedControl"] is GridViewTile)
     {
      GridViewTileGroup DraggedOverTileGroup = sender as GridViewTileGroup;

     // Show some indication that a item can be added to the group
         DraggedOverTileGroup.ShowAddItemIndicator();
            }
        }

        private void GridViewTileGroup_DragLeave(object sender, DragEventArgs e)
        {
     if (e.Data != null && e.Data.Properties["DraggedControl"] != null && e.Data.Properties["DraggedControl"] is GridViewTile)
         {
GridViewTileGroup DraggedOverTileGroup = sender as GridViewTileGroup;

      // Show some indication that a item can be added to the group
    DraggedOverTileGroup.HideAddItemIndicator();
            }
     }

        private void GridViewTileGroup_Drop(object sender, DragEventArgs e)
   {
     if (e.Data != null && e.Data.Properties["DraggedControl"] != null && e.Data.Properties["DraggedControl"] is GridViewTile)
            {
              GridViewTileGroup existingGridViewTileGroup = sender as GridViewTileGroup;
              GridViewTile DraggedTile = e.Data.Properties["DraggedControl"] as GridViewTile;
    DraggedTile.UnhighlightControl();

      // Add the DraggedTile to the existingGridViewTileGroup
          existingGridViewTileGroup.HideAddItemIndicator();
      DraggedTile.AssociateGroup(existingGridViewTileGroup);
        existingGridViewTileGroup.Items.Add(DraggedTile);

  // Mark objects for deletion
          GridViewItemsToRemove.Add(DraggedTile);
         }
  }

     private void ItemsGridView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
   {
   // Remove the old GridView items if applicable
    foreach (UserControl control in GridViewItemsToRemove)
            {
   ItemsGridView.Items.Remove(control);
            }
     GridViewItemsToRemove.Clear();

      // Unhighlight all controls, just in case
         foreach (UserControl gridViewItem in ItemsGridView.Items)
            {
      if (gridViewItem is GridViewTile)
              {
         GridViewTile gridViewTile = gridViewItem as GridViewTile;
  gridViewTile.UnhighlightControl();
          }
     else if (gridViewItem is GridViewTileGroup)
     {
             GridViewTileGroup gridViewTileGroup = gridViewItem as GridViewTileGroup;
          gridViewTileGroup.UnhighlightControl();
     }
}
      }

        // External drag-drop events (files/folders/websites from outside)
    
    private void DragDropParent_DragEnter(object sender, DragEventArgs e)
    {
    TryShowDragDropInterface(e);
        }

        private void DragDropParent_DragOver(object sender, DragEventArgs e)
        {
  TryShowDragDropInterface(e);
 }

        private void DragDropParent_DragLeave(object sender, DragEventArgs e)
      {
   DragDropInterface.Visibility = Visibility.Collapsed;
        }

        private void DragDropInterface_DragEnter(object sender, DragEventArgs e)
        {
            // Modify the caption that shows when something is dragged into LauncherX
            e.AcceptedOperation = DataPackageOperation.Copy;
            if (e.DragUIOverride != null)
            {
    e.DragUIOverride.Caption = "Add to SLauncher";
 e.DragUIOverride.IsContentVisible = true;
  }
        }

     private void DragDropInterface_DragOver(object sender, DragEventArgs e)
     {
      // Modify the caption that shows when something is dragged into LauncherX
e.AcceptedOperation = DataPackageOperation.Copy;
 if (e.DragUIOverride != null)
    {
         e.DragUIOverride.Caption = "Add to SLauncher";
           e.DragUIOverride.IsContentVisible = true;
            }
        }

  private async void DragDropInterface_Drop(object sender, DragEventArgs e)
        {
 DragDropInterface.Visibility = Visibility.Collapsed;

     // Show LoadingDialog while loading items and settings
    LoadingDialog.Visibility = Visibility.Visible;
          await Task.Delay(10);

     // When a URL is dragged in, it technically qualifies as both a internet shortcut (.url) file, and a WebLink
            // Thus, we check if the DataView contains a WebLink first, so that URLs dragged in are added as websites instead of files
            // Source: https://stackoverflow.com/questions/66973410/drag-and-drop-items-in-windows-application-and-get-the-standarddataformats-of-th
            if (e.DataView.Contains(StandardDataFormats.WebLink))
      {
       // Dragged item is a website
      Uri websiteUri = await e.DataView.GetWebLinkAsync();

              // Add the website
      var websiteIcon = IconHelpers.GetWebsiteIcon(websiteUri.ToString());

 // Create new GridViewTile to display the website
              AddGridViewTile(websiteUri.ToString(), "", websiteUri.ToString(), websiteIcon);
  }
  else if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
  // Dragged item(s) are folders/files
        var items = await e.DataView.GetStorageItemsAsync();

  // Folder
                foreach (var storageItem in items.OfType<StorageFolder>())
     {
     // This is a folder
            // Get folder icon
     var bitmapImage = await IconHelpers.GetFolderIcon(storageItem.Path);

                 // Add folder to ItemsGridView
AddGridViewTile(storageItem.Path, "", storageItem.Name, bitmapImage);
  }

      // File
      foreach (var storageItem in items.OfType<StorageFile>())
      {
    // This is a file
         string filePath = storageItem.Path;

            // Get the thumbnail of the file and add it to ItemsGridView
          AddGridViewTile(filePath, "", storageItem.Name, await IconHelpers.GetFileIcon(filePath));
             }
  }

     // Hide LoadingDialog
            await Task.Delay(20);
          LoadingDialog.Visibility = Visibility.Collapsed;
     }
    }
}
