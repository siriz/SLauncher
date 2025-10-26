using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SLauncher.Classes;
using SLauncher.Controls.GridViewItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SLauncher
{
    /// <summary>
 /// Search functionality partial class for MainWindow
 /// </summary>
    public sealed partial class MainWindow
    {
 List<GridViewTile> AllLauncherXItems = new List<GridViewTile>();
        List<string> SearchBoxDropdownItems = new List<string>();

 private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
      AllLauncherXItems.Clear();

            // Retrieve all the items in SLauncher
    foreach (UserControl gridViewItem in ItemsGridView.Items)
            {
         if (gridViewItem is GridViewTile)
                {
            GridViewTile gridViewTile = gridViewItem as GridViewTile;
  AllLauncherXItems.Add(gridViewTile);
      }
     else if (gridViewItem is GridViewTileGroup)
    {
                    GridViewTileGroup gridViewTileGroup = gridViewItem as GridViewTileGroup;
        foreach (GridViewTile tile in gridViewTileGroup.Items)
     {
       AllLauncherXItems.Add(tile);
       }
    }
    }
    }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
 {
   // Only get results when it was a user typing, 
         // otherwise assume the value got filled in by TextMemberPath 
       // or the handler for SuggestionChosen.
       if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
        // Set the ItemsSource to be your filtered dataset
          SearchBoxDropdownItems = new List<string>();
      foreach (GridViewTile gridViewTile in AllLauncherXItems)
              {
                    if (gridViewTile.DisplayText.ToLower().Contains(sender.Text.ToLower()))
       {
       SearchBoxDropdownItems.Add(gridViewTile.DisplayText);
       }
      }
 sender.ItemsSource = SearchBoxDropdownItems;
            }
  }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
sender.Text = args.SelectedItem.ToString();
        }

   private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string query = sender.Text?.Trim();
            
   // If query is empty, do nothing
     if (string.IsNullOrWhiteSpace(query))
       {
             return;
            }

  // Check if the query is a file or folder path
            bool isPath = false;
            bool pathExists = false;
    
            // Check for absolute paths (C:\, D:\, \\server\share, etc.)
         if (query.Length >= 2)
      {
          // Windows drive letter path (C:\, D:\, etc.)
    if (char.IsLetter(query[0]) && query[1] == ':')
     {
  isPath = true;
            }
       // UNC network path (\\server\share)
       else if (query.StartsWith("\\\\"))
     {
    isPath = true;
    }
        }

      // If it looks like a path, try to open it directly
       if (isPath)
            {
 try
          {
          // Check if file exists
      if (System.IO.File.Exists(query))
       {
    pathExists = true;
                // Open file
          ProcessStartInfo processStartInfo = new ProcessStartInfo 
           { 
       FileName = query, 
      UseShellExecute = true 
           };
           Process.Start(processStartInfo);
    
      // Clear search box
    sender.Text = "";
       
      // Hide window if MinimizeToTray is enabled
       if (UserSettingsClass.MinimizeToTray)
           {
    this.AppWindow.Hide();
            }

        return;
}
       // Check if directory exists
   else if (System.IO.Directory.Exists(query))
         {
          pathExists = true;
         // Open folder
           ProcessStartInfo processStartInfo = new ProcessStartInfo 
           { 
        FileName = "explorer.exe", 
   UseShellExecute = true, 
            Arguments = $"\"{query}\"" 
                   };
         Process.Start(processStartInfo);
        
          // Clear search box
                 sender.Text = "";
    
            // Hide window if MinimizeToTray is enabled
      if (UserSettingsClass.MinimizeToTray)
    {
         this.AppWindow.Hide();
  }
     
     return;
  }
          }
    catch (Exception ex)
                {
     System.Diagnostics.Debug.WriteLine($"Error opening path: {ex.Message}");
               
// Show error message
        var dialog = new ContentDialog
          {
                Title = "Error",
 Content = $"Unable to open:\n{query}\n\nError: {ex.Message}",
      CloseButtonText = "OK",
              XamlRoot = this.Content.XamlRoot
 };
 await dialog.ShowAsync();
      
        return;
}
         
         // If path format but doesn't exist, show error
      if (!pathExists)
     {
         var dialog = new ContentDialog
      {
    Title = "Path Not Found",
      Content = $"The specified path does not exist:\n{query}\n\nPlease check the path and try again.",
            CloseButtonText = "OK",
            XamlRoot = this.Content.XamlRoot
        };
        await dialog.ShowAsync();
 
         return;
     }
        }

     // Original search functionality - search through items
            // If there's nothing in the dropdown of the SearchBox, don't do anything
    if (SearchBoxDropdownItems.Count <= 0)
            {
     return;
          }

            string chosenSuggestion = "";
            if (args.ChosenSuggestion != null)
            {
    // User selected an item from the suggestion list, take an action on it here.
          chosenSuggestion = args.ChosenSuggestion.ToString();
     }
            else
            {
      chosenSuggestion = SearchBoxDropdownItems[0];
      }

         sender.Text = chosenSuggestion;

     // Find the corresponding GridViewTile, and start its associated process
            foreach (GridViewTile gridViewTile in AllLauncherXItems)
            {
    if (gridViewTile.DisplayText.ToLower() == chosenSuggestion.ToLower())
       {
        await gridViewTile.StartAssociatedProcess();
          
   // Hide window if MinimizeToTray is enabled
       if (UserSettingsClass.MinimizeToTray)
            {
     this.AppWindow.Hide();
        }
      
      break;
          }
            }

            AllLauncherXItems.Clear();
       SearchBoxDropdownItems.Clear();
          
   // Clear search box
    sender.Text = "";
        }
    }
}
