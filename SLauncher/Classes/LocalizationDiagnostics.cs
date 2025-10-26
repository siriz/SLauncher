using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SLauncher.Classes
{
    /// <summary>
    /// Diagnostic tool for testing resource loading
    /// </summary>
    public static class LocalizationDiagnostics
{
        /// <summary>
        /// Run diagnostics and print results to debug output
/// </summary>
        public static void RunDiagnostics()
        {
  System.Diagnostics.Debug.WriteLine("=== LOCALIZATION DIAGNOSTICS START ===");
            
            // 1. Check base paths
   var basePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings"),
  Path.Combine(Directory.GetCurrentDirectory(), "Strings"),
     Path.Combine(AppContext.BaseDirectory, "Strings")
         };
            
        System.Diagnostics.Debug.WriteLine("\n1. Checking base paths:");
       foreach (var path in basePaths)
     {
  bool exists = Directory.Exists(path);
         System.Diagnostics.Debug.WriteLine($"   {path}: {(exists ? "EXISTS" : "NOT FOUND")}");
            
        if (exists)
      {
            var dirs = Directory.GetDirectories(path);
         System.Diagnostics.Debug.WriteLine($"   - Subdirectories: {string.Join(", ", Array.ConvertAll(dirs, Path.GetFileName))}");
         }
       }
          
     // 2. Check each language file
    System.Diagnostics.Debug.WriteLine("\n2. Checking language resource files:");
            var languages = new[] { "en-US", "ko-KR", "ja-JP" };
            
  foreach (var lang in languages)
            {
     foreach (var basePath in basePaths)
                {
            if (Directory.Exists(basePath))
     {
 string resourceFile = Path.Combine(basePath, lang, "Resources.resw");
            bool exists = File.Exists(resourceFile);
    System.Diagnostics.Debug.WriteLine($"   {lang}: {(exists ? "FOUND" : "NOT FOUND")} at {resourceFile}");
      
    if (exists)
         {
              try
     {
         // Try to load and parse the file
   string content = File.ReadAllText(resourceFile, Encoding.UTF8);
            XDocument doc = XDocument.Parse(content);
          var dataElements = doc.Descendants("data");
                int count = 0;
    
  System.Diagnostics.Debug.WriteLine($"   - Parsing: SUCCESS");
     
  // Show first 3 entries
        foreach (var element in dataElements)
     {
    if (count >= 3) break;
       
  string name = element.Attribute("name")?.Value;
      string value = element.Element("value")?.Value;
      
  if (!string.IsNullOrEmpty(name) && value != null)
          {
      System.Diagnostics.Debug.WriteLine($"     [{name}] = {value}");
        count++;
    }
               }
            }
  catch (Exception ex)
      {
     System.Diagnostics.Debug.WriteLine($"   - ERROR: {ex.Message}");
     }
             
      break; // Found the file, no need to check other paths
     }
         }
        }
  }

            // 3. Check current LocalizationManager state
            System.Diagnostics.Debug.WriteLine("\n3. Current LocalizationManager state:");
            System.Diagnostics.Debug.WriteLine($"   Current language: {LocalizationManager.GetCurrentLanguage()}");
     
      // Test a few keys
   var testKeys = new[] { "AppTitle", "SearchPlaceholder", "AddFileButton", "DefaultTabName" };
         System.Diagnostics.Debug.WriteLine("   Test key values:");
            foreach (var key in testKeys)
     {
     string value = LocalizationManager.GetString(key);
       bool isKey = value == key; // If value equals key, resource wasn't found
                System.Diagnostics.Debug.WriteLine($"   [{key}] = {value} {(isKey ? "? (KEY NOT FOUND)" : "?")}");
  }

            System.Diagnostics.Debug.WriteLine("\n=== LOCALIZATION DIAGNOSTICS END ===\n");
     }
    }
}
