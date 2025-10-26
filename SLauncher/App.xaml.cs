using SLauncher.Classes;
using Microsoft.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;

namespace SLauncher
{
    public partial class App : Application
    {
        public static MainWindow MainWindow;

        public App()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[App] Constructor START");
                this.InitializeComponent();
         
                // Initialize settings and localization BEFORE creating main window
                System.Diagnostics.Debug.WriteLine("[App] Creating settings directories...");
                UserSettingsClass.CreateSettingsDirectories();
 
                System.Diagnostics.Debug.WriteLine("[App] Reading settings file...");
                UserSettingsClass.TryReadSettingsFile();
     
                System.Diagnostics.Debug.WriteLine("[App] Initializing LocalizationManager...");
                LocalizationManager.Initialize();
                System.Diagnostics.Debug.WriteLine("[App] LocalizationManager initialized");
   
                // DON'T create MainWindow here - move to OnLaunched
                // MainWindow = new MainWindow();
    
                System.Diagnostics.Debug.WriteLine("[App] Setting up persistence storage...");
                WinUIEx.WindowManager.PersistenceStorage = new FilePersistence(Path.Combine(UserSettingsClass.SettingsDir, "windowPlace.json"));
         
                System.Diagnostics.Debug.WriteLine("[App] Constructor COMPLETE");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[App] EXCEPTION in constructor: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[App] StackTrace: {ex.StackTrace}");
   
                try
                {
                    var errorFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SLauncher_Startup_Error.txt");
                    File.WriteAllText(errorFile, $"{DateTime.Now}\n\nStartup Error:\n\n{ex}\n\nInner Exception:\n{ex.InnerException}");
                }
                catch { }
                throw;
            }
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[App] OnLaunched called");
       
                // Create MainWindow HERE instead of in constructor
                System.Diagnostics.Debug.WriteLine("[App] Creating MainWindow...");
                MainWindow = new MainWindow();
                System.Diagnostics.Debug.WriteLine($"[App] MainWindow created: {MainWindow != null}");
  
                System.Diagnostics.Debug.WriteLine("[App] Activating MainWindow...");
                MainWindow.Activate();
                System.Diagnostics.Debug.WriteLine("[App] MainWindow activated successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[App] EXCEPTION in OnLaunched: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[App] StackTrace: {ex.StackTrace}");
   
                try
                {
                    var errorFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SLauncher_Launch_Error.txt");
                    File.WriteAllText(errorFile, $"{DateTime.Now}\n\nLaunch Error:\n\n{ex}\n\nInner Exception:\n{ex.InnerException}");
      }
         catch { }
                throw;
        }
   }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, System.Text.StringBuilder packageFullName);

        private class FilePersistence : IDictionary<string, object>
        {
            private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
            private readonly string _file;

            public FilePersistence(string filename)
            {
                _file = filename;
                try
                {
                    if (File.Exists(filename))
                    {
                        var jo = System.Text.Json.Nodes.JsonObject.Parse(File.ReadAllText(filename)) as JsonObject;
                        foreach (var node in jo)
                        {
                            if (node.Value is JsonValue jvalue && jvalue.TryGetValue<string>(out string value))
                                _data[node.Key] = value;
                        }
                    }
                }
                catch { }
            }

            private void Save()
            {
                JsonObject jo = new JsonObject();
                foreach (var item in _data)
                {
                    if (item.Value is string s)
                        jo.Add(item.Key, s);
                }
                File.WriteAllText(_file, jo.ToJsonString());
            }

            public object this[string key] { get => _data[key]; set { _data[key] = value; Save(); } }
            public ICollection<string> Keys => _data.Keys;
            public ICollection<object> Values => _data.Values;
            public int Count => _data.Count;
            public bool IsReadOnly => false;

            public void Add(string key, object value) { _data.Add(key, value); Save(); }
            public void Add(KeyValuePair<string, object> item) { _data.Add(item.Key, item.Value); Save(); }
            public void Clear() { _data.Clear(); Save(); }
            public bool Contains(KeyValuePair<string, object> item) => _data.Contains(item);
            public bool ContainsKey(string key) => _data.ContainsKey(key);
            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => throw new NotImplementedException();
            public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => throw new NotImplementedException();
            public bool Remove(string key) => throw new NotImplementedException();
            public bool Remove(KeyValuePair<string, object> item) => throw new NotImplementedException();
            public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => throw new NotImplementedException();
            IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        }
    }
}


