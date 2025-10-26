# ?? �� Tag �ʱ�ȭ ���� �ذ� �Ϸ�!

## ? **������:**

�� ������ ������� �ʴ� **��¥ ����**�� ã�ҽ��ϴ�!

### **�ٺ� ����:**

**���� `Tag` �Ӽ��� �ʱ�ȭ���� �ʾƼ� `SaveAllTabs()`�� ������ �������� ã�� �� �������ϴ�!**

```csharp
// ? InitializeTabs() - Tag�� null!
private void InitializeTabs()
{
var defaultTab = new TabViewItem();
    defaultTab.Header = "�⺻";
    defaultTab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };
    
    // ? Tag�� �ʱ�ȭ���� ����!
    // defaultTab.Tag = ???  // null ����!
 
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    _previousTab = defaultTab;
}

// ? DeserialiseListToGridViewItems() ȣ�� ��
InitializeTabs();
DeserialiseListToGridViewItems(controls);  // ItemsGridView���� �߰�
// ? defaultTab.Tag�� �������� �������� ����!
```

**���:**
```csharp
// SaveAllTabs() ���� ��:
if (tab.Tag is List<UserControl> tabItems)  // ? tab.Tag == null!
{
    // �� ����� ������� ����!
 foreach (var item in tabItems)  
    {
        allUniqueItems.Add(item);
    }
}

// ���:
// allUniqueItems.Count == 0
// ������ ������ ����!
// tabs.json�� �� �Ǹ� �����!
```

---

## ? **�ذ� ���:**

### **1. `InitializeTabs()`���� Tag �ʱ�ȭ**

**��ġ:** `SLauncher/MainWindow.Tabs.cs`

**���� ��:**
```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new TabViewItem();
    defaultTab.Header = "�⺻";
    defaultTab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };
    
    // ? Tag �ʱ�ȭ ����
 
    // Add context menu to tab
    AttachTabContextMenu(defaultTab);
    
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // Set as previous tab
    _previousTab = defaultTab;
}
```

**���� ��:**
```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new TabViewItem();
    defaultTab.Header = "�⺻";
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

### **2. `MainTabView_AddTabButtonClick()`���� Tag �ʱ�ȭ**

**��ġ:** `SLauncher/MainWindow.Tabs.cs`

**���� ��:**
```csharp
private void MainTabView_AddTabButtonClick(TabView sender, object args)
{
    // Save current tab items before creating new tab
    SaveCurrentTabItems();
    
    // Create new tab
  var newTab = new TabViewItem();
    newTab.Header = $"�� {MainTabView.TabItems.Count + 1}";
    newTab.IconSource = new SymbolIconSource { Symbol = Symbol.Document };
    
    // ? Tag �ʱ�ȭ ����
    
    // Add context menu to tab
  AttachTabContextMenu(newTab);
    
    // Add to TabView
    MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
    
    // Clear items for new tab
    ItemsGridView.Items.Clear();
}
```

**���� ��:**
```csharp
private void MainTabView_AddTabButtonClick(TabView sender, object args)
{
    // Save current tab items before creating new tab
    SaveCurrentTabItems();
    
    // Create new tab
    var newTab = new TabViewItem();
    newTab.Header = $"�� {MainTabView.TabItems.Count + 1}";
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

### **3. `DeserialiseListToGridViewItems()` ȣ�� �� Tag ����**

**��ġ:** `SLauncher/MainWindow.xaml.cs`

**���� �߿��� ����!**

**���� ��:**
```csharp
else
{
    // No saved tabs, use default tab and load all items
    InitializeTabs();
    DeserialiseListToGridViewItems(controls);
    // ? defaultTab.Tag�� �������� �������� ����!
}
```

**���� ��:**
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

## ?? **���� �帧 (Before vs After)**

### **? Before:**

```
�� ����:
    ��
    ���� LoadLauncherXItems()
    ��   ���� Returns: List<UserControl> controls (10 items)
    ��
    ���� LoadTabs()
    ��   ���� Returns: null (no tabs.json)
    ��
    ���� else block:
    ���� InitializeTabs()
        ��   ���� defaultTab.Tag = ??? (null!)
        ��
        ���� DeserialiseListToGridViewItems(controls)
            ���� ItemsGridView.Items.Add(controls[0..9])
    ���� ? defaultTab.Tag�� ������ null!

�� ����:
    ��
    ���� SaveAllTabs()
   ���� for each tab:
          ���� if (tab.Tag is List<UserControl> tabItems)
        ���� ? tab.Tag == null, skip!
      ���� allUniqueItems.Count == 0
    ���� tabs.json�� �� �Ǹ� ����!

���:
? tabs.json:
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "�⺻",
   "itemIndices": []  // �� �迭!
    }
  ],
  "selectedTabIndex": 0
}
```

---

### **? After:**

```
�� ����:
    ��
    ���� LoadLauncherXItems()
    ��   ���� Returns: List<UserControl> controls (10 items)
    ��
 ���� LoadTabs()
    ��   ���� Returns: null (no tabs.json)
    ��
    ���� else block:
        ���� InitializeTabs()
        ��   ���� defaultTab.Tag = new List<UserControl>() ?
      ��
        ���� DeserialiseListToGridViewItems(controls)
        ��   ���� ItemsGridView.Items.Add(controls[0..9])
        ��
  ���� ? SaveCurrentTabItems()
     ���� defaultTab.Tag = [controls[0..9]] ?

�� ����:
    ��
    ���� SaveAllTabs()
        ���� for each tab:
            ���� SaveCurrentTabItems() first
            ���� if (tab.Tag is List<UserControl> tabItems)
          ���� ? tab.Tag has 10 items!
       ���� allUniqueItems.AddRange(tabItems)
         ���� allUniqueItems.Count == 10
     ���� tabs.json�� ���� ����! ?

���:
? tabs.json:
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "�⺻",
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

## ?? **�ٽ� ����Ʈ**

### **1. TabViewItem.Tag�� ����:**

```csharp
// Tab.Tag�� �ش� ���� ������ ����� �����ϴ� �����̳�
Tab.Tag = List<UserControl>  // �� �ǿ� ���� �����۵�

// �� ��ȯ ��:
MainTabView_SelectionChanged:
    1. SaveCurrentTabItems()
       ���� _previousTab.Tag = ItemsGridView.Items�� List�� ��ȯ
    2. LoadCurrentTabItems()
       ���� ItemsGridView.Items = currentTab.Tag
```

### **2. Tag�� null�̸� ���� �Ұ�:**

```csharp
// SaveAllTabs()����:
for (int i = 0; i < MainTabView.TabItems.Count; i++)
{
    if (MainTabView.TabItems[i] is TabViewItem tab)
    {
   if (tab.Tag is List<UserControl> tabItems)  // ? null�̸� false
     {
            // �� ����� ������� ����!
    foreach (var item in tabItems)
    {
    allUniqueItems.Add(item);
            }
     }
    }
}

// ���: allUniqueItems�� �������!
```

### **3. �ʱ�ȭ�� �ʼ��� 3��:**

```csharp
// 1. InitializeTabs() - �⺻ �� ���� ��
defaultTab.Tag = new List<UserControl>();

// 2. AddTabButtonClick() - �� �� ���� ��
newTab.Tag = new List<UserControl>();

// 3. DeserialiseListToGridViewItems() �� - ������ �ε� ��
SaveCurrentTabItems();  // Tag�� �ε�� ������ ����
```

### **4. SaveCurrentTabItems()�� �߿伺:**

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
        currentTab.Tag = items;  // ? Tag�� ����!
    }
}

// ȣ�� ����:
// - �� ��ȯ �� (SelectionChanged)
// - DeserialiseListToGridViewItems() ��
// - �� ���� �� (SaveAllTabs)
```

---

## ?? **���� ���� ���**

### **1. MainWindow.Tabs.cs**
```diff
  private void InitializeTabs()
  {
      var defaultTab = new TabViewItem();
      defaultTab.Header = "�⺻";
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
      newTab.Header = $"�� {MainTabView.TabItems.Count + 1}";
      newTab.IconSource = new SymbolIconSource { Symbol = Symbol.Document };

+     // Initialize Tag with empty list
+     newTab.Tag = new List<UserControl>();

      AttachTabContextMenu(newTab);
      MainTabView.TabItems.Add(newTab);
      MainTabView.SelectedItem = newTab;
      ItemsGridView.Items.Clear();
  }
```

**���� ����:** Tab ���� �� Tag �ʱ�ȭ

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

**���� ����:** ������ �ε� �� Tab.Tag�� ����

---

## ?? **�׽�Ʈ �ó�����**

### **Test 1: ù ���� (tabs.json ����)**
```
1. �� ����
2. ������ 3�� �߰�

Before:
- Tag == null
- ���� �� ���� �ȵ�
- tabs.json ���� �ȵ�

After:
? Tag == new List<UserControl>()
? DeserialiseListToGridViewItems() �� SaveCurrentTabItems() ȣ��
? Tag�� ������ 3�� �����
? ���� �� tabs.json ���� ����:
{
  "tabs": [{
    "id": "tab-0",
    "name": "�⺻",
    "itemIndices": ["0", "1", "2"]
  }],
  "selectedTabIndex": 0
}
```

### **Test 2: �� �� �߰�**
```
1. �⺻ �ǿ� ������ 3��
2. �� �� �߰�
3. �� �ǿ� ������ 2�� �߰�

Before:
- newTab.Tag == null
- �� �� ������ ���� �ȵ�

After:
? newTab.Tag == new List<UserControl>()
? �� �� ������ ���� ����
? tabs.json:
{
  "tabs": [
    {
      "id": "tab-0",
      "name": "�⺻",
      "itemIndices": ["0", "1", "2"]
  },
    {
      "id": "tab-1",
      "name": "�� 2",
      "itemIndices": ["3", "4"]
    }
  ]
}
```

### **Test 3: �� �����**
```
1. �� ���� (tabs.json �����)
2. �� �����

Result:
? LoadTabs() ����
? �� 2�� ����
? �⺻ ��: ������ 3�� ����
? �� 2: ������ 2�� ����
```

---

## ? **�ذ� �Ϸ�!**

### **����:**
- ? Tab.Tag�� �ʱ�ȭ���� ����
- ? SaveAllTabs()�� ������ �������� ã�� �� ����
- ? tabs.json�� �� �Ǹ� �����

### **�ذ�:**
- ? InitializeTabs()���� Tag �ʱ�ȭ
- ? AddTabButtonClick()���� Tag �ʱ�ȭ
- ? DeserialiseListToGridViewItems() �� SaveCurrentTabItems() ȣ��
- ? Tag�� �������� ���������� �����
- ? tabs.json�� ������ ���� �����

### **���� ���:**
```
? ���� ����!
? ��� ����!
```

---

## ?? **���� �� ������ �Ϻ��ϰ� ����ǰ� �����˴ϴ�!**

**Before:**
- Tab.Tag == null ?
- ���� �ȵ� ?
- ���� �ȵ� ?

**After:**
- Tab.Tag �ʱ�ȭ ?
- �Ϻ��� ���� ?
- �Ϻ��� ���� ?
