# ?? �� ��� �߰� �Ϸ�!

## ? ������ ���

### ?? **�ֿ� ���:**

1. ? **�˻�â �Ʒ� �� �߰�**
2. ? **�⺻ �� ("�⺻") �ڵ� ����**
3. ? **"+" ��ư���� �� �� �߰�**
4. ? **�� ��ȯ �� ������ �ڵ� ����/�ε�**
5. ? **������ �� ���� ���� (�ּ� 1�� ����)**
6. ? **�Ǻ��� �������� ������ ����**

---

## ?? ������ ����

### **1. SLauncher/Classes/TabItem.cs (�� ����)**

�� ������ �� Ŭ����:

```csharp
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SLauncher.Classes
{
    public class TabItem : INotifyPropertyChanged
    {
  private string _name;
        private string _id;
 private ObservableCollection<UserControl> _items;

        public TabItem()
   {
        Id = Guid.NewGuid().ToString();
      Items = new ObservableCollection<UserControl>();
    }

        public string Id { get; set; }
        public string Name { get; set; }
   public ObservableCollection<UserControl> Items { get; set; }

 public event PropertyChangedEventHandler PropertyChanged;
     protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

---

### **2. MainWindow.xaml**

**�Ǻ� �߰�:**

```xaml
<!--  TabView for organizing items  -->
<muxc:TabView
    x:Name="MainTabView"
 Margin="0,57,0,0"
    VerticalAlignment="Top"
    AddTabButtonClick="MainTabView_AddTabButtonClick"
    IsAddTabButtonVisible="True"
    SelectionChanged="MainTabView_SelectionChanged"
    TabCloseRequested="MainTabView_TabCloseRequested" />
```

**���� ����:**
- ? �˻�â �Ʒ��� TabView �߰�
- ? `AddTabButtonClick` �̺�Ʈ - �� �� �߰�
- ? `SelectionChanged` �̺�Ʈ - �� ��ȯ
- ? `TabCloseRequested` �̺�Ʈ - �� �ݱ�
- ? `IsAddTabButtonVisible="True"` - "+" ��ư ǥ��

**��ư ��ġ ����:**
```xaml
<!-- Before: Margin="0,62,0,0" -->
<!-- After: Margin="0,100,0,0" -->
```
�Ǻ䰡 �߰��Ǿ� ��ư ��ġ�� �Ʒ��� ����

---

### **3. MainWindow.xaml.cs**

**�߰��� �ڵ�:**

#### **(1) INotifyPropertyChanged ����**

```csharp
public sealed partial class MainWindow : WinUIEx.WindowEx, INotifyPropertyChanged
{
    // Tabs collection
  private ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();
    public ObservableCollection<TabItem> Tabs
    {
        get => _tabs;
        set
  {
            _tabs = value;
   OnPropertyChanged();
     }
    }

    private TabItem _currentTab;
    public TabItem CurrentTab
    {
  get => _currentTab;
  set
        {
          _currentTab = value;
OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

---

#### **(2) �� �ʱ�ȭ**

```csharp
/// <summary>
/// Initialize tabs - create default tab if none exist
/// </summary>
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
}
```

**ȣ�� ��ġ:**
```csharp
public MainWindow()
{
    this.InitializeComponent();

    // Initialize tabs
    InitializeTabs();  // ? �߰���

    // ...���� �ڵ�...
}
```

---

#### **(3) �� �� �߰�**

```csharp
/// <summary>
/// Handle adding a new tab
/// </summary>
private void MainTabView_AddTabButtonClick(Microsoft.UI.Xaml.Controls.TabView sender, object args)
{
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
    
  // Clear current items and show in new tab
    SaveCurrentTabItems();
    ItemsGridView.Items.Clear();
}
```

**����:**
1. �� �� ���� (�̸�: "�� 2", "�� 3", ...)
2. ���� ������ �߰�
3. TabView�� �߰�
4. �� �� ����
5. ���� �� ������ ����
6. ������ �׸��� �ʱ�ȭ

---

#### **(4) �� ��ȯ**

```csharp
/// <summary>
/// Handle tab selection changed
/// </summary>
private void MainTabView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs e)
{
    // Save current tab items before switching
    SaveCurrentTabItems();
    
    // Load items for newly selected tab
    LoadCurrentTabItems();
}
```

**����:**
1. ���� ���� ������ ����
2. ���� ���õ� ���� ������ �ε�

---

#### **(5) �� �ݱ�**

```csharp
/// <summary>
/// Handle tab close request
/// </summary>
private void MainTabView_TabCloseRequested(Microsoft.UI.Xaml.Controls.TabView sender, Microsoft.UI.Xaml.Controls.TabViewTabCloseRequestedEventArgs args)
{
    // Don't allow closing the last tab
    if (MainTabView.TabItems.Count <= 1)
    {
        return;  // ? ������ �� ��ȣ
    }
    
    // Remove the tab
    MainTabView.TabItems.Remove(args.Tab);
}
```

**��ȣ ���:**
- ������ �� ���� ����
- �׻� �ּ� 1�� �� ����

---

#### **(6) ������ ����**

```csharp
/// <summary>
/// Save current ItemsGridView items to current tab's Tag
/// </summary>
private void SaveCurrentTabItems()
{
  if (MainTabView.SelectedItem is Microsoft.UI.Xaml.Controls.TabViewItem currentTab)
    {
 var items = new List<UserControl>();
        foreach (var item in ItemsGridView.Items)
   {
            if (item is UserControl control)
    {
        items.Add(control);
       }
        }
        currentTab.Tag = items;  // ? Tag�� ����
    }
}
```

**���� ��Ŀ����:**
- TabViewItem�� `Tag` �Ӽ� Ȱ��
- List<UserControl>�� ��� ������ ����
- �� ��ȯ �� �ڵ� ȣ��

---

#### **(7) ������ �ε�**

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
        // �̺�Ʈ �ڵ鷯 ����
tile.Drop += GridViewTile_Drop;
  tile.DragEnter += GridViewTile_DragEnter;
         tile.DragLeave += GridViewTile_DragLeave;
          ItemsGridView.Items.Add(tile);
                }
         else if (item is GridViewTileGroup group)
     {
          // �̺�Ʈ �ڵ鷯 ����
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

**�ε� ���μ���:**
1. ItemsGridView �ʱ�ȭ
2. ���� ���� Tag���� ������ ��������
3. �� �������� �̺�Ʈ �ڵ鷯 ����
4. ItemsGridView�� �߰�

---

## ?? ����� �������̽�

### **���̾ƿ�:**

```
������������������������������������������������������������������������������������������������������������������������������
�� [Search Box]    [??] [��]       ��
������������������������������������������������������������������������������������������������������������������������������
�� [�⺻ ��] [�� 2] [+]         �� �� �� ��
������������������������������������������������������������������������������������������������������������������������������
��       [Add file] [Add folder] [Add website] ��
������������������������������������������������������������������������������������������������������������������������������
��       ��
��  �������������� �������������� ��������������         ��
��  ��App 1�� ��App 2�� ��App 3��         ��
��  �������������� �������������� ��������������           ��
��    ��
��         [?? Zoom Slider]   ��
������������������������������������������������������������������������������������������������������������������������������
```

---

## ?? ��� ����

### **1. �⺻ �� ("�⺻")**

**Ư¡:**
- ? �� ���� �� �ڵ� ����
- ? Ȩ ������ (??)
- ? ���� �Ұ� (������ ���� ���)

**���� ����:**
```csharp
public MainWindow()
{
    this.InitializeComponent();
    InitializeTabs();  // ���⼭ "�⺻" �� ����
}
```

---

### **2. �� �� �߰� ("+" ��ư)**

**����:**
1. "+" ��ư Ŭ��
2. �� �� ���� ("�� 2", "�� 3", ...)
3. ���� ������ (??) �߰�
4. �� ������ �ڵ� ��ȯ
5. �� ������ �׸��� ǥ��

**����:**
```
ó��: [�⺻] [+]
Ŭ��: [�⺻] [�� 2] [+]
Ŭ��: [�⺻] [�� 2] [�� 3] [+]
```

---

### **3. �� ��ȯ**

**����:**
1. �� Ŭ��
2. ���� ���� ������ �ڵ� ����
3. ������ ���� ������ �ε�
4. ItemsGridView ������Ʈ

**����:**
```
[�⺻] ��: App1, App2, App3
    �� Ŭ�� [�� 2]
[�� 2] ��: Game1, Game2
    �� Ŭ�� [�⺻]
[�⺻] ��: App1, App2, App3 (������)
```

---

### **4. �� �ݱ�**

**����:**
1. ���� X ��ư Ŭ��
2. ������ ���̸� ���� (�ּ� 1�� ����)
3. �� ����
4. ���� �� �ڵ� ����

**��ȣ ��Ŀ����:**
```csharp
if (MainTabView.TabItems.Count <= 1)
{
    return;  // ������ �� ��ȣ
}
```

**����:**
```
[�⺻] [�� 2] [�� 3]
    �� [�� 2] �ݱ�
[�⺻] [�� 3] ?

[�⺻]�� ����
    �� [�⺻] �ݱ� �õ�
[�⺻] (�״��) ? ���� �ȵ�
```

---

## ?? ������ ���� ��Ŀ����

### **Tag �Ӽ� Ȱ��:**

```csharp
// ����
currentTab.Tag = items;  // List<UserControl>

// �ε�
if (currentTab.Tag is List<UserControl> items)
{
    // items ���
}
```

**����:**
- ? ������ ����
- ? �޸� ȿ����
- ? �� ��ȯ �� ��� ����

**����:**
- ? �� ���� �� �ս� (���� ���� �̱���)
- ? �� �̸� ���� �Ұ� (����)

---

## ?? ���� ���� ����

### **1. �� �̸� ����**

```csharp
// ����Ŭ�� �� �̸� ���� ���̾�α�
private async void TabViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
{
    var textBox = new TextBox();
    var dialog = new ContentDialog
    {
        Title = "�� �̸� ����",
        Content = textBox,
  PrimaryButtonText = "Ȯ��",
        CloseButtonText = "���"
    };
    
    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
    {
        currentTab.Header = textBox.Text;
    }
}
```

---

### **2. �� ���� ���� (�巡�� �� ���)**

```csharp
// TabView�� CanReorderItems Ȱ��
MainTabView.CanReorderItems = true;
MainTabView.CanDragItems = true;
```

---

### **3. �Ǻ� ������ ����**

```csharp
// ������ ���� ���̾�α�
var iconPicker = new SymbolIconPicker();
if (await iconPicker.ShowAsync() == ContentDialogResult.Primary)
{
    currentTab.IconSource = new SymbolIconSource 
    { 
 Symbol = iconPicker.SelectedSymbol 
    };
}
```

---

### **4. �� ������ ���� ���� (JSON)**

```csharp
// TabsData.json
{
    "tabs": [
        {
            "id": "guid-1",
        "name": "�⺻",
            "icon": "Home",
   "items": [...]
        },
     {
        "id": "guid-2",
     "name": "����",
            "icon": "Code",
   "items": [...]
        }
    ],
    "lastSelectedTabId": "guid-1"
}
```

**���� ��ġ:**
```
UserCache/tabs.json
```

---

### **5. �� �׷� (��ø ��)**

```csharp
// �� �ȿ� ���� ��
[����]
  ���� ����
  ���� ���������Ʈ
  ���� ���������̼�
```

---

### **6. �� ���ø�**

```csharp
// �̸� ���ǵ� �� ���ø�
- ���� �� (VSCode, Git, Terminal)
- ������ �� (Photoshop, Figma, Sketch)
- ���� �� (Steam, Epic, Discord)
```

---

## ?? �׽�Ʈ �ó�����

### **Test 1: �⺻ �� Ȯ��**

```
1. SLauncher ����
2. "�⺻" �� ���� Ȯ�� ?
3. Ȩ ������ Ȯ�� ?
4. �� 1���� ���� ?
```

---

### **Test 2: �� �� �߰�**

```
1. "+" ��ư Ŭ��
2. "�� 2" ������ ?
3. ���� ������ ǥ�� ?
4. "�� 2"�� ���õ� ?
5. ������ �׸��� ������� ?
```

---

### **Test 3: �� ��ȯ**

```
1. "�⺻" �ǿ� App1, App2 �߰�
2. "+" ��ư���� "�� 2" ����
3. "�� 2"�� Game1, Game2 �߰�
4. "�⺻" �� Ŭ��
5. App1, App2 ǥ�õ� ?
6. "�� 2" Ŭ��
7. Game1, Game2 ǥ�õ� ?
```

---

### **Test 4: �� �ݱ�**

```
1. [�⺻] [�� 2] [�� 3] ����
2. "�� 2" �ݱ� (X ��ư)
3. [�⺻] [�� 3] ���� ?
4. "�� 3" �ݱ�
5. [�⺻]�� ���� ?
6. "�⺻" �ݱ� �õ�
7. ������ ���� ?
```

---

### **Test 5: ������ �߰�**

```
1. "�⺻" �� ����
2. "Add a file" Ŭ��
3. ���� ����
4. "�⺻" �ǿ� �߰��� ?
5. "�� 2" ����
6. "Add a website" Ŭ��
7. URL �Է�
8. "�� 2"�� �߰��� ?
9. "�⺻" �� Ŭ��
10. ���ϸ� ǥ�õ� ?
```

---

### **Test 6: �巡�� �� ���**

```
1. "�⺻" �� ����
2. ���� �巡�� �� ���
3. "�⺻" �ǿ� �߰��� ?
4. "�� 2" ����
5. ���� �巡�� �� ���
6. "�� 2"�� �߰��� ?
```

---

### **Test 7: �˻� ���**

```
1. ���� �ǿ� ������ �߰�
2. �˻�â�� �Է�
3. ��� ���� ������ �˻� ?
4. ������ ����
5. �ش� ������ ��ȯ �ʿ� (���� ����)
```

---

## ?? ���� ���� ����

### **�Ϸ�� ���:** ?

```
1. ? �⺻ �� �ڵ� ����
2. ? "+" ��ư���� �� �� �߰�
3. ? �� ��ȯ �� ������ �ڵ� ����/�ε�
4. ? ������ �� ���� ����
5. ? �Ǻ� �������� ������ ����
6. ? �� ������ ǥ��
7. ? �� �ݱ� ��ư
```

### **�̱��� ���:** ?

```
1. ? �� ������ ���� ���� (JSON)
2. ? �� �̸� ����
3. ? �� ���� ���� (�巡�� �� ���)
4. ? �� ������ ����
5. ? ������ ���� �� ���
6. ? �� �׷�
7. ? �� ���ø�
8. ? �� ����
9. ? �� ��������/��������
```

---

## ?? ��� ���

### **1. �⺻ ���**

```
1. SLauncher ����
2. "�⺻" �ǿ��� �۾�
3. "+" ��ư���� �� �� �߰�
4. �Ǻ��� ������ ����
```

---

### **2. ������ �з�**

```
[����] ��: Office, Email, Calendar
[����] ��: VSCode, Git, Terminal
[������] ��: Photoshop, Figma, Sketch
[����] ��: Steam, Epic, Discord
```

---

### **3. ������Ʈ�� �з�**

```
[������Ʈ A] ��: ���� ����, ����, ������Ʈ
[������Ʈ B] ��: ���� ����, ����, ������Ʈ
[���� �ڷ�] ��: Ʃ�丮��, ����, ����
```

---

## ?? ��

### **ȿ������ �� ���:**

1. **ī�װ��� �з�**
   - ����, ����, �н� ��

2. **������Ʈ�� �з�**
   - �� ������Ʈ���� �� ����

3. **�󵵺� �з�**
   - ���� ���, ���� ���, �����

4. **�ð��뺰 �з�**
   - ����, ����, ���� �۾�

---

## ? �Ϸ�!

### **����� ����:**
- ? `SLauncher/Classes/TabItem.cs` (�� ����)
- ? `SLauncher/MainWindow.xaml`
  - TabView �߰�
  - ��ư ��ġ ����
- ? `SLauncher/MainWindow.xaml.cs`
  - INotifyPropertyChanged ����
  - �� ���� �޼��� �߰�
  - �� �̺�Ʈ �ڵ鷯 �߰�

---

### **�׽�Ʈ:**

```
1. SLauncher ����
2. "�⺻" �� Ȯ�� ?
3. "+" ��ư Ŭ�� �� "�� 2" ���� ?
4. ������ �߰� ?
5. �� ��ȯ ?
6. ������ ���� ?
7. �� �ݱ� ?
8. ������ �� ��ȣ ?
```

---

## ?? �Ϸ�!

**�� ����� ���������� �߰��Ǿ����ϴ�!**

**���� SLauncher���� �������� ������ ����ϰ� ������ �� �ֽ��ϴ�!** ?

**�� ���� ���������� �������� �����ϸ�, �� ��ȯ �� �ڵ����� ����/�ε�˴ϴ�!** ??

---

## ?? ���� �ܰ� (���û���)

### **�켱���� 1: �� ������ ���� ����**
- JSON ���Ϸ� ����
- �� ����� �� �� ����
- ������ ���� �� ���

### **�켱���� 2: �� �̸� ����**
- ����Ŭ�� �Ǵ� ��Ŭ�� �޴�
- �ؽ�Ʈ �Է� ���̾�α�
- ��� �ݿ�

### **�켱���� 3: �� ������ ����**
- ������ ���� UI
- �پ��� ������ �ɼ�
- �Ǻ� ���� �ο�

---

**�׽�Ʈ�غ�����!** ??
