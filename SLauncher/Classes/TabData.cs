using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SLauncher.Classes
{
    /// <summary>
    /// Class to store tab information for JSON serialization
  /// </summary>
    public class TabData
    {
      /// <summary>
 /// Unique identifier for the tab
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the tab
      /// </summary>
        [JsonPropertyName("name")]
 public string Name { get; set; } = "New Tab";

        /// <summary>
        /// Tab icon symbol (e.g., "Home", "Document", "Folder")
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = "Document";

        /// <summary>
        /// Tab color in ARGB format (e.g., "#50FF4545" for semi-transparent red)
        /// </summary>
   [JsonPropertyName("color")]
   public string Color { get; set; } = "#00000000"; // Transparent by default

        /// <summary>
  /// List of item indices that belong to this tab
        /// Format: "0", "1", "2/" (where "/" suffix indicates a group)
    /// </summary>
        [JsonPropertyName("itemIndices")]
        public List<string> ItemIndices { get; set; } = new List<string>();

     /// <summary>
    /// Whether this tab is currently selected
   /// </summary>
        [JsonPropertyName("isSelected")]
    public bool IsSelected { get; set; } = false;
    }

    /// <summary>
    /// Root class to store all tabs data
    /// </summary>
    public class TabsData
    {
        /// <summary>
        /// List of all tabs
  /// </summary>
        [JsonPropertyName("tabs")]
  public List<TabData> Tabs { get; set; } = new List<TabData>();

        /// <summary>
        /// Index of the currently selected tab
        /// </summary>
        [JsonPropertyName("selectedTabIndex")]
        public int SelectedTabIndex { get; set; } = 0;
    }
}
