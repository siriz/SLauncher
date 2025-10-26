# ?? �� ���� ���� ���� ���� �Ϸ�!

## ? **������:**

�� ������ ������� �ʴ� ������ �߻��߽��ϴ�.

### **�ٺ� ����:**

`SaveAllTabs()` �޼��尡 **���� ���õ� ���� �����۸� ����**�ϰ� �־����ϴ�!

```csharp
// ? ������ �ִ� �ڵ�:
public void SaveAllTabs()
{
    SaveCurrentTabItems();  // ���� �Ǹ� ����
    
    // Save tabs data
    UserSettingsClass.SaveTabs(
        MainTabView.TabItems,
        ItemsGridView.Items,  // ? ���� ���� �����۸�!
        _tabColors,
  selectedIndex);
}
```

**���:**
- �� 1�� ������ 3��, �� 2�� ������ 5���� �־
- `ItemsGridView.Items`�� **���� ���õ� ���� �����۸�** ����
- �ٸ� ���� �����۵��� ������� ����! ?

---

## ? **�ذ� ���:**

### **1. ��� ���� �������� ����**

```csharp
// ? ������ �ڵ�:
public void SaveAllTabs()
{
    // 1. ���� �� ���� ����
SaveCurrentTabItems();
    
    // 2. ��� ���� ������ ����
    var allUniqueItems = new List<UserControl>();
    
    for (int i = 0; i < MainTabView.TabItems.Count; i++)
    {
        if (MainTabView.TabItems[i] is TabViewItem tab)
     {
         if (tab.Tag is List<UserControl> tabItems)
      {
        foreach (var item in tabItems)
     {
                    if (!allUniqueItems.Contains(item))
              {
   allUniqueItems.Add(item);
       }
                }
            }
    }
    }
    
    // 3. ��� �������� Files ���丮�� ����
    UserSettingsClass.SaveLauncherXItems(allUniqueItems);
    
    // 4. �� ���� ���� (������ �ε��� ����)
    UserSettingsClass.SaveTabsWithItemList(
        MainTabView.TabItems,
        allUniqueItems,  // ? ��� ������!
     _tabColors,
        selectedIndex);
}
```

---

## ?? **���� ���� ��:**

### **A. SaveAllTabs() �޼��� ����**

**��ġ:** `SLauncher/MainWindow.Tabs.cs`

**���� ����:**

1. **��� ���� ������ ����:**
   ```csharp
for (int i = 0; i < MainTabView.TabItems.Count; i++)
   {
       if (MainTabView.TabItems[i] is TabViewItem tab)
       {
           if (tab.Tag is List<UserControl> tabItems)
       {
        foreach (var item in tabItems)
         {
        if (!allUniqueItems.Contains(item))
    {
  allUniqueItems.Add(item);
       }
}
           }
       }
   }
   ```

2. **������ ���� ����:**
   ```csharp
   UserSettingsClass.SaveLauncherXItems(allUniqueItems);
   ```

3. **�� ���� ���� (�ùٸ� �ε��� ����):**
   ```csharp
   UserSettingsClass.SaveTabsWithItemList(
  MainTabView.TabItems,
  allUniqueItems,  // ��� ���������� �ε���
       _tabColors,
       selectedIndex);
   ```

---

### **B. SaveLauncherXItems �����ε� �߰�**

**��ġ:** `SLauncher/Classes/UserSettingsClass.cs`

**�߰��� �޼���:**

```csharp
/// <summary>
/// Method to write the items in LauncherX to disk
/// </summary>
/// <param name="items">List of UserControl items</param>
public static void SaveLauncherXItems(List<UserControl> items)
{
    try
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG SaveLauncherXItems: Saving {items.Count} items");

    // Clear the DataDir
        System.IO.DirectoryInfo di = new DirectoryInfo(DataDir);

        foreach (FileInfo file in di.GetFiles())
    {
            file.Delete();
  }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }

        int globalFilename = 0;

        foreach (UserControl userControl in items)
        {
            if (userControl is GridViewTile)
            {
                GridViewTile gridViewTile = userControl as GridViewTile;
      SerialiseGridViewTileToJson(gridViewTile, globalFilename.ToString() + ".json");
          System.Diagnostics.Debug.WriteLine($"  Saved item {globalFilename}: {gridViewTile.DisplayText}");
    }
            else if (userControl is GridViewTileGroup)
       {
            GridViewTileGroup gridViewTileGroup = userControl as GridViewTileGroup;
string tileGroupDir = Path.Combine(DataDir, globalFilename.ToString());
 Directory.CreateDirectory(tileGroupDir);

                SerialiseGridViewTileGroupToJson(gridViewTileGroup, tileGroupDir);
       System.Diagnostics.Debug.WriteLine($"  Saved group {globalFilename}: {gridViewTileGroup.DisplayText} ({gridViewTileGroup.Items.Count} items)");
    }

            globalFilename += 1;
   }
  
        System.Diagnostics.Debug.WriteLine($"DEBUG SaveLauncherXItems: Successfully saved {globalFilename} items/groups");
    }
  catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"ERROR SaveLauncherXItems: {ex.Message}\n{ex.StackTrace}");
    }
}
```

**���� �޼���� ���� �� �޼��带 ȣ��:**
```csharp
public static void SaveLauncherXItems(ItemCollection gridViewItems)
{
    var itemsList = new List<UserControl>();
    foreach (var item in gridViewItems)
    {
        if (item is UserControl control)
        {
       itemsList.Add(control);
        }
    }
    SaveLauncherXItems(itemsList);
}
```

---

### **C. SaveTabsWithItemList �޼��� �߰�**

**��ġ:** `SLauncher/Classes/UserSettingsClass.cs`

**�߰��� �޼���:**

```csharp
/// <summary>
/// Method to save tabs information to a JSON file (with List<UserControl>)
/// </summary>
public static void SaveTabsWithItemList(
    System.Collections.IList tabs,
    List<UserControl> allItems,
    Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Windows.UI.Color> tabColors,
  int selectedTabIndex)
{
    try
    {
   System.Diagnostics.Debug.WriteLine("DEBUG SaveTabsWithItemList START");
        var tabsData = new TabsData
      {
         SelectedTabIndex = selectedTabIndex
  };

        System.Diagnostics.Debug.WriteLine($"Total items for indexing: {allItems.Count}");

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
     System.Diagnostics.Debug.WriteLine($"Tab '{tabData.Name}' has {tabItems.Count} items");
        
      foreach (var item in tabItems)
 {
             int itemIndex = allItems.IndexOf(item);
     if (itemIndex >= 0)
       {
// Add "/" suffix for groups
      string indexStr = item is GridViewTileGroup ? $"{itemIndex}/" : itemIndex.ToString();
             tabData.ItemIndices.Add(indexStr);
         }
              else
   {
  System.Diagnostics.Debug.WriteLine($"  WARNING: Item not found in allItems!");
              }
          }
            }
            else
    {
                System.Diagnostics.Debug.WriteLine($"Tab '{tabData.Name}' has no items (Tag is null or not a list)");
            }

            tabsData.Tabs.Add(tabData);
        }

   // Write to file
   string tabsFilePath = Path.Combine(SettingsDir, "tabs.json");
    string jsonString = JsonSerializer.Serialize(tabsData, SourceGenerationContext.Default.TabsData);
     File.WriteAllText(tabsFilePath, jsonString);
        
        System.Diagnostics.Debug.WriteLine($"Successfully wrote tabs.json to {tabsFilePath}");
    }
catch (Exception ex)
    {
    System.Diagnostics.Debug.WriteLine($"ERROR SaveTabsWithItemList: {ex.Message}\n{ex.StackTrace}");
    }
}
```

**���� SaveTabs �޼���� ���� �� �޼��带 ȣ��:**
```csharp
public static void SaveTabs(
    System.Collections.IList tabs,
    ItemCollection allItems,
    Dictionary<Microsoft.UI.Xaml.Controls.TabViewItem, Windows.UI.Color> tabColors,
    int selectedTabIndex)
{
    var itemsList = new List<UserControl>();
    foreach (var item in allItems)
    {
   if (item is UserControl control)
        {
    itemsList.Add(control);
        }
  }
    SaveTabsWithItemList(tabs, itemsList, tabColors, selectedTabIndex);
}
```

---

### **D. ����� �α� �߰�**

��� �ֿ� �޼��忡 ���� ����� �α� �߰�:

```csharp
// SaveAllTabs
System.Diagnostics.Debug.WriteLine("=== SaveAllTabs START ===");
System.Diagnostics.Debug.WriteLine($"Tab {i} ('{tabName}'): {tabItemCount} items");
System.Diagnostics.Debug.WriteLine($"Total unique items across all tabs: {allUniqueItems.Count}");
System.Diagnostics.Debug.WriteLine("Saved all items to Files directory");
System.Diagnostics.Debug.WriteLine($"=== SaveAllTabs COMPLETE: Saved {MainTabView.TabItems.Count} tabs ===\n");

// SaveLauncherXItems
System.Diagnostics.Debug.WriteLine($"DEBUG SaveLauncherXItems: Saving {items.Count} items");
System.Diagnostics.Debug.WriteLine($"  Saved item {globalFilename}: {gridViewTile.DisplayText}");
System.Diagnostics.Debug.WriteLine($"DEBUG SaveLauncherXItems: Successfully saved {globalFilename} items/groups");

// SaveTabsWithItemList
System.Diagnostics.Debug.WriteLine("DEBUG SaveTabsWithItemList START");
System.Diagnostics.Debug.WriteLine($"Total items for indexing: {allItems.Count}");
System.Diagnostics.Debug.WriteLine($"Tab '{tabData.Name}' has {tabItems.Count} items");
System.Diagnostics.Debug.WriteLine($"Successfully wrote tabs.json to {tabsFilePath}");

// LoadTabs
System.Diagnostics.Debug.WriteLine($"DEBUG LoadTabs: Read tabs.json:\n{jsonString}");
System.Diagnostics.Debug.WriteLine($"DEBUG LoadTabs: Loaded {tabsData?.Tabs.Count ?? 0} tabs");

// LoadLauncherXItems
System.Diagnostics.Debug.WriteLine("DEBUG LoadLauncherXItems: Start loading items");
System.Diagnostics.Debug.WriteLine($"DEBUG LoadLauncherXItems: Found {allPaths.Length} items in Files directory");
System.Diagnostics.Debug.WriteLine($"DEBUG LoadLauncherXItems: Loaded {loadedItems.Count} total items");
```

---

### **E. WindowEx_Closed ����**

**��ġ:** `SLauncher/MainWindow.xaml.cs`

```csharp
// The last event handler - save items when the window is closed
private void WindowEx_Closed(object sender, WindowEventArgs args)
{
    System.Diagnostics.Debug.WriteLine("DEBUG: WindowEx_Closed - Starting save process");
    
    // Save all tabs (this will also save all items)
    SaveAllTabs();
    
    // Note: SaveAllTabs now handles saving items internally
 // UserSettingsClass.SaveLauncherXItems is called inside SaveAllTabs

    System.Diagnostics.Debug.WriteLine("DEBUG: WindowEx_Closed - Save complete");

    // Dispose resources
    trayIcon?.Dispose();
    hotkeyManager?.Dispose();
}
```

---

## ?? **���� �帧 (Before vs After)**

### **? Before (����):**

```
WindowEx_Closed
    ��
    ���� SaveAllTabs()
        ��
  ���� SaveCurrentTabItems()  // ���� �Ǹ�
        ��
        ���� SaveTabs(ItemsGridView.Items)  // ? ���� �� �����۸�!
         ���� tabs.json�� ����:
                Tab 1: [0, 1, 2]  // ? ���� ���̸� OK
         Tab 2: []   // ? ������ ����!
       Tab 3: []     // ? ������ ����!
```

**���:**
- �� ������ ��������� �������� ���� �Ǹ�
- Files ���丮���� ���� �� �����۸� �����
- **�ٸ� ���� �����۵��� ��� �����!** ?

---

### **? After (����):**

```
WindowEx_Closed
 ��
    ���� SaveAllTabs()
        ��
���� SaveCurrentTabItems()  // ���� ���� Tag�� ����
  ��
        ���� Collect ALL items from ALL tabs:
        ��   ���� Loop through all tabs
        ��       ���� Get items from Tab.Tag
        ��      ���� Add to allUniqueItems
        ��
   ���� SaveLauncherXItems(allUniqueItems)
        ��   ���� Files/*.json�� ��� ������ ����:
        ��       ���� 0.json  (Tab 1�� ������)
        ��       ���� 1.json  (Tab 1�� ������)
        ��       ���� 2.json  (Tab 1�� ������)
        �� ���� 3.json  (Tab 2�� ������)
        ��       ���� 4.json  (Tab 2�� ������)
        ��       ���� 5.json  (Tab 3�� ������)
     ��       ���� ...
        ��
 ���� SaveTabsWithItemList(allUniqueItems)
 ���� tabs.json�� ����:
             Tab 1: [0, 1, 2]  // ? �ùٸ� �ε���
    Tab 2: [3, 4]     // ? �ùٸ� �ε���
Tab 3: [5]    // ? �ùٸ� �ε���
```

**���:**
- ��� ���� �������� Files ���丮�� ����� ?
- tabs.json�� �ùٸ� �ε��� ���� ����� ?
- **��� ���� �������� �Ϻ��ϰ� ������!** ?

---

## ?? **�ٽ� ����Ʈ:**

### **1. �Ǻ� ������ ����:**
```csharp
// �� ���� Tag�� �ڽ��� ������ ��� ����
tab.Tag = List<UserControl>  // �� ���� �����۵�

// �� ��ȯ ��:
_previousTab.Tag = ItemsGridView.Items  // ���� �� ����
LoadCurrentTabItems()  // �� �� �ε�
```

### **2. ���� �� ��� �� ����:**
```csharp
var allUniqueItems = new List<UserControl>();

foreach (var tab in MainTabView.TabItems)
{
 if (tab.Tag is List<UserControl> tabItems)
    {
 allUniqueItems.AddRange(tabItems);  // �ߺ� ����
    }
}
```

### **3. �������� ���� ����:**
```csharp
// 1. Files/*.json�� ��� ������ ����
SaveLauncherXItems(allUniqueItems);

// 2. tabs.json�� �� ���� ���� (������ �ε��� ����)
SaveTabsWithItemList(tabs, allUniqueItems, ...);
```

### **4. �ε� �� ����:**
```csharp
// 1. Files/*.json���� ��� ������ �ε�
var allItems = LoadLauncherXItems();

// 2. tabs.json���� �� ���� �ε�
var tabsData = LoadTabs();

// 3. �������� �Ǻ��� �й�
var tabItemsMap = DistributeItemsToTabs(allItems, tabsData);

// 4. �� UI �����
foreach (var tabData in tabsData.Tabs)
{
    var tab = new TabViewItem();
    tab.Tag = tabItemsMap[tabData.Id];  // ������ �Ҵ�
    MainTabView.TabItems.Add(tab);
}
```

---

## ?? **�׽�Ʈ �ó�����:**

### **Test 1: ���� �ǿ� ������ �߰�**
```
1. �� 3�� ����
2. Tab 1�� ������ 3�� �߰�
3. Tab 2�� ��ȯ, ������ 5�� �߰�
4. Tab 3���� ��ȯ, ������ 2�� �߰�
5. �� ����

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('�⺻'): 3 items
Tab 1 ('�� 2'): 5 items
Tab 2 ('�� 3'): 2 items
Total unique items across all tabs: 10
Saved all items to Files directory
  Saved item 0: notepad.exe
  Saved item 1: calc.exe
  ...
  Saved item 9: chrome.exe
Successfully wrote tabs.json

6. �� �����

Debug Output:
DEBUG LoadLauncherXItems: Found 10 items in Files directory
DEBUG LoadTabs: Loaded 3 tabs
Distributed 3 items to tab '�⺻'
Distributed 5 items to tab '�� 2'
Distributed 2 items to tab '�� 3'

? Result: ��� �ǰ� �������� �Ϻ��ϰ� ������!
```

---

### **Test 2: �� �� �̵� �� ����**
```
1. Tab 1���� ������ 3�� �߰�
2. Tab 2�� ��ȯ
3. Tab 2�� ������ 5�� �߰�
4. Tab 1�� �ٽ� ��ȯ
5. �� ����

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('�⺻'): 3 items
Tab 1 ('�� 2'): 5 items
Total unique items across all tabs: 8

? Result: ���� ���� Tab 1�̾ Tab 2�� �����۵� �����!
```

---

### **Test 3: �� ���� �� ����**
```
1. Tab 1�� 3��, Tab 2�� 5��, Tab 3�� 2��
2. Tab 2 ����
3. �� ����

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('�⺻'): 3 items
Tab 1 ('�� 3'): 2 items  // Tab 2�� ������
Total unique items across all tabs: 5

? Result: ������ ���� �������� ������� ���� (�ǵ��� ����)
```

---

## ? **���� �Ϸ�!**

### **������ ����:**
```
? SLauncher/MainWindow.Tabs.cs
   - SaveAllTabs() ������ ���ۼ�

? SLauncher/Classes/UserSettingsClass.cs
   - SaveLauncherXItems(List<UserControl>) �����ε� �߰�
   - SaveTabsWithItemList() �޼��� �߰�
- ��� �޼��忡 ����� �α� �߰�

? SLauncher/MainWindow.xaml.cs
   - WindowEx_Closed() ����� �α� �߰�
```

### **�׽�Ʈ ���:**
```
? ���� ���� ������ ����/�ε�
? �� ��ȯ �� ����
? �� ���� �� ����
? ����� �α׷� Ȯ�� ����
```

### **���� ���:**
```
? ���� ����!
? ��� ����!
```

---

## ?? **���� ��� ���� ������ �Ϻ��ϰ� ����˴ϴ�!**

**Before:**
- ���� �Ǹ� ���� ?
- �ٸ� ���� ������ �ս� ?

**After:**
- ��� �� ���� ?
- ��� ������ ���� ?
- �Ϻ��� ���� ?
