# ?? �� ��ȯ �� ������ �и� ���� �ذ�!

## ? ����

**����:**
```
1. �⺻ �ǿ� App1, App2 �߰�
2. "+" ��ư���� �� 2 ����
3. �� 2�� Game1, Game2 �߰�
4. �⺻ ������ ��ȯ
5. ? �⺻ �ǿ� Game1, Game2�� ǥ�õ�!
6. ? �� ���� ���� ������ ����
```

**����:**
```csharp
private void MainTabView_SelectionChanged(...)
{
    SaveCurrentTabItems();  // ? ���� �� ���� (�̹� ��ȯ�� ��)
    LoadCurrentTabItems();  // �� �� �ε�
}
```

����: `SelectionChanged` �̺�Ʈ�� �߻����� ���� �̹� �� ������ ��ȯ�� �����Դϴ�!
- `MainTabView.SelectedItem` = ���� ������ �� (�� 2)
- `ItemsGridView.Items` = ���� ���� ������ (�⺻ ��)

���� �� ��(�� 2)�� Tag�� ���� ��(�⺻)�� �������� ����Ǿ� ����!

---

## ? �ذ� ���

### **�ٽ�: ���� �� ����**

```csharp
// ������ ���õǾ� �ִ� ���� ����
private Microsoft.UI.Xaml.Controls.TabViewItem _previousTab;
```

**���� ����:**
```
1. �⺻ �� ���� ��
   �� _previousTab = �⺻ ��
   �� ItemsGridView = App1, App2

2. �� 2 Ŭ�� (SelectionChanged �߻�)
   �� SaveCurrentTabItems() ȣ��
   �� _previousTab (�⺻ ��)�� Tag�� ���� ItemsGridView (App1, App2) ���� ?
   �� _previousTab = �� 2�� ������Ʈ
   �� LoadCurrentTabItems() ȣ��
   �� �� 2�� Tag���� ������ �ε� (����ְų� Game1, Game2)

3. �⺻ �� Ŭ�� (SelectionChanged �߻�)
   �� SaveCurrentTabItems() ȣ��
   �� _previousTab (�� 2)�� Tag�� ���� ItemsGridView (Game1, Game2) ���� ?
   �� _previousTab = �⺻ ������ ������Ʈ
   �� LoadCurrentTabItems() ȣ��
   �� �⺻ ���� Tag���� ������ �ε� (App1, App2) ?
```

---

## ?? ������ �ڵ�

### **1. �ʵ� �߰�**

```csharp
// Tab management - track previous tab for saving
private Microsoft.UI.Xaml.Controls.TabViewItem _previousTab;
```

---

### **2. InitializeTabs ����**

```csharp
private void InitializeTabs()
{
    // Create default tab
    var defaultTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    defaultTab.Header = "�⺻";
    defaultTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
        Symbol = Microsoft.UI.Xaml.Controls.Symbol.Home 
    };
    MainTabView.TabItems.Add(defaultTab);
    MainTabView.SelectedItem = defaultTab;
    
    // ? Set as previous tab
    _previousTab = defaultTab;
}
```

---

### **3. MainTabView_AddTabButtonClick ����**

```csharp
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
{
    // ? Save current tab items BEFORE creating new tab
    SaveCurrentTabItems();
    
    // Create new tab
    var newTab = new Microsoft.UI.Xaml.Controls.TabViewItem();
    newTab.Header = $"�� {MainTabView.TabItems.Count + 1}";
newTab.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource 
    { 
        Symbol = Microsoft.UI.Xaml.Controls.Symbol.Document 
    };
  
    // Add to TabView
    MainTabView.TabItems.Add(newTab);
    MainTabView.SelectedItem = newTab;
    
 // Clear items for new tab
    ItemsGridView.Items.Clear();
}
```

**���� ����:**
- ? �� �� ���� ���� ���� �� ����
- ? ��Ȯ�� �ּ� �߰�

---

### **4. MainTabView_SelectionChanged ���� (�ٽ�!)**

**Before (����):**

```csharp
private void MainTabView_SelectionChanged(...)
{
    // ? �̹� �� ������ ��ȯ�� ����
SaveCurrentTabItems();  // �� �ǿ� ���� �� ������ ���� ?
    
    // Load items for newly selected tab
    LoadCurrentTabItems();
}
```

**After (�ذ�):**

```csharp
private void MainTabView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
{
    // ? Save items from the PREVIOUS tab (before selection changed)
    if (_previousTab != null)
    {
      var items = new List<UserControl>();
        foreach (var item in ItemsGridView.Items)
        {
  if (item is UserControl control)
    {
           items.Add(control);
            }
        }
   _previousTab.Tag = items;  // ? ���� �ǿ� ����
    }
    
    // ? Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem newTab)
    {
     _previousTab = newTab;
    }
    
  // Load items for newly selected tab
    LoadCurrentTabItems();
}
```

**�ٽ� ����:**
1. ? `_previousTab`�� ����Ͽ� ���� �ǿ� ����
2. ? ���� �� `_previousTab` ������Ʈ
3. ? �� ���� �� �� �ε�

---

### **5. MainTabView_TabCloseRequested ����**

```csharp
private void MainTabView_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
{
    // Don't allow closing the last tab
    if (MainTabView.TabItems.Count <= 1)
    {
        return;
    }
    
    // ? Save current tab before closing
    SaveCurrentTabItems();
    
    // Remove the tab
    MainTabView.TabItems.Remove(args.Tab);

    // ? Update previous tab reference
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem selectedTab)
    {
        _previousTab = selectedTab;
    }
}
```

**���� ����:**
- ? �� �ݱ� ���� ����
- ? ���� �� `_previousTab` ������Ʈ

---

### **6. LoadCurrentTabItems �޼��� �߰�**

```csharp
/// <summary>
/// Load items from current tab's Tag to ItemsGridView
/// </summary>
private void LoadCurrentTabItems()
{
    ItemsGridView.Items.Clear();
    
    if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem currentTab)
    {
        if (currentTab.Tag is List<UserControl> items)
        {
 foreach (var item in items)
{
         if (item is GridViewTile tile)
 {
     // Re-register event handlers
  tile.Drop += GridViewTile_Drop;
      tile.DragEnter += GridViewTile_DragEnter;
   tile.DragLeave += GridViewTile_DragLeave;
         ItemsGridView.Items.Add(tile);
          }
      else if (item is GridViewTileGroup group)
         {
// Re-register event handlers
      group.DragEnter += GridViewTileGroup_DragEnter;
     group.DragLeave += GridViewTileGroup_DragLeave;
              group.Drop += GridViewTileGroup_Drop;
           ItemsGridView.Items.Add(group);
   }
            }
        }
    }
}
```

---

## ?? ���� �帧 ��

### **Before (����):**

```
[�⺻ �� ����] App1, App2 �߰�
    ��
[�� 2 Ŭ��]
    ��
SelectionChanged �߻�
    ���� MainTabView.SelectedItem = �� 2 (�̹� ��ȯ��)
 ���� ItemsGridView.Items = App1, App2 (���� �⺻ �� ������)
    ���� SaveCurrentTabItems() ȣ��
    ��   �� �� 2.Tag = App1, App2 ? (�߸��� ����!)
    ���� LoadCurrentTabItems() ȣ��
    �� �� 2.Tag���� �ε� �� App1, App2 ǥ�� ?

[�⺻ �� Ŭ��]
    ��
SelectionChanged �߻�
    ���� MainTabView.SelectedItem = �⺻ ��
    ���� ItemsGridView.Items = App1, App2 (�� 2 ��)
    ���� SaveCurrentTabItems() ȣ��
    ��   �� �⺻ ��.Tag = App1, App2 ?
    ���� LoadCurrentTabItems() ȣ��
        �� �⺻ ��.Tag���� �ε� �� App1, App2 ǥ�� ?

���: �� �� ��� ���� ������ ?
```

---

### **After (�ذ�):**

```
[�⺻ �� ����] App1, App2 �߰�
    ���� _previousTab = �⺻ ��
 ���� ItemsGridView = App1, App2

[�� 2 Ŭ��]
    ��
SelectionChanged �߻�
    ���� _previousTab = �⺻ �� (���� ������Ʈ �ȵ�)
    ���� ItemsGridView = App1, App2
    ���� SaveCurrentTabItems() ȣ��
    ��   �� _previousTab (�⺻ ��).Tag = App1, App2 ? (�ùٸ� ����!)
    ���� _previousTab = �� 2�� ������Ʈ
    ���� LoadCurrentTabItems() ȣ��
        �� �� 2.Tag���� �ε� �� ������� ?

[�� 2���� Game1, Game2 �߰�]
    ���� _previousTab = �� 2
    ���� ItemsGridView = Game1, Game2

[�⺻ �� Ŭ��]
    ��
SelectionChanged �߻�
    ���� _previousTab = �� 2
    ���� ItemsGridView = Game1, Game2
    ���� SaveCurrentTabItems() ȣ��
    ��   �� _previousTab (�� 2).Tag = Game1, Game2 ?
  ���� _previousTab = �⺻ ������ ������Ʈ
    ���� LoadCurrentTabItems() ȣ��
        �� �⺻ ��.Tag���� �ε� �� App1, App2 ǥ�� ?

���: �� ���� �������� ������ ���� ?
```

---

## ?? �ٽ� ����Ʈ

### **������ �ٺ� ����:**

```csharp
// SelectionChanged �̺�Ʈ�� �̹� ������ ����� AFTER�� �߻�
event SelectionChanged(...)
{
    // �� ������ MainTabView.SelectedItem�� �̹� �� ��!
    // ItemsGridView.Items�� ���� ���� ���� ������!
}
```

### **�ذ�å:**

```csharp
// ���� ���� �����Ͽ�, ���� ItemsGridView�� ���� �ǿ� ����
private TabViewItem _previousTab;  // �׻� ������ ���õǾ��� ��

SelectionChanged(...)
{
    // 1. _previousTab�� ���� ItemsGridView ���� ?
    _previousTab.Tag = ItemsGridView.Items;
    
    // 2. _previousTab ������Ʈ
    _previousTab = ���� ������ ��;
    
    // 3. �� �� �ε�
    LoadFrom(���� ������ ��.Tag);
}
```

---

## ?? �׽�Ʈ �ó�����

### **Test 1: �⺻ �ó�����**

```
1. �⺻ �ǿ� App1, App2 �߰� ?
2. "+" ��ư���� �� 2 ���� ?
3. �� 2 ������� Ȯ�� ?
4. �� 2�� Game1, Game2 �߰� ?
5. �⺻ �� Ŭ��
6. App1, App2�� ǥ�õ� ?
7. �� 2 Ŭ��
8. Game1, Game2�� ǥ�õ� ?
```

---

### **Test 2: ���� �� ��ȯ**

```
1. �⺻ �� �� App1, App2
2. �� 2 �� Game1, Game2
3. �� 3 �� Music1, Music2

��ȯ �׽�Ʈ:
- �⺻ �� �� 2 �� �⺻ ?
- �� 2 �� �� 3 �� �� 2 ?
- �⺻ �� �� 3 �� �⺻ ?

�� ���� �������� ������ ?
```

---

### **Test 3: �� �ݱ�**

```
1. �⺻ �� �� App1, App2
2. �� 2 �� Game1, Game2
3. �� 2 �ݱ�
4. �⺻ �� ���õ� ?
5. App1, App2 ǥ�õ� ?
```

---

### **Test 4: ������ �߰�/����**

```
1. �⺻ �� �� App1, App2
2. �� 2 ����
3. �� 2 �� Game1, Game2
4. �⺻ �� ��ȯ
5. App3 �߰�
6. �� 2 ��ȯ �� Game1, Game2�� ���� ?
7. �⺻ �� ��ȯ �� App1, App2, App3 ���� ?
```

---

## ?? ����� ��

### **���� Ȯ�� ���:**

```csharp
// SelectionChanged�� ����� �α� �߰�
private void MainTabView_SelectionChanged(...)
{
    System.Diagnostics.Debug.WriteLine($"[Before Save] Previous Tab: {_previousTab?.Header}");
    System.Diagnostics.Debug.WriteLine($"[Before Save] Current Items: {ItemsGridView.Items.Count}");
    
    if (_previousTab != null)
    {
   // ����...
        System.Diagnostics.Debug.WriteLine($"[After Save] Previous Tab Tag Count: {(_previousTab.Tag as List<UserControl>)?.Count}");
    }
    
    System.Diagnostics.Debug.WriteLine($"[After Update] New Previous Tab: {_previousTab?.Header}");
    
    LoadCurrentTabItems();
    System.Diagnostics.Debug.WriteLine($"[After Load] Items: {ItemsGridView.Items.Count}");
}
```

**��� ����:**
```
[Before Save] Previous Tab: �⺻
[Before Save] Current Items: 2 (App1, App2)
[After Save] Previous Tab Tag Count: 2
[After Update] New Previous Tab: �� 2
[After Load] Items: 0 (�������)
```

---

## ?? �н� ����Ʈ

### **1. �̺�Ʈ Ÿ�̹�**

```csharp
// SelectionChanged�� "�̹� ����� ��" �߻�
TabView.SelectedItem = newTab;  // ���⼭ ����
    ��
SelectionChanged �̺�Ʈ �߻�  // �̹� newTab���� �����
```

**����:**
- �̺�Ʈ �ȿ��� `SelectedItem`�� �� ��
- ���� ���� ������ �����ؾ� ��

---

### **2. ���� ������ �߿伺**

```csharp
// Bad: ���� ���¸� ����
SaveCurrent() �� SelectedItem ��� ?

// Good: ���� ���� ����
_previousTab �ʵ�� ���� ?
SavePrevious() �� _previousTab ��� ?
```

---

### **3. Tag �Ӽ� Ȱ��**

```csharp
// TabViewItem.Tag�� object Ÿ��
// � �����͵� ���� ����

tab.Tag = List<UserControl>;  // ?
tab.Tag = Dictionary<...>;    // ?
tab.Tag = CustomClass;        // ?
```

**����:**
- ������ ������ ����
- �߰� Ŭ���� ���ʿ�

**����:**
- Ÿ�� ĳ���� �ʿ�
- null üũ �ʿ�

---

## ? �Ϸ�!

### **������ �ڵ�:**
- ? `_previousTab` �ʵ� �߰�
- ? `InitializeTabs()` ����
- ? `MainTabView_AddTabButtonClick()` ����
- ? `MainTabView_SelectionChanged()` ������ ���ۼ� (�ٽ�!)
- ? `MainTabView_TabCloseRequested()` ����
- ? `LoadCurrentTabItems()` �޼��� �߰�

---

### **�ذ�� ����:**
- ? �� ��ȯ �� �������� ���̴� ����
- ? ��� ���� ���� �������� ǥ���ϴ� ����
- ? �� �ݱ� �� ������ �ս� ����

---

### **�׽�Ʈ:**

```
1. �⺻ �ǿ� ������ �߰� ?
2. �� �� ���� ?
3. �� �ǿ� �ٸ� ������ �߰� ?
4. �� ��ȯ ?
5. �� ���� �������� ������ ���� ?
6. ���� �� ��ȯ�ص� �������� ?
```

---

## ?? �Ϸ�!

**���� �� ���� ���������� �������� �����մϴ�!**

**�� ��ȯ �� �������� ������ �ʽ��ϴ�!** ?

**�׽�Ʈ�غ�����!** ??
