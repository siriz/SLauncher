using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace SLauncher.Classes
{
    /// <summary>
    /// Class to manage Windows startup registration
    /// </summary>
    public static class StartupManager
    {
     private const string AppName = "SLauncher";
        private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Register the application to start with Windows
        /// </summary>
  public static bool RegisterStartup()
     {
  try
            {
     string exePath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
 
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, true))
  {
      key?.SetValue(AppName, $"\"{exePath}\"");
      }
   
      return true;
       }
         catch
      {
                return false;
 }
        }

/// <summary>
    /// Unregister the application from starting with Windows
        /// </summary>
        public static bool UnregisterStartup()
        {
     try
         {
  using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, true))
           {
       key?.DeleteValue(AppName, false);
       }
       
  return true;
   }
      catch
            {
      return false;
   }
        }

        /// <summary>
    /// Check if the application is registered to start with Windows
        /// </summary>
        public static bool IsRegistered()
        {
      try
 {
using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, false))
{
    object value = key?.GetValue(AppName);
           return value != null;
     }
     }
            catch
       {
            return false;
          }
     }

        /// <summary>
        /// Update startup registration based on settings
      /// </summary>
    public static void UpdateStartupRegistration(bool shouldStart)
      {
    if (shouldStart)
    {
                RegisterStartup();
          }
            else
            {
          UnregisterStartup();
            }
      }
    }
}
