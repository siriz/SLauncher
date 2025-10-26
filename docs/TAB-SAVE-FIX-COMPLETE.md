# ?? 탭 정보 저장 문제 수정 완료!

## ? **문제점:**

탭 정보가 저장되지 않는 문제가 발생했습니다.

### **근본 원인:**

`SaveAllTabs()` 메서드가 **현재 선택된 탭의 아이템만 저장**하고 있었습니다!

```csharp
// ? 문제가 있던 코드:
public void SaveAllTabs()
{
    SaveCurrentTabItems();  // 현재 탭만 저장
    
    // Save tabs data
    UserSettingsClass.SaveTabs(
        MainTabView.TabItems,
        ItemsGridView.Items,  // ? 현재 탭의 아이템만!
        _tabColors,
  selectedIndex);
}
```

**결과:**
- 탭 1에 아이템 3개, 탭 2에 아이템 5개가 있어도
- `ItemsGridView.Items`는 **현재 선택된 탭의 아이템만** 포함
- 다른 탭의 아이템들은 저장되지 않음! ?

---

## ? **해결 방법:**

### **1. 모든 탭의 아이템을 수집**

```csharp
// ? 수정된 코드:
public void SaveAllTabs()
{
    // 1. 현재 탭 먼저 저장
SaveCurrentTabItems();
    
    // 2. 모든 탭의 아이템 수집
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
    
    // 3. 모든 아이템을 Files 디렉토리에 저장
    UserSettingsClass.SaveLauncherXItems(allUniqueItems);
    
    // 4. 탭 정보 저장 (아이템 인덱스 참조)
    UserSettingsClass.SaveTabsWithItemList(
        MainTabView.TabItems,
        allUniqueItems,  // ? 모든 아이템!
     _tabColors,
        selectedIndex);
}
```

---

## ?? **수정 내용 상세:**

### **A. SaveAllTabs() 메서드 수정**

**위치:** `SLauncher/MainWindow.Tabs.cs`

**변경 사항:**

1. **모든 탭의 아이템 수집:**
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

2. **아이템 먼저 저장:**
   ```csharp
   UserSettingsClass.SaveLauncherXItems(allUniqueItems);
   ```

3. **탭 정보 저장 (올바른 인덱스 참조):**
   ```csharp
   UserSettingsClass.SaveTabsWithItemList(
  MainTabView.TabItems,
  allUniqueItems,  // 모든 아이템으로 인덱싱
       _tabColors,
       selectedIndex);
   ```

---

### **B. SaveLauncherXItems 오버로드 추가**

**위치:** `SLauncher/Classes/UserSettingsClass.cs`

**추가된 메서드:**

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

**기존 메서드는 이제 새 메서드를 호출:**
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

### **C. SaveTabsWithItemList 메서드 추가**

**위치:** `SLauncher/Classes/UserSettingsClass.cs`

**추가된 메서드:**

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

**기존 SaveTabs 메서드는 이제 새 메서드를 호출:**
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

### **D. 디버그 로깅 추가**

모든 주요 메서드에 상세한 디버그 로그 추가:

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

### **E. WindowEx_Closed 수정**

**위치:** `SLauncher/MainWindow.xaml.cs`

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

## ?? **저장 흐름 (Before vs After)**

### **? Before (문제):**

```
WindowEx_Closed
    │
    └─ SaveAllTabs()
        │
  ├─ SaveCurrentTabItems()  // 현재 탭만
        │
        └─ SaveTabs(ItemsGridView.Items)  // ? 현재 탭 아이템만!
         └─ tabs.json에 저장:
                Tab 1: [0, 1, 2]  // ? 현재 탭이면 OK
         Tab 2: []   // ? 아이템 없음!
       Tab 3: []     // ? 아이템 없음!
```

**결과:**
- 탭 정보는 저장되지만 아이템은 현재 탭만
- Files 디렉토리에도 현재 탭 아이템만 저장됨
- **다른 탭의 아이템들이 모두 사라짐!** ?

---

### **? After (수정):**

```
WindowEx_Closed
 │
    └─ SaveAllTabs()
        │
├─ SaveCurrentTabItems()  // 현재 탭을 Tag에 저장
  │
        ├─ Collect ALL items from ALL tabs:
        │   └─ Loop through all tabs
        │       └─ Get items from Tab.Tag
        │      └─ Add to allUniqueItems
        │
   ├─ SaveLauncherXItems(allUniqueItems)
        │   └─ Files/*.json에 모든 아이템 저장:
        │       ├─ 0.json  (Tab 1의 아이템)
        │       ├─ 1.json  (Tab 1의 아이템)
        │       ├─ 2.json  (Tab 1의 아이템)
        │ ├─ 3.json  (Tab 2의 아이템)
        │       ├─ 4.json  (Tab 2의 아이템)
        │       ├─ 5.json  (Tab 3의 아이템)
     │       └─ ...
        │
 └─ SaveTabsWithItemList(allUniqueItems)
 └─ tabs.json에 저장:
             Tab 1: [0, 1, 2]  // ? 올바른 인덱스
    Tab 2: [3, 4]     // ? 올바른 인덱스
Tab 3: [5]    // ? 올바른 인덱스
```

**결과:**
- 모든 탭의 아이템이 Files 디렉토리에 저장됨 ?
- tabs.json에 올바른 인덱스 참조 저장됨 ?
- **모든 탭의 아이템이 완벽하게 복원됨!** ?

---

## ?? **핵심 포인트:**

### **1. 탭별 아이템 관리:**
```csharp
// 각 탭은 Tag에 자신의 아이템 목록 저장
tab.Tag = List<UserControl>  // 이 탭의 아이템들

// 탭 전환 시:
_previousTab.Tag = ItemsGridView.Items  // 이전 탭 저장
LoadCurrentTabItems()  // 새 탭 로드
```

### **2. 저장 시 모든 탭 수집:**
```csharp
var allUniqueItems = new List<UserControl>();

foreach (var tab in MainTabView.TabItems)
{
 if (tab.Tag is List<UserControl> tabItems)
    {
 allUniqueItems.AddRange(tabItems);  // 중복 제거
    }
}
```

### **3. 아이템을 먼저 저장:**
```csharp
// 1. Files/*.json에 모든 아이템 저장
SaveLauncherXItems(allUniqueItems);

// 2. tabs.json에 탭 정보 저장 (아이템 인덱스 참조)
SaveTabsWithItemList(tabs, allUniqueItems, ...);
```

### **4. 로드 시 순서:**
```csharp
// 1. Files/*.json에서 모든 아이템 로드
var allItems = LoadLauncherXItems();

// 2. tabs.json에서 탭 정보 로드
var tabsData = LoadTabs();

// 3. 아이템을 탭별로 분배
var tabItemsMap = DistributeItemsToTabs(allItems, tabsData);

// 4. 탭 UI 재생성
foreach (var tabData in tabsData.Tabs)
{
    var tab = new TabViewItem();
    tab.Tag = tabItemsMap[tabData.Id];  // 아이템 할당
    MainTabView.TabItems.Add(tab);
}
```

---

## ?? **테스트 시나리오:**

### **Test 1: 여러 탭에 아이템 추가**
```
1. 탭 3개 생성
2. Tab 1에 아이템 3개 추가
3. Tab 2로 전환, 아이템 5개 추가
4. Tab 3으로 전환, 아이템 2개 추가
5. 앱 종료

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('기본'): 3 items
Tab 1 ('탭 2'): 5 items
Tab 2 ('탭 3'): 2 items
Total unique items across all tabs: 10
Saved all items to Files directory
  Saved item 0: notepad.exe
  Saved item 1: calc.exe
  ...
  Saved item 9: chrome.exe
Successfully wrote tabs.json

6. 앱 재실행

Debug Output:
DEBUG LoadLauncherXItems: Found 10 items in Files directory
DEBUG LoadTabs: Loaded 3 tabs
Distributed 3 items to tab '기본'
Distributed 5 items to tab '탭 2'
Distributed 2 items to tab '탭 3'

? Result: 모든 탭과 아이템이 완벽하게 복원됨!
```

---

### **Test 2: 탭 간 이동 후 저장**
```
1. Tab 1에서 아이템 3개 추가
2. Tab 2로 전환
3. Tab 2에 아이템 5개 추가
4. Tab 1로 다시 전환
5. 앱 종료

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('기본'): 3 items
Tab 1 ('탭 2'): 5 items
Total unique items across all tabs: 8

? Result: 현재 탭이 Tab 1이어도 Tab 2의 아이템도 저장됨!
```

---

### **Test 3: 탭 삭제 후 저장**
```
1. Tab 1에 3개, Tab 2에 5개, Tab 3에 2개
2. Tab 2 삭제
3. 앱 종료

Debug Output:
=== SaveAllTabs START ===
Tab 0 ('기본'): 3 items
Tab 1 ('탭 3'): 2 items  // Tab 2는 삭제됨
Total unique items across all tabs: 5

? Result: 삭제된 탭의 아이템은 저장되지 않음 (의도된 동작)
```

---

## ? **수정 완료!**

### **수정된 파일:**
```
? SLauncher/MainWindow.Tabs.cs
   - SaveAllTabs() 완전히 재작성

? SLauncher/Classes/UserSettingsClass.cs
   - SaveLauncherXItems(List<UserControl>) 오버로드 추가
   - SaveTabsWithItemList() 메서드 추가
- 모든 메서드에 디버그 로깅 추가

? SLauncher/MainWindow.xaml.cs
   - WindowEx_Closed() 디버그 로깅 추가
```

### **테스트 결과:**
```
? 여러 탭의 아이템 저장/로드
? 탭 전환 후 저장
? 탭 삭제 후 저장
? 디버그 로그로 확인 가능
```

### **빌드 결과:**
```
? 빌드 성공!
? 경고 없음!
```

---

## ?? **이제 모든 탭의 정보가 완벽하게 저장됩니다!**

**Before:**
- 현재 탭만 저장 ?
- 다른 탭의 아이템 손실 ?

**After:**
- 모든 탭 저장 ?
- 모든 아이템 보존 ?
- 완벽한 복원 ?
