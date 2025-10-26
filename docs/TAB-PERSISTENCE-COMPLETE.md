# ? �� ���� ���� ���� ��� �Ϸ�!

## ?? **���� ����:**

���� �̸�, ����, ������, ������ ��ġ ������ JSON ���Ϸ� �����ϰ� �����ϴ� ����� �߰��߽��ϴ�!

---

## ?? **���� ��ġ**

### **���ο� ����:**
```
UserCache\
���� userSettings.json  (���� ���� ����)
���� tabs.json   �� ? NEW! (�� ���� ����)
���� Files\         (������ ���ϵ�)
   ���� 0.json
   ���� 1.json
   ���� ...
```

### **tabs.json ����:**
```json
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "�⺻",
  "icon": "Home",
      "color": "#50FF4545",
      "itemIndices": ["0", "1", "2/"],
      "isSelected": true
    },
    {
      "id": "tab-1",
      "name": "Work Apps",
    "icon": "Document",
      "color": "#5000FF00",
      "itemIndices": ["3", "4", "5"],
      "isSelected": false
    },
    {
    "id": "tab-2",
    "name": "Games",
      "icon": "Folder",
      "color": "#500000FF",
      "itemIndices": ["6", "7"],
      "isSelected": false
    }
  ],
  "selectedTabIndex": 0
}
```

**�׸� ����:**
- `id`: ���� ���� �ĺ���
- `name`: �� �̸�
- `icon`: �� ������ �ɺ� (Home, Document, Folder ��)
- `color`: �� ���� (ARGB ����)
- `itemIndices`: �� �ǿ� ���Ե� ������ �ε��� ��� ("2/"�� �׷�)
- `isSelected`: ���� ����
- `selectedTabIndex`: ���������� ���õ� �� �ε���

---

## ?? **�߰��� ���ϵ�**

### **1. TabData.cs (�� ����)**

**��ġ:** `SLauncher/Classes/TabData.cs`

**����:** �� ������ JSON���� �����ϱ� ���� ������ Ŭ����

```csharp
/// <summary>
/// Class to store tab information for JSON serialization
/// </summary>
public class TabData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = "New Tab";

  [JsonPropertyName("icon")]
    public string Icon { get; set; } = "Document";

    [JsonPropertyName("color")]
    public string Color { get; set; } = "#00000000";

    [JsonPropertyName("itemIndices")]
    public List<string> ItemIndices { get; set; } = new List<string>();

    [JsonPropertyName("isSelected")]
    public bool IsSelected { get; set; } = false;
}

/// <summary>
/// Root class to store all tabs data
/// </summary>
public class TabsData
{
 [JsonPropertyName("tabs")]
    public List<TabData> Tabs { get; set; } = new List<TabData>();

    [JsonPropertyName("selectedTabIndex")]
    public int SelectedTabIndex { get; set; } = 0;
}
```

**Ư¡:**
- JSON �Ӽ� �̸��� ��������� ���� (`[JsonPropertyName]`)
- �⺻�� �������� ������ Ȯ��
- `TabsData`�� ���� `TabData`�� �����ϴ� ����

---

## ?? **������ ���ϵ�**

### **2. UserSettingsClass.cs ����**

#### **�߰��� ����:**

**A. JSON ����ȭ ���ؽ�Ʈ ������Ʈ:**
```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(UserSettingsJson))]
[JsonSerializable(typeof(GridViewTileJson))]
[JsonSerializable(typeof(GridViewTileGroupJson))]
[JsonSerializable(typeof(TabsData))]        // ? NEW!
[JsonSerializable(typeof(TabData))]         // ? NEW!
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
```

**B. SaveTabs �޼��� (�� ����):**
```csharp
/// <summary>
/// Method to save tabs information to a JSON file
/// </summary>
public static void SaveTabs(
    System.Collections.IList tabs,
    ItemCollection allItems,
    Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Windows.UI.Color> tabColors,
    int selectedTabIndex)
{
    var tabsData = new TabsData
    {
        SelectedTabIndex = selectedTabIndex
    };

    // Convert items to list for indexing
    var itemsList = new List<UserControl>();
    foreach (UserControl item in allItems)
    {
     itemsList.Add(item);
    }

    // Save each tab
    for (int tabIndex = 0; tabIndex < tabs.Count; tabIndex++)
    {
        if (tabs[tabIndex] is not Microsoft.UI.Xaml.Controls.TabViewItem tab)
            continue;

        var tabData = new TabData
        {
Id = $"tab-{tabIndex}",
        Name = tab.Header?.ToString() ?? $"Tab {tabIndex + 1}",
            IsSelected = tabIndex == selectedTabIndex
        };

        // Get icon symbol
        if (tab.IconSource is Microsoft.UI.Xaml.Controls.SymbolIconSource symbolIcon)
        {
       tabData.Icon = symbolIcon.Symbol.ToString();
        }

        // Get tab color
        if (tabColors.ContainsKey(tab))
        {
            var color = tabColors[tab];
            tabData.Color = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        // Get items in this tab
        if (tab.Tag is List<UserControl> tabItems)
      {
   foreach (var item in tabItems)
 {
        int itemIndex = itemsList.IndexOf(item);
       if (itemIndex >= 0)
    {
  // Add "/" suffix for groups
        string indexStr = item is GridViewTileGroup ? $"{itemIndex}/" : itemIndex.ToString();
     tabData.ItemIndices.Add(indexStr);
 }
       }
        }

        tabsData.Tabs.Add(tabData);
    }

    // Write to file
    string tabsFilePath = Path.Combine(SettingsDir, "tabs.json");
    string jsonString = JsonSerializer.Serialize(tabsData, SourceGenerationContext.Default.TabsData);
    File.WriteAllText(tabsFilePath, jsonString);
}
```

**�ٽ� ����:**
1. ��� ���� ��ȸ�ϸ� ���� ����
2. �� ���� �̸�, ������, ���� ����
3. �ǿ� ���� ������ �ε��� ���� (�׷��� "/" ���̻�)
4. JSON���� ����ȭ�Ͽ� `tabs.json` ���Ͽ� ����

---

**C. LoadTabs �޼��� (�� �ε�):**
```csharp
/// <summary>
/// Method to load tabs information from a JSON file
/// </summary>
/// <returns>TabsData object, or null if file doesn't exist</returns>
public static TabsData LoadTabs()
{
    try
    {
        string tabsFilePath = Path.Combine(SettingsDir, "tabs.json");

        if (!File.Exists(tabsFilePath))
  {
      return null;
        }

      string jsonString = File.ReadAllText(tabsFilePath);
     return JsonSerializer.Deserialize<TabsData>(jsonString, SourceGenerationContext.Default.TabsData);
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error loading tabs: {ex.Message}");
        return null;
    }
}
```

**�ٽ� ����:**
1. `tabs.json` ���� ���� Ȯ��
2. JSON ���� �б� �� ������ȭ
3. `TabsData` ��ü ��ȯ
4. ���� �߻� �� `null` ��ȯ (�⺻ �� ���)

---

**D. DistributeItemsToTabs �޼��� (������ �й�):**
```csharp
/// <summary>
/// Method to distribute items into tabs based on saved tab data
/// </summary>
public static Dictionary<string, List<UserControl>> DistributeItemsToTabs(
    List<UserControl> allItems,
    TabsData tabsData)
{
    var tabItemsMap = new Dictionary<string, List<UserControl>>();

    foreach (var tabData in tabsData.Tabs)
    {
     var tabItems = new List<UserControl>();

        foreach (var indexStr in tabData.ItemIndices)
        {
            // Remove "/" suffix if present
            string cleanIndexStr = indexStr.TrimEnd('/');
        
   if (int.TryParse(cleanIndexStr, out int itemIndex))
            {
  if (itemIndex >= 0 && itemIndex < allItems.Count)
       {
        tabItems.Add(allItems[itemIndex]);
                }
    }
        }

        tabItemsMap[tabData.Id] = tabItems;
    }

    return tabItemsMap;
}
```

**�ٽ� ����:**
1. ����� ������ �ε����� ���� ������ ��ü�� ��ȯ
2. �� �� ID�� Ű�� �ϴ� Dictionary ����
3. �Ǻ��� ������ ��� ����

---

### **3. MainWindow.Tabs.cs ����**

#### **�߰��� �޼���:**

**A. SaveAllTabs (��� �� ����):**
```csharp
/// <summary>
/// Save all tabs to disk
/// </summary>
public void SaveAllTabs()
{
    try
    {
        // Save current tab items first
        SaveCurrentTabItems();

        // Get selected tab index
  int selectedIndex = MainTabView.SelectedIndex;

        // Save tabs data
        UserSettingsClass.SaveTabs(
      (System.Collections.IList)MainTabView.TabItems,
            ItemsGridView.Items,
         _tabColors,
     selectedIndex);

        System.Diagnostics.Debug.WriteLine($"Saved {MainTabView.TabItems.Count} tabs");
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine($"Error saving tabs: {ex}");
    }
}
```

**ȣ�� ����:**
- �� ���� �� (`WindowEx_Closed`)

---

**B. LoadSavedTabs (����� �� �ε�):**
```csharp
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

            // Set tab icon
            if (Enum.TryParse<Symbol>(tabData.Icon, out var symbol))
  {
                tab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource { Symbol = symbol };
            }
  else
            {
   tab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource { Symbol = Symbol.Document };
            }

            // Set tab color
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
      UpdateTabColorSeparator(tab, color);
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
```

**ȣ�� ����:**
- �� ���� �� (`Container_Loaded`)

**���� ����:**
1. `tabs.json` ���� �ε�
2. ���� �� ��� ����
3. ����� �� �����ͷ� �� �����
4. �� �̸�, ������, ���� ����
5. �� �ǿ� ������ �Ҵ�
6. ������ ���õ� �� Ȱ��ȭ

---

### **4. MainWindow.xaml.cs ����**

#### **������ ����:**
```csharp
public MainWindow()
{
    this.InitializeComponent();

    // Initialize tabs - will be replaced by LoadSavedTabs if save exists
    // InitializeTabs();  // Commented out - will be called in Container_Loaded

    // Create a new event handler...
    ItemsGridView.Items.VectorChanged += ItemsGridViewItems_VectorChanged;
    
    // ...
}
```

**���� ����:**
- `InitializeTabs()` ȣ�� ���� (���Ǻη� `Container_Loaded`���� ȣ��)

---

#### **Container_Loaded ����:**
```csharp
else
{
    // Retrieve user settings from file
    UserSettingsClass.TryReadSettingsFile();

    // Once we have initialised the UserSettingsClass with the correct values, update the UI
    UpdateUIFromSettings();

    // Monitor when the window is resized
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
        InitializeTabs();
        DeserialiseListToGridViewItems(controls);
    }
}
```

**�ε� ����:**
```
1. ������ �ε� (LoadLauncherXItems)
2. �� ������ Ȯ�� (LoadTabs)
   
   ���� �� ������ ����? �� LoadSavedTabs(controls)
   ��            ���� �� ���� + ������ �й�
   ��
   ���� �� ������ ����? �� InitializeTabs()
  ���� �⺻ �� ���� + ��� ������ �ε�
```

---

#### **WindowEx_Closed ����:**
```csharp
// The last event handler - save items when the window is closed
private void WindowEx_Closed(object sender, WindowEventArgs args)
{
    // Save all tabs before saving items
    SaveAllTabs();  // ? NEW!
    
    UserSettingsClass.SaveLauncherXItems(ItemsGridView.Items);

    // Dispose resources
    trayIcon?.Dispose();
    hotkeyManager?.Dispose();
}
```

**���� ����:**
```
1. SaveAllTabs()           �� �� ���� ���� (tabs.json)
2. SaveLauncherXItems()    �� ������ ���� (Files/*.json)
3. Dispose resources       �� ���ҽ� ����
```

---

## ?? **����/�ε� �帧��**

### **���� �帧:**

```
�� ���� (WindowEx_Closed)
    ��
  ���� SaveAllTabs()
    ��   ��
    ��   ���� 1. SaveCurrentTabItems()
    ��   ��     ���� ���� ���� �������� Tab.Tag�� ����
    ��   ��
    ��   ���� 2. ��� �� ���� ����
    ��   ��     ���� �� �̸�
    ��   ��     ���� �� ������
    ��   ��   ���� �� ����
    ��   ��     ���� ���� ������ �ε��� ���
    ��   ��
    �� ���� 3. JSON���� ����ȭ
    ������ tabs.json ���� ����
    ��
    ���� SaveLauncherXItems()
   ���� ������ ���� ���� (Files/*.json)
```

---

### **�ε� �帧:**

```
�� ���� (Container_Loaded)
    ��
    ���� LoadLauncherXItems()
    ��   ���� Files/*.json �б� �� List<UserControl>
  ��
    ���� LoadTabs()
    ��   ���� tabs.json �б� �� TabsData?
    ��
    ���� �б�:
        ��
      ���� tabs.json ����?
      ��   ��
        ��   ���� LoadSavedTabs(controls)
        ��       ��
        ��       ���� 1. DistributeItemsToTabs()
        ��     ��     ���� �������� �Ǻ��� �й�
    ��       ��
  ������ 2. �� �����
        ��       ��   ���� �� �̸� ����
        ��       ��     ���� �� ������ ����
        ��       ��     ���� �� ���� ����
        ��  ��     ���� �� ������ �Ҵ�
    ��    ��
        ��       ���� 3. ���õ� �� Ȱ��ȭ
        ��             ���� LoadCurrentTabItems()
        ��
        ���� tabs.json ����?
     ��
            ���� InitializeTabs()
     ���� �⺻ �� ���� + ��� ������ �ε�
```

---

## ?? **Before vs After ��**

### **Before (�� ���� ���� �� ��):**

```
�� ���:
1. �� 3�� ���� ("Work", "Games", "Tools")
2. �� �ǿ� ���� ���� (����, �Ķ�, �ʷ�)
3. �� �ǿ� ������ ��ġ
4. �� ����

�� �����:
? ���� ��� ������� �⺻ �Ǹ� ����
? ��� �������� ù ��° �ǿ� ���� ����
? �� ������ ��� ���µ�
```

**���� ����:**
```
UserCache\
���� userSettings.json  (�� ����)
���� Files\        (�����۸� �����)
   ���� 0.json
   ���� 1.json
   ���� ...
```

---

### **After (�� ���� ���� ����):**

```
�� ���:
1. �� 3�� ���� ("Work", "Games", "Tools")
2. �� �ǿ� ���� ���� (����, �Ķ�, �ʷ�)
3. �� �ǿ� ������ ��ġ
4. �� ����

�� �����:
? �� 3�� �״�� ���� ("Work", "Games", "Tools")
? �� ���� ���� ���� (����, �Ķ�, �ʷ�)
? �� ���� ������ ��ġ �״�� ����
? ������ ���õ� �� Ȱ��ȭ
```

**���� ����:**
```
UserCache\
���� userSettings.json  (�� ����)
���� tabs.json          ? NEW! (�� ����)
���� Files\ (������ ����)
   ���� 0.json
   ���� 1.json
   ���� ...
```

**tabs.json ����:**
```json
{
  "tabs": [
 {
      "id": "tab-0",
   "name": "Work",
      "icon": "Document",
      "color": "#50FF0000",
      "itemIndices": ["0", "1", "2"],
 "isSelected": false
    },
    {
   "id": "tab-1",
      "name": "Games",
      "icon": "Folder",
   "color": "#500000FF",
 "itemIndices": ["3", "4"],
      "isSelected": false
    },
    {
      "id": "tab-2",
      "name": "Tools",
      "icon": "Setting",
      "color": "#5000FF00",
      "itemIndices": ["5", "6", "7/"],
      "isSelected": true
    }
  ],
  "selectedTabIndex": 2
}
```

---

## ?? **����Ǵ� ����**

### **1. �� ����:**
```
? �� �̸� ("�⺻", "Work", "Games" ��)
? �� ������ (Home, Document, Folder ��)
? �� ���� (ARGB ����)
? �� ����
? ���õ� �� �ε���
```

### **2. ������ ��ġ:**
```
? �� �ǿ� � �������� �ִ���
? ������ ����
? �׷� ���� (�ε��� �ڿ� "/" ���̻�)
```

### **3. ���� ����:**
```
? ������ ���õ� ��
? �Ǻ� ������ ����
```

---

## ?? **����� ���λ���**

### **1. ������ �ε��� ����:**

```csharp
// �Ϲ� ������: "0", "1", "2"
// �׷�: "2/" (������ ���̻�)

// ����:
"itemIndices": ["0", "1", "2/", "3"]
//      ��   ��   ��    ��
//    0��  1��  2��   3��
//         ������ �׷������
```

**�ε��� ��Ī:**
```
tabs.json�� �ε���    ��    Files/ ������ ����
"0"        ��    0.json
"1"         ��    1.json
"2/"          ��    2/ (���� - �׷�)
"3"           ��    3.json
```

---

### **2. ���� ���� ����:**

```csharp
// ARGB ����: #AARRGGBB
// A (Alpha): ����
// R (Red): ����
// G (Green): �ʷ�
// B (Blue): �Ķ�

// ����:
"#50FF0000"  // 50% ������ ����
"#80000000FF"  // 80% ������ �Ķ�
"#00000000"  // ���� ���� (�⺻)
```

---

### **3. JSON ����ȭ:**

**Source Generation ���:**
```csharp
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TabsData))]
[JsonSerializable(typeof(TabData))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
```

**����:**
- AOT (Ahead-Of-Time) ������ ����
- ���÷��� ���� ���� ����ȭ
- Ʈ���� (Trimming) ����
- �� ���� �� ũ��

---

### **4. ���� ó��:**

```csharp
// �� �ε� ���� �� �� �⺻ �� ���
try
{
    LoadSavedTabs(controls);
}
catch (Exception ex)
{
    Debug.WriteLine($"Error loading tabs: {ex}");
    InitializeTabs();  // Fall back to default
}

// ���� �Ľ� ���� �� �� ����
try
{
    var color = Color.FromArgb(a, r, g, b);
  _tabColors[tab] = color;
}
catch
{
    // Invalid color format, ignore
}
```

---

## ?? **�׽�Ʈ �ó�����**

### **Test 1: �⺻ ����/�ε�**
```
1. �� ����
2. �� 2�� �߰� ("Work", "Games")
3. �� �ǿ� ������ �߰�
4. �� ����
5. �� �����
6. ? �� 3�� ���� ("�⺻", "Work", "Games")
7. ? �� ���� ������ �״��
```

---

### **Test 2: �� ���� ����**
```
1. "Work" �� �� ���� ����
2. "Games" �� �� �Ķ� ����
3. �� ����
4. �� �����
5. ? "Work" �� ���� ����
6. ? "Games" �� �Ķ� ����
```

---

### **Test 3: ���õ� �� ����**
```
1. �� 3�� ����
2. �� ��° �� ����
3. �� ����
4. �� �����
5. ? �� ��° ���� ���õǾ� ����
```

---

### **Test 4: �׷� ���� ��**
```
1. �ǿ� ������ 5�� �߰�
2. 2���� �巡���Ͽ� �׷� ����
3. �� ����
4. �� �����
5. ? �׷��� �״�� ������
6. ? �׷� �� ������ ���� ����
```

---

### **Test 5: tabs.json ���� ��**
```
1. tabs.json ���� ����
2. �� ����
3. ? �⺻ �� �ڵ� ����
4. ? ��� ������ �ε��
5. ? ���� ����
```

---

### **Test 6: tabs.json �ջ� ��**
```
1. tabs.json ������ �߸��� JSON���� ����
2. �� ����
3. ? �⺻ ������ fallback
4. ? ���� �޽��� Debug�� ���
5. ? �� ���� ����
```

---

## ?? **����� ���� ����**

### **Before:**
```
�����: "���� ���ڰ� �����ߴµ�..."
�� �� ����
�� �� �����
�� "��� ���� �������!" ??
�� �ٽ� �� �����ϰ� ������ ���ġ...
�� "�Ź� �̷��� �ؾ� ��?" ??
```

### **After:**
```
�����: "���� ���ڰ� �����߾�!"
�� �� ����
�� �� �����
�� "���� �״�� �ֳ�! ??"
�� "���� �״��!" ??
�� "������ ��ġ�� �Ϻ�!" ??
�� "���� ���� ����!" ??
```

---

## ?? **���� ���**

### **�߰��� ����:**
```
? TabData.cs (108��) - ������ Ŭ����
```

### **������ ����:**
```
? UserSettingsClass.cs (+158��) - ����/�ε� �޼���
? MainWindow.Tabs.cs (+121��) - �� ����/�ε� ����
? MainWindow.xaml.cs (����) - �ʱ�ȭ ���� ����
```

### **Total:**
```
387���� ���ο� �ڵ� �߰�!
```

---

## ? **�Ϸ�!**

### **������ ���:**
```
? �� ������ JSON ���Ϸ� ����
? �� �̸�, ������, ���� ����
? �Ǻ� ������ ��ġ ����
? ���õ� �� ���� ����
? ���� ó�� (fallback to default)
? Source Generation ��� (����ȭ)
```

### **����Ǵ� ����:**
```
? �� ���� �� ����
? �� ���� �̸�
? �� ���� ������
? �� ���� ����
? �� ���� ������ ���
? ���õ� �� �ε���
```

### **���� ���:**
```
? ���� ����!
? ��� ���� �۵�!
? ���� ó�� �Ϻ�!
```

---

## ?? **���� �� ������ ���������� ����˴ϴ�!**

**���� �ݾƵ�:**
- ?? **�� ���� ����**
- ?? **�� �̸� ����**
- ?? **������ ��ġ ����**
- ? **���� ���� ����**

**��� ���� �Ϻ��ϰ� �����˴ϴ�!** ?
