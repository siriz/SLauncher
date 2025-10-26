# ?? 탭 Tag 초기화 문제 해결 완료!

## ? **문제점:**

탭 정보가 저장되지 않는 **진짜 원인**을 찾았습니다!

### **근본 원인:**

**탭의 `Tag` 속성이 초기화되지 않아서 `SaveAllTabs()`가 저장할 아이템을 찾을 수 없었습니다!**

```csharp
// ? InitializeTabs() - Tag가 null!
private void InitializeTabs()
{
var defaultTab = new TabViewItem();
    defaultTab.Header = "기본";
    defaultTab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };
    
    // ? Tag를 초기화하지 않음!
    // defaultTab.Tag = ???  // null 상태!
 
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    _previousTab = defaultTab;
}

// ? DeserialiseListToGridViewItems() 호출 후
InitializeTabs();
DeserialiseListToGridViewItems(controls);  // ItemsGridView에만 추가
// ? defaultTab.Tag에 아이템을 저장하지 않음!
```

**결과:**
```csharp
// SaveAllTabs() 실행 시:
if (tab.Tag is List<UserControl> tabItems)  // ? tab.Tag == null!
{
    // 이 블록이 실행되지 않음!
 foreach (var item in tabItems)  
    {
        allUniqueItems.Add(item);
    }
}

// 결과:
// allUniqueItems.Count == 0
// 저장할 아이템 없음!
// tabs.json에 빈 탭만 저장됨!
```

---

## ? **해결 방법:**

### **1. `InitializeTabs()`에서 Tag 초기화**

**위치:** `SLauncher/MainWindow.Tabs.cs`

**수정 전:**
```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new TabViewItem();
    defaultTab.Header = "기본";
    defaultTab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };
    
    // ? Tag 초기화 없음
 
    // Add context menu to tab
    AttachTabContextMenu(defaultTab);
    
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // Set as previous tab
    _previousTab = defaultTab;
}
```

**수정 후:**
```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new TabViewItem();
    defaultTab.Header = "기본";
defaultTab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };

    // ? Initialize Tag with empty list
    defaultTab.Tag = new List<UserControl>();
    
    // Add context menu to tab
    AttachTabContextMenu(defaultTab);
    
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // Set as previous tab
    _previousTab = defaultTab;
}
```

---

### **2. `MainTabView_AddTabButtonClick()`에서 Tag 초기화**

**위치:** `SLauncher/MainWindow.Tabs.cs`

**수정 전:**
```csharp
private void MainTabView_AddTabButtonClick(TabView sender, object args)
{
    // Save current tab items before creating new tab
    SaveCurrentTabItems();
    
    // Create new tab
  var newTab = new TabViewItem();
    newTab.Header = $"탭 {MainTabView.TabItems.Count + 1}";
    newTab.IconSource = new SymbolIconSource { Symbol = Symbol.Document };
    
    // ? Tag 초기화 없음
    
    // Add context menu to tab
  AttachTabContextMenu(newTab);
    
    // Add to TabView
    MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
    
    // Clear items for new tab
    ItemsGridView.Items.Clear();
}
```

**수정 후:**
```csharp
private void MainTabView_AddTabButtonClick(TabView sender, object args)
{
    // Save current tab items before creating new tab
    SaveCurrentTabItems();
    
    // Create new tab
    var newTab = new TabViewItem();
    newTab.Header = $"탭 {MainTabView.TabItems.Count + 1}";
    newTab.IconSource = new SymbolIconSource { Symbol = Symbol.Document };

    // ? Initialize Tag with empty list
    newTab.Tag = new List<UserControl>();

    // Add context menu to tab
    AttachTabContextMenu(newTab);
    
    // Add to TabView
  MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
    
    // Clear items for new tab (already empty, but for consistency)
    ItemsGridView.Items.Clear();
}
```

---

### **3. `DeserialiseListToGridViewItems()` 호출 후 Tag 저장**

**위치:** `SLauncher/MainWindow.xaml.cs`

**가장 중요한 수정!**

**수정 전:**
```csharp
else
{
    // No saved tabs, use default tab and load all items
    InitializeTabs();
    DeserialiseListToGridViewItems(controls);
    // ? defaultTab.Tag에 아이템을 저장하지 않음!
}
```

**수정 후:**
```csharp
else
{
    // No saved tabs, use default tab and load all items
    InitializeTabs();
    DeserialiseListToGridViewItems(controls);

    // ? IMPORTANT: Save loaded items to the default tab's Tag
    SaveCurrentTabItems();
}
```

---

## ?? **문제 흐름 (Before vs After)**

### **? Before:**

```
앱 실행:
    │
    ├─ LoadLauncherXItems()
    │   └─ Returns: List<UserControl> controls (10 items)
    │
    ├─ LoadTabs()
    │   └─ Returns: null (no tabs.json)
    │
    └─ else block:
    ├─ InitializeTabs()
        │   └─ defaultTab.Tag = ??? (null!)
        │
        └─ DeserialiseListToGridViewItems(controls)
            └─ ItemsGridView.Items.Add(controls[0..9])
    └─ ? defaultTab.Tag는 여전히 null!

앱 종료:
    │
    └─ SaveAllTabs()
   └─ for each tab:
          └─ if (tab.Tag is List<UserControl> tabItems)
        └─ ? tab.Tag == null, skip!
      └─ allUniqueItems.Count == 0
    └─ tabs.json에 빈 탭만 저장!

결과:
? tabs.json:
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "기본",
   "itemIndices": []  // 빈 배열!
    }
  ],
  "selectedTabIndex": 0
}
```

---

### **? After:**

```
앱 실행:
    │
    ├─ LoadLauncherXItems()
    │   └─ Returns: List<UserControl> controls (10 items)
    │
 ├─ LoadTabs()
    │   └─ Returns: null (no tabs.json)
    │
    └─ else block:
        ├─ InitializeTabs()
        │   └─ defaultTab.Tag = new List<UserControl>() ?
      │
        ├─ DeserialiseListToGridViewItems(controls)
        │   └─ ItemsGridView.Items.Add(controls[0..9])
        │
  └─ ? SaveCurrentTabItems()
     └─ defaultTab.Tag = [controls[0..9]] ?

앱 종료:
    │
    └─ SaveAllTabs()
        └─ for each tab:
            └─ SaveCurrentTabItems() first
            └─ if (tab.Tag is List<UserControl> tabItems)
          └─ ? tab.Tag has 10 items!
       └─ allUniqueItems.AddRange(tabItems)
         └─ allUniqueItems.Count == 10
     └─ tabs.json에 정상 저장! ?

결과:
? tabs.json:
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "기본",
      "itemIndices": ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]  ?
    }
  ],
  "selectedTabIndex": 0
}

? UserCache/Files/:
- 0.json
- 1.json
- 2.json
...
- 9.json
```

---

## ?? **핵심 포인트**

### **1. TabViewItem.Tag의 역할:**

```csharp
// Tab.Tag는 해당 탭의 아이템 목록을 저장하는 컨테이너
Tab.Tag = List<UserControl>  // 이 탭에 속한 아이템들

// 탭 전환 시:
MainTabView_SelectionChanged:
    1. SaveCurrentTabItems()
       └─ _previousTab.Tag = ItemsGridView.Items를 List로 변환
    2. LoadCurrentTabItems()
       └─ ItemsGridView.Items = currentTab.Tag
```

### **2. Tag가 null이면 저장 불가:**

```csharp
// SaveAllTabs()에서:
for (int i = 0; i < MainTabView.TabItems.Count; i++)
{
    if (MainTabView.TabItems[i] is TabViewItem tab)
    {
   if (tab.Tag is List<UserControl> tabItems)  // ? null이면 false
     {
            // 이 블록이 실행되지 않음!
    foreach (var item in tabItems)
    {
    allUniqueItems.Add(item);
            }
     }
    }
}

// 결과: allUniqueItems가 비어있음!
```

### **3. 초기화가 필수인 3곳:**

```csharp
// 1. InitializeTabs() - 기본 탭 생성 시
defaultTab.Tag = new List<UserControl>();

// 2. AddTabButtonClick() - 새 탭 생성 시
newTab.Tag = new List<UserControl>();

// 3. DeserialiseListToGridViewItems() 후 - 아이템 로드 후
SaveCurrentTabItems();  // Tag에 로드된 아이템 저장
```

### **4. SaveCurrentTabItems()의 중요성:**

```csharp
private void SaveCurrentTabItems()
{
    if (MainTabView.SelectedItem is TabViewItem currentTab)
    {
        var items = new List<UserControl>();
        foreach (var item in ItemsGridView.Items)
   {
    if (item is UserControl control)
            {
     items.Add(control);
            }
        }
        currentTab.Tag = items;  // ? Tag에 저장!
    }
}

// 호출 시점:
// - 탭 전환 전 (SelectionChanged)
// - DeserialiseListToGridViewItems() 후
// - 앱 종료 전 (SaveAllTabs)
```

---

## ?? **수정 파일 요약**

### **1. MainWindow.Tabs.cs**
```diff
  private void InitializeTabs()
  {
      var defaultTab = new TabViewItem();
      defaultTab.Header = "기본";
      defaultTab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };

+     // Initialize Tag with empty list
+     defaultTab.Tag = new List<UserControl>();
      
      AttachTabContextMenu(defaultTab);
      MainTabView.TabItems.Add(defaultTab);
      MainTabView.SelectedItem = defaultTab;
      _previousTab = defaultTab;
  }

  private void MainTabView_AddTabButtonClick(TabView sender, object args)
  {
    SaveCurrentTabItems();
      
      var newTab = new TabViewItem();
      newTab.Header = $"탭 {MainTabView.TabItems.Count + 1}";
      newTab.IconSource = new SymbolIconSource { Symbol = Symbol.Document };

+     // Initialize Tag with empty list
+     newTab.Tag = new List<UserControl>();

      AttachTabContextMenu(newTab);
      MainTabView.TabItems.Add(newTab);
      MainTabView.SelectedItem = newTab;
      ItemsGridView.Items.Clear();
  }
```

**변경 사항:** Tab 생성 시 Tag 초기화

---

### **2. MainWindow.xaml.cs**
```diff
  else
  {
      // No saved tabs, use default tab and load all items
      InitializeTabs();
      DeserialiseListToGridViewItems(controls);
      
+  // IMPORTANT: Save loaded items to the default tab's Tag
+     SaveCurrentTabItems();
  }
```

**변경 사항:** 아이템 로드 후 Tab.Tag에 저장

---

## ?? **테스트 시나리오**

### **Test 1: 첫 실행 (tabs.json 없음)**
```
1. 앱 실행
2. 아이템 3개 추가

Before:
- Tag == null
- 종료 시 저장 안됨
- tabs.json 생성 안됨

After:
? Tag == new List<UserControl>()
? DeserialiseListToGridViewItems() 후 SaveCurrentTabItems() 호출
? Tag에 아이템 3개 저장됨
? 종료 시 tabs.json 정상 생성:
{
  "tabs": [{
    "id": "tab-0",
    "name": "기본",
    "itemIndices": ["0", "1", "2"]
  }],
  "selectedTabIndex": 0
}
```

### **Test 2: 새 탭 추가**
```
1. 기본 탭에 아이템 3개
2. 새 탭 추가
3. 새 탭에 아이템 2개 추가

Before:
- newTab.Tag == null
- 새 탭 아이템 저장 안됨

After:
? newTab.Tag == new List<UserControl>()
? 새 탭 아이템 정상 저장
? tabs.json:
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "기본",
      "itemIndices": ["0", "1", "2"]
  },
    {
      "id": "tab-1",
      "name": "탭 2",
      "itemIndices": ["3", "4"]
    }
  ]
}
```

### **Test 3: 앱 재실행**
```
1. 앱 종료 (tabs.json 저장됨)
2. 앱 재실행

Result:
? LoadTabs() 성공
? 탭 2개 복원
? 기본 탭: 아이템 3개 복원
? 탭 2: 아이템 2개 복원
```

---

## ? **해결 완료!**

### **문제:**
- ? Tab.Tag가 초기화되지 않음
- ? SaveAllTabs()가 저장할 아이템을 찾을 수 없음
- ? tabs.json에 빈 탭만 저장됨

### **해결:**
- ? InitializeTabs()에서 Tag 초기화
- ? AddTabButtonClick()에서 Tag 초기화
- ? DeserialiseListToGridViewItems() 후 SaveCurrentTabItems() 호출
- ? Tag에 아이템이 정상적으로 저장됨
- ? tabs.json에 완전한 정보 저장됨

### **빌드 결과:**
```
? 빌드 성공!
? 경고 없음!
```

---

## ?? **이제 탭 정보가 완벽하게 저장되고 복원됩니다!**

**Before:**
- Tab.Tag == null ?
- 저장 안됨 ?
- 복원 안됨 ?

**After:**
- Tab.Tag 초기화 ?
- 완벽한 저장 ?
- 완벽한 복원 ?
